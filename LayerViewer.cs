using Google.Protobuf.Collections;
using modularEmulator.FileReader.SLMFileReaderAdapter;
using ModuleFramework.Classes;
using ModuleFramework.Classes.DynamicAllocation;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenVectorFormat;
using ProceduralControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayerViewer
{
    public partial class LayerViewer : Form
    {
        Painter _painter;
        Mover _mover;
        VectorblockToLineMapper _mapper;
        public LayerViewer()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.glCanvas.MouseWheel += new MouseEventHandler(this.MouseWheelZoom);

            _painter = new Painter(glCanvas, this);
            _mover = new Mover(_painter);
            _mapper = new VectorblockToLineMapper();
        }
                
        public float ZoomFaktor { get; set; } = 1.25f;
        private void MouseWheelZoom(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                ZoomFaktor -= 0.5f;
                if (ZoomFaktor <= 0.5f)
                {
                    ZoomFaktor = 0.5f;
                }
            }
            else
            {
                ZoomFaktor += 0.5f;
            }

            _painter.Scale(ZoomFaktor);
        }

        int _numberOfLines = 3;
        private void DrawWorkplane(bool resetNumDrawBlocks = false)
        {
            int layernumber = layerTrackBar.Value;
            _mapper = new VectorblockToLineMapper();

            if (CurrentFile != null && CurrentFile.FileLoadingFinished)
            {
                var workplane = CurrentFile.GetWorkPlane(layernumber);
                var blocks = workplane.VectorBlocks;
                var numBlocks = blocks.Count();

                for (int i = 0; i < numBlocks; i++)
                {
                    _mapper.CalculateVectorBlock(blocks[i], workplane.ZPosInMm);
                }

                var vertices = _mapper.GetVertices();
                _numberOfLines = vertices.Count() / 2;
                _painter.SetLinesToDraw(vertices.ToArray(), _numberOfLines);
            }
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
            _mover.StartMovement(new System.Numerics.Vector2(MousePosition.X, MousePosition.Y));
        }
        private void CameraMoveMouseUp(object sender, MouseEventArgs e)
        {
            _mover.StopMovement(new System.Numerics.Vector2(MousePosition.X, MousePosition.Y));
        }
        private void CameraMoveMouseMove(object sender, MouseEventArgs e)
        {
            _mover.Move(new System.Numerics.Vector2(MousePosition.X, MousePosition.Y));
        }
        private void layerTrackBarMouseUp(object sender, MouseEventArgs e)
        {
            SetTimeTrackBar(_numberOfLines);
        }
    }
}
