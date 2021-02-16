﻿using OVFSliceViewer.Classes;
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
        public OVFSliceViewer(bool showLoadButton): this()
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
            _painter.Draw();
        }

        private async void DrawWorkplane()
        {
            int layernumber = layerTrackBar.Value;
            var mapper = new VectorblockToLineMapper();
            mapper.HightlightIndex = checkHighlightIndex;
            //mapper.HightlightIndex = 
            int fromLayer;

            fromLayer = threeDCheckbox.Checked ? 0 : layernumber;
            _painter.DrawableParts.ToList().ForEach(x => { x.Value.RemoveVolume(); x.Value.SetRangeToDraw(layernumber, fromLayer); });
            _painter.Draw();
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
            var vertices = mapper.GetVertices();
            _numberOfLines = vertices.Count() / 2;
            _painter.DrawableParts.ToList().ForEach(x => { x.Value.SetRangeToDraw(layernumber, fromLayer); x.Value.UpdateVolume(); });
            _painter.SetLinesAndDraw(vertices, _numberOfLines);

            mapper.Dispose();
            mapper = null;
        }

        private void SetTimeTrackBar(int numberOfLines)
        {
            timeTrackBar.Maximum = numberOfLines;
            timeTrackBar.Value = numberOfLines;
        }

        private void LoadJobButtonClick(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "ovf files (*.ovf)|*.ovf|All files (*.*)|*.*";
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
                _painter.DrawableParts.Add(item, new DrawablePart(shader, buffer[i], buffer[i + 1], _currentFile.JobShell.NumWorkPlanes));
                i += 2;
            }

            _viewerJob = new JobViewer(_currentFile);

            layerTrackBar.Maximum = _currentFile.JobShell.NumWorkPlanes - 1;
            layerTrackBar.Value = 0;

            Console.WriteLine(_viewerJob.Center.ToString());
            _painter.Camera.MoveToPosition2D(_viewerJob.Center);

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
                    var workplane = await _currentFile.GetWorkPlaneAsync(j);
                    var blocks = workplane.VectorBlocks;
                    var numBlocks = blocks.Count();

                    for (int i = 0; i < numBlocks; i++)
                    {
                        if (blocks[i].LpbfMetadata.PartArea == VectorBlock.Types.PartArea.Contour)
                        {
                            _painter.DrawableParts[blocks[i].MetaData.PartKey].AddContour(blocks[i], workplane.WorkPlaneNumber , workplane.ZPosInMm);
                        }
                    }
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
            _painter.Draw();
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
            if(e.NewValue == CheckState.Checked)
            {
                checkHighlightIndex = e.Index+1;
            }
            else
            {
                checkHighlightIndex = 0;
            }
            DrawWorkplane();
            
        }
        private void gridCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            _painter.ShowGrid = gridCheckbox.Checked;
            DrawWorkplane();
        }
    }
}
