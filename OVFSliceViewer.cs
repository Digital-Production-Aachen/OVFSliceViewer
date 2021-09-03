using OVFSliceViewer.Classes;
using OpenTK;
using OpenVectorFormat.AbstractReaderWriter;
using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace OVFSliceViewer
{
    public partial class OVFSliceViewer : Form
    {
        LayerViewer.Model.SceneController SceneController;
        MotionTracker _motionTracker;
        int _numberOfLines = 3;
        private int checkHighlightIndex = 0;
        FileReader _currentFile { get; set; }

        CanvasWrapper _canvasWrapper;
        public OVFSliceViewer()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            //highlightCheckedListBox.SetItemChecked(0, true);//always start with highlighted  contours
            this.glCanvas.MouseWheel += new MouseEventHandler(this.MouseWheelZoom);
            _motionTracker = new MotionTracker();

            _canvasWrapper = new CanvasWrapper(glCanvas);

            SceneController = new LayerViewer.Model.SceneController(_canvasWrapper);
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
            SceneController.Camera.Zoom(e.Delta > 0);
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SceneController.Render();
        }

        private async void DrawWorkplane()
        {
            //await _viewerAPI.DrawWorkplane(layerTrackBar.Value);
            _canvasWrapper.Init();
            SceneController.Scene.LoadWorkplaneToBuffer(layerTrackBar.Value);
            SceneController.Render();
            _numberOfLines = SceneController.Scene.NumberOfLinesInWorkplane;
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

        public void LoadJob(string filename)
        {
            SceneController.LoadFile(filename);

            layerTrackBar.Maximum = Math.Max(SceneController.Scene.NumberOfWorkplanes-1, 0);
            layerTrackBar.Value = 0;
            SetTrackBarText();
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
            layerNumberLabel.Text = $"Layer: {SceneController.Scene.CurrentWorkplane+1} von {SceneController.Scene.NumberOfWorkplanes}";
        }

        private void timeTrackBarScroll(object sender, EventArgs e)
        {
            if (timeTrackBar.Maximum != _numberOfLines)
            {
                timeTrackBar.Maximum = _numberOfLines;
            }
            //_viewerAPI.SetNumberOfLines(timeTrackBar.Value);
            //_viewerAPI.Draw();
            //Scene.RenderAllParts();
        }

        private void canvasMouseDown(object sender, MouseEventArgs e)
        {
            var position = new Vector2(MousePosition.X, MousePosition.Y);

            if (e.Button == MouseButtons.Middle || e.Button == MouseButtons.Left)
            {
                _motionTracker.Start(position);
            }

            _canvasWrapper.Init();

            //_test = new LayerViewer.Model.RenderObject(Scene.Camera);

            Vertex[] vertices =
            {
                new Vertex(new Vector3(-1.0f, 1.0f, 0.0f), 0),
                new Vertex(new Vector3(-1.0f, -1.0f, 0.0f), 0),
                new Vertex(new Vector3(1.0f, 1.0f, 0.0f), 0),
                new Vertex(new Vector3(-1.0f, -1.0f, 0.0f), 0),
                new Vertex(new Vector3(1.0f, 1.0f, 0.0f), 0),
                new Vertex(new Vector3(1.0f, -1.0f, 0.0f), 0)
            };

            //_test.AddVertices(vertices);
            //_test.PrimitiveType = OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles;
            
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
            SetTimeTrackBar(_numberOfLines);
        }

        private void canvasMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                SceneController.CenterView();
            }
        }

        private void threeDCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            //_viewerAPI.DrawThreeD = threeDCheckbox.Checked;
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
            //_viewerAPI.SetHighlightColors(checkHighlightIndex);
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
                _canvasWrapper.Resize(this.glCanvas.ClientSize);
                SceneController.Camera.Resize(_canvasWrapper.GetCanvasArea());
                SceneController.Render();
            });
        }
    }

}
