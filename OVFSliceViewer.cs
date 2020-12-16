using LayerViewer.Classes;
using ModularEmulator.ProductView;
using OpenTK;
using OpenVectorFormat;
using ProceduralControl;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OVFSliceViewer
{
    public partial class OVFSliceViewer : Form
    {
        Painter _painter;
        //Mover _mover;
        //Rotater _rotater;
        MotionTracker _motionTracker;
        VectorblockToLineMapper _mapper;
        public OVFSliceViewer()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.glCanvas.MouseWheel += new MouseEventHandler(this.MouseWheelZoom);

            _painter = new Painter(glCanvas, this);
            _motionTracker = new MotionTracker();

            var size = new Vector2(glCanvas.Width, glCanvas.Height);
            
            _mapper = new VectorblockToLineMapper();
        }
                
        public float ZoomFaktor { get; set; } = 1.25f;
        private void MouseWheelZoom(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                //ZoomFaktor -= 0.5f;
                //if (ZoomFaktor <= 0.5f)
                //{
                //    ZoomFaktor = 0.5f;
                //}
                _painter.Scale(true);
            }
            else
            {
                _painter.Scale(false);
                ZoomFaktor += 0.5f;
            }

            //_painter.Scale(ZoomFaktor);
        }

        int _numberOfLines = 3;
        private void DrawWorkplane(bool resetNumDrawBlocks = false)
        {
            int layernumber = layerTrackBar.Value;
            _mapper = new VectorblockToLineMapper();
            var height = 0f;

            for (int j = layernumber; j < layernumber+1; j++)
            {
                if (CurrentFile != null && CurrentFile.FileLoadingFinished)
                {
                    var workplane = CurrentFile.GetWorkPlane(j);
                    var blocks = workplane.VectorBlocks;
                    var numBlocks = blocks.Count();
                    height = workplane.ZPosInMm;

                    for (int i = 0; i < numBlocks; i++)
                    {
                        if (blocks[i].LPbfMetadata.PartArea == VectorBlock.Types.PartArea.Contour || j == layernumber)
                        {
                            _mapper.CalculateVectorBlock(blocks[i], workplane.ZPosInMm);
                        }
                    }
                }
            }

            var vertices = _mapper.GetVertices();
            _numberOfLines = vertices.Count() / 2;
            _painter.SetLinesToDraw(vertices, _numberOfLines+1);
            _painter.Camera.ChangeHeight(height);

            _mapper.Dispose();
            _mapper = null;
        }
        private void SetTimeTrackBar(int numberOfLines)
        {
            timeTrackBar.Maximum = numberOfLines;
            timeTrackBar.Value = numberOfLines;
        }

        FileReader CurrentFile { get; set; }
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
            CurrentFile = FileReaderFactory.CreateNewReader(Path.GetExtension(filename));

            var command = new JobExecutionCommand
            {
                FileReader = CurrentFile,
                FilePath = openFileDialog1.FileNames[0],
            };

            await CurrentFile.OpenJobAsync(filename, command);

            layerTrackBar.Maximum = CurrentFile.Job.NumWorkPlanes - 1;
            layerTrackBar.Value = 0;
        }
        public void LoadJob(FileReader fileReader)
        {
            CurrentFile = fileReader;
        }

        private void layerTrackBarScroll(object sender, EventArgs e)
        {
            DrawWorkplane(true);

            label2.Text = "Layer: " + layerTrackBar.Value + " von " + layerTrackBar.Maximum;
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

            if (e.Button == MouseButtons.Middle/* || e.Button == MouseButtons.Left*/)
            {
                _motionTracker.Start(position);
            }
        }
        private void CameraMoveMouseUp(object sender, MouseEventArgs e)
        {
            var position = new Vector2(MousePosition.X, MousePosition.Y);
            if (e.Button == MouseButtons.Middle /*|| e.Button == MouseButtons.Left*/)
            {
                _motionTracker.Stop();
            }
        }
        private void CameraMoveMouseMove(object sender, MouseEventArgs e)
        {
            var position = new Vector2(MousePosition.X, MousePosition.Y);

            if (e.Button == MouseButtons.Middle)
            {
                _motionTracker.Move(position, _painter.Camera.Move);
            }
            //else if (e.Button == MouseButtons.Left)
            //{
            //    _motionTracker.Move(position, _painter.Camera.Rotate);
            //}
        }
        private void layerTrackBarMouseUp(object sender, MouseEventArgs e)
        {
            SetTimeTrackBar(_numberOfLines);
        }
    }
}
