using OVFSliceViewer.Classes;
using OpenTK;
using OpenVectorFormat.AbstractReaderWriter;
using System;
using System.Windows.Forms;
using System.ComponentModel;
using LayerViewer;
using System.Drawing;
using OVFSliceViewer.Classes.ShaderNamespace;

namespace OVFSliceViewer
{
    public partial class OVFSliceViewer : Form
    {
        ViewerAPI _viewerAPI;
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
            var painter = new Painter(glCanvas, this);
            _motionTracker = new MotionTracker();

            _viewerAPI = new ViewerAPI(painter);
        }
        public OVFSliceViewer(bool showLoadButton) : this()
        {
            this.loadFileButton.Visible = showLoadButton;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            _viewerAPI.Dispose();
            base.OnClosing(e);
        }

        private void MouseWheelZoom(object sender, MouseEventArgs e)
        {
            _viewerAPI.Zoom(e.Delta);
        }

        private async void DrawWorkplane()
        {
            await _viewerAPI.DrawWorkplane(layerTrackBar.Value);
            _numberOfLines = _viewerAPI.NumberOfLines;
            SetTimeTrackBar(_numberOfLines);
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
            await _viewerAPI.LoadJob(filename);
            layerTrackBar.Maximum = _viewerAPI.NumberOfLayer - 1;
            layerTrackBar.Value = _viewerAPI.CurrentLayer;
            SetTrackBarText();
        }

        //private async void LoadContours()
        //{
        //    int layernumber = layerTrackBar.Value;
        //    //mapper.HightlightIndex = 
        //    int fromLayer = 0;
        //    float minPower = 0;
        //    float maxPower = 1;

        //    for (int j = fromLayer; j < _currentFile.JobShell.NumWorkPlanes; j++)
        //    {
        //        if (_currentFile != null)
        //        {
        //            var pointOrderManagement = new PointOrderManagement(j);
        //            var workplane = await _currentFile.GetWorkPlaneAsync(j);


        //            if (workplane.MetaData != null && workplane.MetaData.MaxPower != 0 && workplane.MetaData.MinPower != 0)
        //            {
        //                if (minPower > (workplane.MetaData.MinPower))
        //                {
        //                    minPower = workplane.MetaData.MinPower;
        //                }
        //                if (maxPower < workplane.MetaData.MaxPower)
        //                {
        //                    maxPower = workplane.MetaData.MaxPower;
        //                }
        //            }

        //            var blocks = workplane.VectorBlocks;
        //            var numBlocks = blocks.Count();
        //            var numberOfPoints = 0;

        //            for (int i = 0; i < numBlocks; i++)
        //            {
        //                bool isContour = blocks[i].LpbfMetadata != null ? blocks[i].LpbfMetadata.PartArea == VectorBlock.Types.PartArea.Contour : true;
        //                if (blocks[i].VectorDataCase == VectorBlock.VectorDataOneofCase.LineSequence)
        //                {
        //                    numberOfPoints = (blocks[i].LineSequence.Points.Count/2 -1);
        //                }
        //                else if (blocks[i].VectorDataCase == VectorBlock.VectorDataOneofCase.Hatches)
        //                {
        //                    numberOfPoints = blocks[i].Hatches.Points.Count/2;
        //                }
        //                else if (blocks[i].VectorDataCase == VectorBlock.VectorDataOneofCase.HatchParaAdapt)
        //                {
        //                    numberOfPoints = blocks[i].HatchParaAdapt.HatchAsLinesequence.Sum(x => x.PointsWithParas.Count/3*2-1);
        //                }
        //                else if (blocks[i].VectorDataCase == VectorBlock.VectorDataOneofCase.LineSequenceParaAdapt)
        //                {
        //                    numberOfPoints = blocks[i].LineSequenceParaAdapt.PointsWithParas.Count / 3*2-1;
        //                }
        //                var newInfo = new PartVectorblockInfo() { Partnumber = blocks[i].MetaData != null ? blocks[i].MetaData.PartKey : 0, NumberOfPoints = numberOfPoints, IsContour = isContour };
        //                pointOrderManagement.AddVectorblockInfo(newInfo);

