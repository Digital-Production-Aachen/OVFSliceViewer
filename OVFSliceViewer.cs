using OVFSliceViewerBusinessLayer.Classes;
using OpenTK;
using OpenVectorFormat.AbstractReaderWriter;
using System;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;
using OVFSliceViewerBusinessLayer.Model;
using System.Linq;
using SliceViewerBusinessLayer.Classes;
using System.Collections.Generic;
using System.Drawing.Text;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using OpenTK.WinForms;

namespace OVFSliceViewer
{
    public partial class OVFSliceViewer : Form
    {
        SceneController SceneController;
        MotionTracker _motionTracker;
        private int checkHighlightIndex = 0;

        // STL part specifics
        private int paintingRadius = 0;
        FileReader _currentFile { get; set; }
        private PaintFunction _currentPaintFunction = PaintFunction.None;

        CanvasWrapper _canvasWrapper;
        enum PaintFunction
        {
            None,
            Erase,
            NoSupport,
            Accessibility,
            FunctionalArea
        }
        private Dictionary<PaintFunction, float> _functionColorDictionary = new Dictionary<PaintFunction, float>()
        {

            {PaintFunction.Erase, 0f},
            {PaintFunction.NoSupport, 0.9f},
            {PaintFunction.Accessibility, 0.5f},
            {PaintFunction.FunctionalArea, 0.64f}
        };
        public OVFSliceViewer()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.glCanvas.MouseWheel += new MouseEventHandler(this.MouseWheelZoom);
            _motionTracker = new MotionTracker();

            _canvasWrapper = new CanvasWrapper(glCanvas);

            SceneController = new SceneController(_canvasWrapper);

            

            this.KeyDown += _canvasWrapper.KeyDown;
            this.KeyUp += _canvasWrapper.KeyUp;
            this.KeyPreview = true;

            //_input = _canvasWrapper.Canvas.EnableNativeInput();
            _canvasWrapper.Canvas.KeyDown += _canvasWrapper.KeyDown;
            _canvasWrapper.Canvas.KeyUp += _canvasWrapper.KeyUp;
        }

        public OVFSliceViewer(string filename):this()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.glCanvas.MouseWheel += new MouseEventHandler(this.MouseWheelZoom);
            _motionTracker = new MotionTracker();

            _canvasWrapper = new CanvasWrapper(glCanvas);

            SceneController = new SceneController(_canvasWrapper);

            _firstLoadFileName = filename;
            this.Shown += OVFSliceViewer_Shown;

