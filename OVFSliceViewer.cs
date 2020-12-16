﻿using LayerViewer.Classes;
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
        JobViewer _viewerJob;
        Painter _painter;
        MotionTracker _motionTracker;
        //VectorblockToLineMapper _mapper;
        int _numberOfLines = 3;
        FileReader _currentFile { get; set; }
        public OVFSliceViewer()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.glCanvas.MouseWheel += new MouseEventHandler(this.MouseWheelZoom);

            _painter = new Painter(glCanvas, this);
            _motionTracker = new MotionTracker();
        }

        private void MouseWheelZoom(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                _painter.Scale(true);
            }
            else
            {
                _painter.Scale(false);
            }
            _painter.Draw();
        }

        private void DrawWorkplane()
        {
            int layernumber = layerTrackBar.Value;
            var mapper = new VectorblockToLineMapper();
            var height = 0f;

            for (int j = layernumber; j < layernumber+1; j++)
            {
                if (_currentFile != null && _currentFile.FileLoadingFinished)
                {
                    var workplane = _currentFile.GetWorkPlane(j);
                    var blocks = workplane.VectorBlocks;
                    var numBlocks = blocks.Count();
                    height = workplane.ZPosInMm;
                    //_currentFile.

                    for (int i = 0; i < numBlocks; i++)
                    {
                        if (blocks[i].LPbfMetadata.PartArea == VectorBlock.Types.PartArea.Contour || j == layernumber)
                        {
                            mapper.CalculateVectorBlock(blocks[i], workplane.ZPosInMm);
                        }
                    }
                }
            }

            var vertices = mapper.GetVertices();
            _numberOfLines = vertices.Count() / 2;
            _painter.SetLinesAndDraw(vertices, _numberOfLines+1);

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

            _viewerJob = new JobViewer(_currentFile);

            layerTrackBar.Maximum = _currentFile.Job.NumWorkPlanes - 1;
            layerTrackBar.Value = 0;

            Console.WriteLine(_viewerJob.Center.ToString());
            _painter.Camera.MoveToPosition2D(_viewerJob.Center);
        }
        
        public void LoadJob(FileReader fileReader)
        {
            _currentFile = fileReader;
        }

        private void layerTrackBarScroll(object sender, EventArgs e)
        {
            DrawWorkplane();

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
    }
}