        //                if (blocks[i].LpbfMetadata == null || blocks[i].LpbfMetadata.PartArea == VectorBlock.Types.PartArea.Contour)
        //                {
        //                    _painter.DrawableParts[newInfo.Partnumber].AddContour(blocks[i], workplane.WorkPlaneNumber, workplane.ZPosInMm);
        //                }
        //            }
        //            _painter.LayerPointManager[j] = pointOrderManagement;
        //        }
        //    }
        //    //GC.Collect();
        //    _painter.DrawableParts.ToList().ForEach(x => { x.Value.UpdateContour(); x.Value.VectorFactory.SetPowerLevels(minPower, maxPower); });
        //}

        public async void LoadJob(FileReader fileReader, string filePath)
        {
            await _viewerAPI.LoadJob(fileReader, filePath);
            
            layerTrackBar.Maximum = _viewerAPI.NumberOfLayer-1;
            layerTrackBar.Value = _viewerAPI.CurrentLayer;

            SetTrackBarText();
            LoadPartNames();
        }
        
        private void LoadPartNames()
        {
            var names = _viewerAPI.LoadPartNames();
            if(_currentFile.CacheState != CacheState.NotCached)
            {
                partsCheckedListBox.Items.Clear();
                foreach (var name in names)
                {
                    partsCheckedListBox.Items.Add(name);
                }
            }
        }

        private void layerTrackBarScroll(object sender, EventArgs e)
        {
            DrawWorkplane();
            SetTrackBarText();
        }
        private void SetTrackBarText()
        {
            layerNumberLabel.Text = "Layer: " + _viewerAPI.CurrentLayer + " von " + _viewerAPI.NumberOfLayer;
        }

        private void timeTrackBarScroll(object sender, EventArgs e)
        {
            if (timeTrackBar.Maximum != _numberOfLines)
            {
                timeTrackBar.Maximum = _numberOfLines;
            }
            _viewerAPI.SetNumberOfLines(timeTrackBar.Value);
            _viewerAPI.Draw();
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
                _motionTracker.Move(position, _viewerAPI.Move);
            }
            else if (e.Button == MouseButtons.Left)
            {
                _motionTracker.Move(position, _viewerAPI.Rotate);
            }
        }
        private void layerTrackBarMouseUp(object sender, MouseEventArgs e)
        {
            SetTimeTrackBar(_numberOfLines);
        }

        private void canvasMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _viewerAPI.CenterView();
            }
        }

        private void threeDCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            _viewerAPI.DrawThreeD = threeDCheckbox.Checked;
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
            _viewerAPI.SetHighlightColors(checkHighlightIndex);
            DrawWorkplane();
        }
        private void gridCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            _viewerAPI.ShowGrid = gridCheckbox.Checked;
            _viewerAPI.Draw();
        }

        private void moveButton_Click(object sender, EventArgs e)
        {
            if (Double.TryParse(xTextBox.Text, out double valX) && Double.TryParse(yTextBox.Text, out double valY))
            {
                _viewerAPI.MoveToPosition2D(new Vector2((float)valX, (float)valY));
                _viewerAPI.Draw();
            }
        }
        private int oldFloatIndex = 0;
        private string oldFloatText = String.Empty;

        private void numBoxKeyDown(object sender, KeyEventArgs e)
        {
            oldFloatIndex = ((TextBox)sender).SelectionStart;
            oldFloatText = ((TextBox)sender).Text;
        }

        private void numBoxTextChanged(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            double val;
            if (!Double.TryParse(tb.Text, out val))
            {
                tb.TextChanged -= numBoxTextChanged;
                tb.Text = oldFloatText;
                tb.SelectionStart = oldFloatIndex;
                tb.TextChanged += numBoxTextChanged;
                tb.BackColor = Color.Red;
            }
            else
            {
                tb.BackColor = Color.White;
            }
        }
    }

}
