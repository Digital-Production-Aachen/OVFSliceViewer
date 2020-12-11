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
        public LayerViewer()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.glControl1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.PictureBox1OnMouseWheel);
            Canvas = new DrawingBuffer();
            _canvas = new OpenGLCanvas(glControl1, this);
        }
        OpenGLCanvas _canvas;
        DrawingBuffer Canvas;

        List<TimedHatch> TestPattern(float length = 15, float hatchDistance = 0.05f)
        {
            float vm = 1000;
            var hatches = new List<TimedHatch>();
            var hatch = new TimedHatch(new System.Numerics.Vector2(0, 0), new System.Numerics.Vector2(0, length), 0, length / vm);

            hatches.Add(hatch);

            for (int i = 1; i < 4; i += 2)
            {
                hatch = new TimedHatch(new System.Numerics.Vector2(hatchDistance * i, length), new System.Numerics.Vector2(hatchDistance * i, 0), hatches[i - 1].EndTime, hatches[i - 1].EndTime + length / vm);
                hatches.Add(hatch);

                //mark = new Mark(new Vector2(hatchDistance * i + hatchDistance, 0), new Vector2(0, length), markierungen, parameter);
                hatch = new TimedHatch(new System.Numerics.Vector2(hatchDistance * (i + 1), 0), new System.Numerics.Vector2(hatchDistance * (i + 1), length), hatches[i].EndTime, hatches[i].EndTime + length / vm);
                hatches.Add(hatch);
            }

            return hatches;
        }

        public float ZoomFaktor { get; set; } = 1.25f;
        private void PictureBox1OnMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
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

            _canvas.Scale(ZoomFaktor);
        }

        private List<VmLine> VectorBlockToViewModel(OpenVectorFormat.VectorBlock vectorBlock, float height)
        {
            var list = new List<VmLine>();
            var points = new RepeatedField<float>();

            //vectorBlock.LPbfMetadata.PartArea = VectorBlock.Types.PartArea.Contour
            //vectorBlock.LPbfMetadata.SkinType = VectorBlock.Types.LPBFMetadata.Types.SkinType.DownSkin
            //vectorBlock.LPbfMetadata.StructureType = VectorBlock.Types.StructureType.Part

            if (vectorBlock.VectorDataCase == OpenVectorFormat.VectorBlock.VectorDataOneofCase.LineSequence)
            {
                points = vectorBlock.LineSequence.Points;
                for (int i = 3; i < points.Count(); i += 2)
                {
                    var vmVectorStart = new VmVectorWithPower(points[i - 3], points[i - 2], 350);
                    var vmVectorEnde = new VmVectorWithPower(points[i - 1], points[i], 350);
                    var vmVector = new VmLine() { Start = vmVectorStart, Ende = vmVectorEnde, Height = height };
                    list.Add(vmVector);
                }
            }
            else if (vectorBlock.VectorDataCase == OpenVectorFormat.VectorBlock.VectorDataOneofCase.Hatches)
            {
                points = vectorBlock.Hatches.Points;
                var listLine = new List<VmLine>();
                for (int i = 3; i < points.Count; i += 4)
                {
                    var vmVectorEnde = new VmVectorWithPower(points[i - 1], points[i - 0], 350);
                    var vmVectorStart = new VmVectorWithPower(points[i - 3], points[i - 2], 350);

                    var vmLine = new VmLine() { Start = vmVectorStart, Ende = vmVectorEnde, Height = height };

                    list.Add(vmLine);
                }
                return list;
            }
            else if (vectorBlock.VectorDataCase == OpenVectorFormat.VectorBlock.VectorDataOneofCase.LineSequenceParaAdapt)
            {
                points = vectorBlock.LineSequenceParaAdapt.PointsWithParas;

                for (int i = 5; i < points.Count(); i += 3)
                {
                    var vmVectorStart = new VmVectorWithPower(points[i - 5], points[i - 4], points[i - 3]);
                    var vmVectorEnde = new VmVectorWithPower(points[i - 2], points[i - 1], points[i]);
                    var vmVector = new VmLine() { Start = vmVectorStart, Ende = vmVectorEnde, Height = height };
                    list.Add(vmVector);
                }
                return list;
            }
            return list;
        }

        int _numberOfLines = 6;
        private async void DrawWorkplane(bool resetNumDrawBlocks = false)
        {
            int layernumber = trackBar1.Value;

            if (CurrentFile != null && CurrentFile.FileLoadingFinished)
            {
                var workplane = CurrentFile.GetWorkPlane(layernumber);
                var blocks = workplane.VectorBlocks;
                var numBlocks = blocks.Count();

                Canvas.Clear();
                for (int i = 0; i < numBlocks; i++)
                {
                    var vm = VectorBlockToViewModel(blocks[i], workplane.ZPosInMm);
                    Canvas.UpdateLines(vm);
                }

                var temp = Canvas.GetVertices();
                _numberOfLines = temp.Count() / 2;
                _canvas.SetLinesToDraw(temp.ToArray(), _numberOfLines);
            }
        }
        private async void SetTrackBar2(int numberOfLines)
        {
            trackBar2.Maximum = numberOfLines;
            trackBar2.Value = numberOfLines;
        }

        Graphics CanvasMaszstab { get; set; }

        //Job Job { get; set; }
        FileReader CurrentFile { get; set; }

        private void button1_Click(object sender, EventArgs e)
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
        private async void LoadJob(string filename)
        {
            CurrentFile = CreateNewInstance(Path.GetExtension(filename));

            var command = new JobExecutionCommand
            {
                FileReader = CurrentFile,
                FilePath = openFileDialog1.FileNames[0],
            };

            await CurrentFile.OpenJobAsync(filename, command);

            //wait for loading of the file Reader
            //while (!CurrentFile.FileLoadingFinished)
            //{
            //    System.Threading.Thread.Sleep(100);
            //}

            trackBar1.Maximum = CurrentFile.Job.NumWorkPlanes - 1;
            trackBar1.Value = 0;
        }

        public static FileReader CreateNewInstance(string extension)
        {
            FileReader newFileReader;
            if (SLMFileReaderAdapter.SupportedFileFormats.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                newFileReader = new SLMFileReaderAdapter();
            }
            else if (OpenVectorFormat.FileReaderFactory.SupportedFileFormats.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                newFileReader = OpenVectorFormat.FileReaderFactory.CreateNewReader(extension);
            }
            else
            {
                throw new ArgumentException("format " + extension + " is not supported");
            }
            return newFileReader;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            DrawWorkplane(true);

            label2.Text = "Layer: " + trackBar1.Value + " von " + trackBar1.Maximum;
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (trackBar2.Maximum != _numberOfLines)
            {
                trackBar2.Maximum = _numberOfLines;
            }
            _canvas.SetNumberOfLinesToDraw(trackBar2.Value);
        }

        private int XMouseDown { get; set; }
        private int YMouseDown { get; set; }

        private int XMouseUp { get; set; }
        private int YMouseUp { get; set; }

        private float DeltaX { get; set; }
        private float DeltaY { get; set; }

        private void MoveCanvas()
        {
            float zoom = 1;
            if (ZoomFaktor > 1)
            {
                zoom = (float)ZoomFaktor;
            }
            DeltaX = ((float)(XMouseUp - XMouseDown));
            DeltaY = ((float)(-YMouseUp + YMouseDown));

            _canvas.Move(DeltaX, DeltaY);
        }

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            XMouseDown = MousePosition.X;
            YMouseDown = MousePosition.Y;
            _mouseIsDown = true;
            _canvas.StartMove();
        }
        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseIsDown = false;

            MoveCanvas();
            _canvas.StopMove();
        }

        bool _mouseIsDown = false;
        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseIsDown)
            {
                XMouseUp = MousePosition.X;
                YMouseUp = MousePosition.Y;
                MoveCanvas();
            }
        }


        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            SetTrackBar2(_numberOfLines);
        }
    }
}
