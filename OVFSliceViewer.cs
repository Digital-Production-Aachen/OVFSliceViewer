using OVFSliceViewerBusinessLayer.Classes;
using OpenTK;
using OpenVectorFormat.AbstractReaderWriter;
using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OVFSliceViewerBusinessLayer.Model;

namespace OVFSliceViewer
{
    public partial class OVFSliceViewer : Form
    {
        SceneController SceneController;
        MotionTracker _motionTracker;
        private int checkHighlightIndex = 0;
        FileReader _currentFile { get; set; }

        CanvasWrapper _canvasWrapper;
        public OVFSliceViewer()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.glCanvas.MouseWheel += new MouseEventHandler(this.MouseWheelZoom);
            _motionTracker = new MotionTracker();

            _canvasWrapper = new CanvasWrapper(glCanvas);

            SceneController = new SceneController(_canvasWrapper);
        }
        public OVFSliceViewer(string filename)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.glCanvas.MouseWheel += new MouseEventHandler(this.MouseWheelZoom);
            _motionTracker = new MotionTracker();

            _canvasWrapper = new CanvasWrapper(glCanvas);

            SceneController = new SceneController(_canvasWrapper);

            _firstLoadFileName = filename;

            this.Shown += OVFSliceViewer_Shown;

            //LoadJob(filename);
        }

        string _firstLoadFileName = "";
        private void OVFSliceViewer_Shown(object sender, EventArgs e)
        {
            if (_firstLoadFileName != "")
            {
                LoadJob(_firstLoadFileName);
            }
        }

        public OVFSliceViewer(bool showLoadButton) : this()
        {
            this.loadFileButton.Visible = showLoadButton;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            SceneController.Dispose();
            base.OnClosing(e);
        }

        private void MouseWheelZoom(object sender, MouseEventArgs e)
        {
            var fastZoom = System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl) || System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.RightCtrl);
            SceneController.Camera.Zoom(e.Delta > 0, fastZoom);
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SceneController.Render();
        }

        private void DrawWorkplane()
        {
            _canvasWrapper.Init();
            SceneController.Scene.LoadWorkplaneToBuffer(layerTrackBar.Value);
            SceneController.Render();
            SetTimeTrackBar(SceneController.Scene.GetNumberOfLinesInWorkplane());

            label2.Text = SceneController.Scene.lastPosition.ToString();
        }

        private void SetTimeTrackBar(int numberOfLines)
        {
            timeTrackBar.Maximum = numberOfLines;
            timeTrackBar.Value = numberOfLines;
        }

        private async void LoadJobButtonClick(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "ovf and ilt files (*.ovf; *.ilt; *.gcode)|*.ovf;*.ilt;*.gcode|All files (*.*)|*.*";
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                var filename = openFileDialog1.FileNames[0];
                LoadJob(filename);
            }
        }

        public async void LoadJob(string filename)
        {
            await SceneController.LoadFile(filename);

            layerTrackBar.Maximum = Math.Max(SceneController.Scene.OVFFileInfo.NumberOfWorkplanes-1, 0);
            layerTrackBar.Value = 0;
            SetTrackBarText();

            var parts = SceneController.GetParts();

            ((ListBox)this.partsCheckedListBox).DataSource = parts;
            ((ListBox)this.partsCheckedListBox).DisplayMember = "Name";
            ((ListBox)this.partsCheckedListBox).ValueMember = "IsActive";

            for (int i = 0; i < partsCheckedListBox.Items.Count; i++)
            {
                partsCheckedListBox.SetItemChecked(i, true);
            }
        }        
        private void LoadPartNames()
        {
            var names = SceneController.GetPartNames();
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
            layerNumberLabel.Text = $"Layer: {SceneController.Scene.SceneSettings.CurrentWorkplane + 1} von {SceneController.Scene.OVFFileInfo.NumberOfWorkplanes}";
        }

        private void timeTrackBarScroll(object sender, EventArgs e)
        {
            SceneController.Scene.ChangeNumberOfLinesToDraw(timeTrackBar.Value);
            SceneController.Render();
            label2.Text = SceneController.Scene.lastPosition.ToString();
        }

        private void canvasMouseDown(object sender, MouseEventArgs e)
        {
            var position = new Vector2(MousePosition.X, MousePosition.Y);

            if (e.Button == MouseButtons.Middle || e.Button == MouseButtons.Left)
            {
                _motionTracker.Start(position);
            }

            _canvasWrapper.Init();          
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
                _motionTracker.Move(position, SceneController.Camera.Move);
            }
            else if (e.Button == MouseButtons.Left)
            {
                _motionTracker.Move(position, SceneController.Camera.Rotate);
            }
        }
        private void layerTrackBarMouseUp(object sender, MouseEventArgs e)
        {
            SetTimeTrackBar(SceneController.Scene.GetNumberOfLinesInWorkplane());
        }

        private void canvasMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                SceneController.CenterView();
            }
        }

        private void highlightCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            SceneController.Scene.SceneSettings.UseColorIndex = !SceneController.Scene.SceneSettings.UseColorIndex;

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
            DrawWorkplane();
        }
        private void gridCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            //_viewerAPI.ShowGrid = gridCheckbox.Checked;
            //_viewerAPI.Draw();
        }

        private void moveButton_Click(object sender, EventArgs e)
        {
            if (Double.TryParse(xTextBox.Text, out double valX) && Double.TryParse(yTextBox.Text, out double valY))
            {
                // ToDo: needs to be adapted to new structure
                //_viewerAPI.MoveToPosition2D(new Vector2((float)valX, (float)valY));
                //_viewerAPI.Draw();
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

        private void glCanvas_Resize(object sender, EventArgs e)
        {
            this.glCanvas.BeginInvoke((MethodInvoker)delegate
            {
                _canvasWrapper.Resize(this.glCanvas.ClientSize.Width, this.glCanvas.ClientSize.Height);
                var canvasArea = _canvasWrapper.GetCanvasArea();
                SceneController.Camera.Resize(canvasArea.Item1, canvasArea.Item2);
                SceneController.Render();
            });
        }

        private void partsCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var index = e.Index;
            var newState = e.NewValue == CheckState.Checked ? true : false;

            ((AbstrPart)partsCheckedListBox.Items[index]).IsActive = newState;

            SceneController.Render();
        }

        private void CbShow3dModel_CheckedChanged(object sender, EventArgs e)
        {
            SceneController.Scene.SceneSettings.ShowAs3dObject = CbShow3dModel.Checked;

            if (SceneController.Scene.SceneSettings.ShowAs3dObject)
            {
                layerTrackBar.Enabled = false;
                layerTrackBar.Value = layerTrackBar.Maximum;
                timeTrackBar.Enabled = false;
                SceneController.Scene.SceneSettings.UseColorIndex = false;
            }
            else
            {
                timeTrackBar.Enabled = true;
                layerTrackBar.Enabled = true;
            }

            DrawWorkplane();
        }

        private void cBLaserIndexColor_CheckedChanged(object sender, EventArgs e)
        {
            SceneController.Scene.SceneSettings.UseColorIndex = cBLaserIndexColor.Checked;
            DrawWorkplane();
        }

        private void btnCloseFile_Click(object sender, EventArgs e)
        {
            SceneController.CloseFile();

            SetTrackBarText();
            SetTimeTrackBar(0);

            layerTrackBar.Maximum = 0;
            layerTrackBar.Value = 0;
        }
    }

}