            //_canvasWrapper.Canvas.DisableNativeInput();
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
            //keyboardState.IsKeyDown(OpenTK.Input.Key.ControlLeft)
            var fastZoom = _canvasWrapper.IsKeyPressed(System.Windows.Forms.Keys.ControlKey) || _canvasWrapper.IsKeyPressed(System.Windows.Forms.Keys.RControlKey);
            SceneController.Camera.Zoom(e.Delta > 0, fastZoom);
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SceneController.Render();
        }

        private void DrawWorkplane()
        {
            _canvasWrapper.Init();
            SceneController.Scene.LoadWorkplaneToBuffer(layerTrackBar.Value);
            SetTimeTrackBar(SceneController.Scene.GetNumberOfLinesInWorkplane());
            SceneController.Scene.ChangeNumberOfLinesToDraw(timeTrackBar.Value);
            SceneController.Render();

            label2.Text = SceneController.Scene.LastPosition.ToString();
        }

        private void SetTimeTrackBar(int numberOfLines)
        {
            timeTrackBar.Maximum = numberOfLines;
            timeTrackBar.Value = numberOfLines;
        }

        private async void LoadJobButtonClick(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "ovf, ilt and stl files (*.ovf; *.ilt; *.gcode)|*.ovf;*.ilt;*.stl;*.gcode|All files (*.*)|*.*";
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                var filename = openFileDialog1.FileNames[0];
                LoadJob(filename);
            }
            
        }

        public async void LoadJob(string filename)
        {
            try
            {
                //this.KeyDown += _canvasWrapper.KeyDown;
                //this.KeyUp += _canvasWrapper.KeyUp;
                //this.KeyPreview = true;

                await SceneController.LoadFile(filename);
                layerTrackBar.Maximum = Math.Max(SceneController.Scene.OVFFileInfo.NumberOfWorkplanes - 1, 0);
                layerTrackBar.Value = 0;
                SetTrackBarText();

                var parts = SceneController.GetParts();
                bool isStl = Path.GetExtension(filename) == ".stl";
                exportButton.Enabled = isStl;
                if (!isStl)
                {
                    ((ListBox)this.partsCheckedListBox).DataSource = parts;
                }

                ((ListBox)this.partsCheckedListBox).DisplayMember = "Name";
                ((ListBox)this.partsCheckedListBox).ValueMember = "IsActive";

                for (int i = 0; i < partsCheckedListBox.Items.Count; i++)
                {
                    partsCheckedListBox.SetItemChecked(i, true);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Datei konnte nicht gelesen werden!", "Fehler beim Laden einer Datei", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            label2.Text = SceneController.Scene.LastPosition.ToString();
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
            if ( _currentPaintFunction!= PaintFunction.None && SceneController.Scene is STLScene && e.Button == MouseButtons.Right)
            {
                ColorSTLPart(e);
            }
            else
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
        }
        private void layerTrackBarMouseUp(object sender, MouseEventArgs e)
        {
            if (SceneController.Scene is null)
                return;
            SetTimeTrackBar(SceneController.Scene.GetNumberOfLinesInWorkplane());
        }

        private void canvasMouseClick(object sender, MouseEventArgs e)
        {
            if (_currentPaintFunction != PaintFunction.None && SceneController.Scene is STLScene)
            {
                ColorSTLPart(e);
            }
            else if (e.Button == MouseButtons.Right)
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

        private void ColorSTLPart(MouseEventArgs e)
        {
            if(_currentPaintFunction != PaintFunction.None)
            {
                //var position = new Vector2(Cursor.Position.X, Cursor.Position.Y);
                var position = new Vector2(e.Location.X, e.Location.Y);
                if (e.Button == MouseButtons.Right)
                {
                    (SceneController.Scene as STLScene).ColorNearestHitTriangles(position, _functionColorDictionary[_currentPaintFunction], paintingRadius);
                }
                SceneController.Render();
            }
        }

        private void STLKeyActions(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if(SceneController.Scene is STLScene)
            {
                if (e.KeyChar == 'e')
                {
                    ExportPartsAsObj();
                }
                else if(e.KeyChar == '1')
                {
                    paintingRadius -= 1;
                    paintingRadius = MathHelper.Clamp(paintingRadius, 0, 10);
                }
                else if (e.KeyChar == '2')
                {
                    paintingRadius += 1;
                    paintingRadius = MathHelper.Clamp(paintingRadius, 0, 10);
                }
                Debug.WriteLine("Painting radius: " + paintingRadius);
            }   
        }

        private void ExportPartsAsObj()
        {
            if (!(SceneController.Scene is STLScene))
                return;
            foreach (STLPart part in SceneController.Scene.PartsInScene)
                ExportAsObj(part);
        }
        private void ExportAsObj(STLPart part)
        {
            string path;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "obj files (*.obj)|*.obj|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = saveFileDialog.FileName;
                part.WriteAsObj(path);
            }
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            ExportPartsAsObj();
        }

        private void paintFunctrionCheckedListBox_SelectedValueChanged(object sender, EventArgs e)
        {


        }

        private void paintFunctrionCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            Dictionary<int, PaintFunction> guiToEnumDict = new Dictionary<int, PaintFunction>()
            {
                {0, PaintFunction.NoSupport },
                {1, PaintFunction.Accessibility },
                {2, PaintFunction.FunctionalArea },
                {3, PaintFunction.Erase }
            };
            //paintingMode

            if (e.NewValue == CheckState.Checked && paintFunctrionCheckedListBox.CheckedItems.Count > 0)
            {
                paintFunctrionCheckedListBox.ItemCheck -= paintFunctrionCheckedListBox_ItemCheck;
                paintFunctrionCheckedListBox.SetItemChecked(paintFunctrionCheckedListBox.CheckedIndices[0], false);
                paintFunctrionCheckedListBox.ItemCheck += paintFunctrionCheckedListBox_ItemCheck;
            }
            _currentPaintFunction = guiToEnumDict[e.Index];
            if(e.NewValue == CheckState.Unchecked)
            {
                _currentPaintFunction = PaintFunction.None;
            }
            //MessageBox.Show(paintFunctrionCheckedListBox.Items[e.Index].ToString());
        }

        private void OVFSliceViewer_KeyDown(object sender, KeyEventArgs e)
        {
            _canvasWrapper.KeyDown(sender, e);
        }

        private void OVFSliceViewer_KeyUp(object sender, KeyEventArgs e)
        {
            _canvasWrapper.KeyUp(sender, e);
        }

        private void OVFSliceViewer_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }
    }

}
