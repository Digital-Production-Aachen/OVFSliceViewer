using OVFSliceViewer.Classes;
using OpenTK;
using OpenVectorFormat;
using OpenVectorFormat.AbstractReaderWriter;
using ProceduralControl;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using OpenVectorFormat.FileReaderWriterFactory;
using LayerViewer;
using System.Collections.Generic;
using ModularEmulator.ProductView;

namespace OVFSliceViewer
{
    public partial class OVFSliceViewer : Form
    {
        JobViewer _viewerJob;
        ProgressStatus statusForm;
        Painter _painter;
        MotionTracker _motionTracker;
        int _numberOfLines = 3;
        private int checkHighlightIndex = 0;
        FileReader _currentFile { get; set; }
        public OVFSliceViewer()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            //highlightCheckedListBox.SetItemChecked(0, true);//always start with highlighted  contours
            this.glCanvas.MouseWheel += new MouseEventHandler(this.MouseWheelZoom);
            _painter = new Painter(glCanvas, this);
            _motionTracker = new MotionTracker();
        }
        public OVFSliceViewer(bool showLoadButton) : this()
        {
            this.loadFileButton.Visible = showLoadButton;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            _painter.DisposeShader();
            base.OnClosing(e);
        }

        private void DisposeShader(CancelEventArgs e)
        {

        }

        private void MouseWheelZoom(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                _painter.Camera.Zoom(true);
            }
            else
            {
                _painter.Camera.Zoom(false);
            }
            _painter.Draw(layerTrackBar.Value);
        }

        private async void DrawWorkplane()
        {
            int layernumber = layerTrackBar.Value;
            int fromLayer;

            fromLayer = threeDCheckbox.Checked ? 0 : layernumber;
            _painter.DrawableParts.ToList().ForEach(x => { x.Value.RemoveVolume(); x.Value.SetContourRangeToDraw3d(layernumber, fromLayer); });
            if (threeDCheckbox.Checked)
            {
                _painter.Draw(layernumber);
            }
            
            if (layernumber != layerTrackBar.Value)
            {
                return;
            }


            for (int j = layernumber; j < layernumber + 1; j++)
            {
                if (_currentFile != null)
                {
                    var workplane = await _currentFile.GetWorkPlaneAsync(j);
                    if (layernumber != layerTrackBar.Value)
                    {
                        return;
                    }
                    var blocks = workplane.VectorBlocks;
                    var numBlocks = blocks.Count();

                    for (int i = 0; i < numBlocks; i++)
                    {
                        if (blocks[i].LpbfMetadata.PartArea != VectorBlock.Types.PartArea.Contour && j == layernumber)
                        {
                            _painter.DrawableParts[blocks[i].MetaData.PartKey].AddVectorBlock(blocks[i], workplane.WorkPlaneNumber, workplane.ZPosInMm);
                            //mapper.CalculateVectorBlock(blocks[i], workplane.ZPosInMm);
                        }
                    }
                }
            }
            //GC.Collect();
            _numberOfLines = _painter.LayerPointManager[layernumber].GetPointNumbersToDraw(null).Sum(x => x.Value.HatchNumberOfPoints + x.Value.ContourNumberOfPoints);
            if (threeDCheckbox.Checked)
            {
                _painter.DrawableParts.ToList().ForEach(x => { x.Value.SetContourRangeToDraw3d(layernumber, fromLayer); x.Value.UpdateVolume(); });
            }
            else
            {
                _painter.DrawableParts.ToList().ForEach(x => { x.Value.UpdateVolume(); });
            }
            //_painter.SetLinesAndDraw(vertices, _numberOfLines);

            SetTimeTrackBar(_numberOfLines);
            _painter.SetNumberOfLinesToDraw(_numberOfLines);
            _painter.Draw(layernumber);
        }

        private void SetTimeTrackBar(int numberOfLines)
        {
            timeTrackBar.Maximum = numberOfLines;
            timeTrackBar.Value = numberOfLines;
        }

        private void LoadJobButtonClick(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "ovf and ilt files (*.ovf; *.ilt)|*.ovf;*.ilt|All files (*.*)|*.*";
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                //openFileDialog1.FileNames;
                var filename = openFileDialog1.FileNames[0];

                LoadJob(filename);
            }
        }

        public async void LoadJob(string filename)
        {
            _currentFile = FileReaderFactory.CreateNewReader(Path.GetExtension(filename));

            var command = new JobExecutionCommand
            {
                FileReader = _currentFile,
                FilePath = openFileDialog1.FileNames[0],
            };

            await _currentFile.OpenJobAsync(filename, command);

            _painter.DrawableParts = new Dictionary<int, DrawablePart>();
            var shader = _painter.GetShader();
            int i = 1;
            var buffer = _painter.GetBufferPointer(_currentFile.JobShell.PartsMap.Count * 2);
            foreach (var item in _currentFile.JobShell.PartsMap.Keys)
            {
                _painter.DrawableParts.Add(item, new DrawablePart(shader, buffer[i], buffer[i + 1], _currentFile.JobShell.NumWorkPlanes, item));
                i += 2;
            }

            _viewerJob = new JobViewer(_currentFile);

            layerTrackBar.Maximum = _currentFile.JobShell.NumWorkPlanes - 1;
            layerTrackBar.Value = 0;

            Console.WriteLine(_viewerJob.Center.ToString());
            _painter.Camera.MoveToPosition2D(_viewerJob.Center);
            layerNumberLabel.Text = "Layer: " + layerTrackBar.Value + " von " + layerTrackBar.Maximum;
            LoadContours();
        }

        private async void LoadContours()
        {
            int layernumber = layerTrackBar.Value;
            //mapper.HightlightIndex = 
            int fromLayer = 0;

            for (int j = fromLayer; j < _currentFile.JobShell.NumWorkPlanes; j++)
            {
                if (_currentFile != null)
                {
                    var pointOrderManagement = new PointOrderManagement(j);
                    var workplane = await _currentFile.GetWorkPlaneAsync(j);
                    var blocks = workplane.VectorBlocks;
                    var numBlocks = blocks.Count();
                    var numberOfPoints = 0;

                    for (int i = 0; i < numBlocks; i++)
                    {
                        bool isContour = blocks[i].LpbfMetadata.PartArea == VectorBlock.Types.PartArea.Contour;
                        if (blocks[i].VectorDataCase == VectorBlock.VectorDataOneofCase.LineSequence)
                        {
                            numberOfPoints = (blocks[i].LineSequence.Points.Count/2 -1);
                        }
                        else if (blocks[i].VectorDataCase == VectorBlock.VectorDataOneofCase.Hatches)
                        {
                            numberOfPoints = blocks[i].Hatches.Points.Count/2;
                        }
                        else if (blocks[i].VectorDataCase == VectorBlock.VectorDataOneofCase.HatchParaAdapt)
                        {
                            numberOfPoints = blocks[i].HatchParaAdapt.HatchAsLinesequence.Sum(x => x.PointsWithParas.Count/3*2-1);
                        }
                        else if (blocks[i].VectorDataCase == VectorBlock.VectorDataOneofCase.LineSequenceParaAdapt)
                        {
                            numberOfPoints = blocks[i].LineSequenceParaAdapt.PointsWithParas.Count / 3*2-1;
                        }

                        pointOrderManagement.AddVectorblockInfo(new PartVectorblockInfo() { Partnumber = blocks[i].MetaData.PartKey, NumberOfPoints = numberOfPoints, IsContour = isContour });

                        if (blocks[i].LpbfMetadata.PartArea == VectorBlock.Types.PartArea.Contour)
                        {
                            _painter.DrawableParts[blocks[i].MetaData.PartKey].AddContour(blocks[i], workplane.WorkPlaneNumber, workplane.ZPosInMm);
                        }
                    }
                    _painter.LayerPointManager[j] = pointOrderManagement;
                }
            }
            //GC.Collect();
            _painter.DrawableParts.ToList().ForEach(x => x.Value.UpdateContour());
        }

        public async void LoadJob(FileReader fileReader, string filePath)
        {
            if (fileReader.CacheState == CacheState.NotCached)
            {
                //var task = fileReader.OpenJobAsync(filePath, new Classes.FileHandlerProgress());
                var task = fileReader.CacheJobToMemoryAsync();
                //ToDO: show a progressbar
                //statusForm.Show();
                //var temp = task.Result;
                task.Wait();
            }
            _currentFile = fileReader;
            layerTrackBar.Value = 0;
            layerTrackBar.Maximum = _currentFile.JobShell.NumWorkPlanes - 1;
            // DrawWorkplaneBeforePaint(true);
            layerNumberLabel.Text = "Layer: " + layerTrackBar.Value + " von " + layerTrackBar.Maximum;
        }

        private void layerTrackBarScroll(object sender, EventArgs e)
        {
            DrawWorkplane();
            layerNumberLabel.Text = "Layer: " + layerTrackBar.Value + " von " + layerTrackBar.Maximum;
        }

        private void timeTrackBarScroll(object sender, EventArgs e)
        {
            if (timeTrackBar.Maximum != _numberOfLines)
            {
                timeTrackBar.Maximum = _numberOfLines;
            }
            _painter.SetNumberOfLinesToDraw(timeTrackBar.Value);
            _painter.Draw(layerTrackBar.Value);
        }

        private void canvasMouseDown(object sender, MouseEventArgs e)
        {
            var position = new Vector2(MousePosition.X, MousePosition.Y);

            if (e.Button == MouseButtons.Middle || e.Button == MouseButtons.Left)
            {
                _motionTracker.Start(position);
            }
        }

        private void canvasMoveMouseUp(object sender, MouseEventArgs e)
        {
            var position = new Vector2(MousePosition.X, MousePosition.Y);
            if (e.Button == MouseButtons.Middle || e.Button == MouseButtons.Left)
            {
                _motionTracker.Stop();
            }
        }

        private void canvasMouseMove(object sender, MouseEventArgs e)
        {
            var position = new Vector2(MousePosition.X, MousePosition.Y);

            if (e.Button == MouseButtons.Middle)
            {
                _motionTracker.Move(position, _painter.Camera.Move);
            }
            else if (e.Button == MouseButtons.Left)
            {
                _motionTracker.Move(position, _painter.Camera.Rotate);
            }
            _painter.Draw(layerTrackBar.Value);
        }
        private void layerTrackBarMouseUp(object sender, MouseEventArgs e)
        {
            SetTimeTrackBar(_numberOfLines);
        }

        private void canvasMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _painter.TargetCenter();
            }
        }

        private void threeDCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            DrawWorkplane();
        }

        private void highlightCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var c = sender as CheckedListBox;
            if (e.NewValue == CheckState.Checked && c.CheckedItems.Count > 0)
            {
                c.ItemCheck -= highlightCheckedListBox_ItemCheck;
                c.SetItemChecked(c.CheckedIndices[0], false);
                c.ItemCheck += highlightCheckedListBox_ItemCheck;
            }
            if (e.NewValue == CheckState.Checked)
            {
                checkHighlightIndex = e.Index + 1;
            }
            else
            {
                checkHighlightIndex = 0;
            }
            _painter.SetHighlightColors(checkHighlightIndex);
            DrawWorkplane();
        }
        private void gridCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            _painter.ShowGrid = gridCheckbox.Checked;
            DrawWorkplane();
        }
    }
}
