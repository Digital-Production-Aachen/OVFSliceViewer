using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OVFSliceViewer.Classes.ShaderNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OVFSliceViewer.Classes
{

    // https://gist.github.com/GeirGrusom/347f30e981f33972c934 <- code source
    public class Painter
    {
        protected GLControl _gl;
        protected Form _parent;
        protected bool _isDrawing = true;
        protected bool _eventRemoved = false;
        protected Matrix4 _view;
        protected int _numberOfLinesToDraw = 6;
        private bool _showGrid = true;
        protected float _zHeight;
        protected int _vertexBuffer;
        protected int _vertexArray;
        protected Shader _shader;
        protected Grid _grid = new Grid();
        //protected DrawableObject _drawableGrid;
        private int[] _buffers;
        public Dictionary<int, DrawablePart> DrawableParts { get; set; }

        Vertex[] _vertices = new Vertex[6]{
                new Vertex { Color = new Vector4(1,0,0,0), Position = new Vector3(0, 0, 0)},
                new Vertex { Color = new Vector4(1,0,0,0), Position = new Vector3(20f, 0, 0)},
                new Vertex { Color = new Vector4(0,1,0,0), Position = new Vector3(0, 0, 0)},
                new Vertex { Color = new Vector4(0,1,0,0), Position = new Vector3(0, 20f, 0)},
                new Vertex { Color = new Vector4(0,0,1,0), Position = new Vector3(0, 0, 0)},
                new Vertex { Color = new Vector4(0,0,1,0), Position = new Vector3(0, 0, 20f)}
        };

        public Camera Camera { get; set; }
        public IRotateable Rotation => Camera;

        public bool ShowGrid { get => _showGrid; set => _showGrid = value; }

        int _mvp;
        PaintEventHandler _test;
        public Painter(GLControl gl, Form parent)
        {
            _gl = gl;
            _parent = parent;
            Init();
        }

        public int[] GetBufferPointer(int numberOfBuffers)
        {
            if (_buffers != null)
            {
                GL.DeleteBuffers(_buffers.Length, _buffers);
            }
            
            _buffers = new int[numberOfBuffers+1];
            
            GL.GenBuffers(numberOfBuffers+1, _buffers);

            return _buffers;
        }
        public Shader GetShader()
        {
            return _shader;
        }

        protected void Init()
        {
            _parent.FormClosing += (sender2, e2) => _isDrawing = false;
            _test = new PaintEventHandler(GLControlPaint);
            _gl.Paint += _test;
            _gl.Resize += CanvasResizeEvent;
            ResetCamera(_gl);
            SetLines(new Vertex[0], _grid.Length);

            //_drawableGrid = new DrawableObject(_shader, _buffers[1]);
        }
        void CanvasResizeEvent(object sender, EventArgs e)
        {
            GLControl c = sender as GLControl;

            if (c.ClientSize.Height == 0)
                c.ClientSize = new System.Drawing.Size(c.ClientSize.Width, 1);

            GL.Viewport(c.ClientRectangle);
            ResetCamera(c);
        }

        private void ResetCamera(GLControl gl)
        {
            Camera = new Camera(gl.Width, gl.Height);
        }

        public void TargetCenter()
        {
            if (_vertices.Length > _grid.Length)
            {
                float minX = _vertices[_grid.Length].Position.X;
                float maxX = _vertices[_grid.Length].Position.X;
                float minY = _vertices[_grid.Length].Position.Y;
                float maxY = _vertices[_grid.Length].Position.Y;

                for (int i = _grid.Length + 1; i < _vertices.Length; i++)
                {
                    var x = _vertices[i].Position.X;
                    var y = _vertices[i].Position.Y;
                    if (x < minX)
                    {
                        minX = x;
                    }
                    else if (x > maxX)
                    {
                        maxX = x;
                    }

                    if (y < minY)
                    {
                        minY = y;
                    }
                    else if (y > maxY)
                    {
                        maxY = y;
                    }

                }
                var center = new Vector2(minX + (maxX - minX) / 2, minY + (maxY - minY) / 2);
                Camera.MoveToPosition2D(center);
            }
            else
            {
                Camera.MoveToPosition2D(new Vector2(0, 0));
            }
            //Draw();
        }

        public void SetLinesAndDraw(Vertex[] vertices, int numberOfLinesToDraw = 0)
        {
            //SetLines(vertices, numberOfLinesToDraw);
            //RebindBufferObject();
            Draw();
        }
        public void SetLines(Vertex[] vertices, int numberOfLinesToDraw = 0)
        {
            _vertices = null;
            _vertices = new Vertex[_grid.Length + vertices.Length];
            _grid.GetGrid().CopyTo(_vertices, 0);
            vertices.CopyTo(_vertices, _grid.Length);

            _numberOfLinesToDraw = numberOfLinesToDraw;

            MoveCameraWithLayer();
        }
        private void MoveCameraWithLayer()
        {
            if (_vertices.Length > _grid.Length)
            {
                var zHeight = _vertices[_vertices.Length-1].Position.Z;
                if (zHeight != _zHeight)
                {
                    Camera.ChangeHeight(zHeight - _zHeight);
                    _zHeight = zHeight;
                }
            }
        }
        private void RebindBufferObject()
        {
            //GCHandle()
            var handle = GCHandle.Alloc(_vertices, GCHandleType.Pinned);

            try
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
                GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(28 * _vertices.Length), handle.AddrOfPinnedObject(), BufferUsageHint.StaticDraw);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            }
            finally
            {
                handle.Free();
            }

        }
        public void SetNumberOfLinesToDraw(int numberOfLinesToDraw)
        {
            if (numberOfLinesToDraw == 0)
            {
                _numberOfLinesToDraw = _vertices.Count();
            }
            else _numberOfLinesToDraw = numberOfLinesToDraw;
            Draw();
        }

        private void GLControlPaint(object sender, PaintEventArgs e)
        {
            if (!_eventRemoved)
            {
                _gl.Paint -= _test;
            }
            PrepareDrawing();
            Draw();
        }
        private void PrepareDrawing()
        {
            var display = _gl;
            display.Show();

            _gl.MakeCurrent();
            _shader = new Shader();

            GL.ClearColor(Color4.LightGray);
            CreateVertexBuffer();
            //_drawableGrid = new DrawableObject(_shader, _buffers[1]);
            //_drawableGrid.ChangePicture(_grid.GetGrid());
            CreateVertexArray(_vertexBuffer);

            _shader.Use();

            GL.ShadeModel(ShadingModel.Flat);
            _mvp = _shader.GetUniformLocation();

            //float angle = 0;

            //var lastTimestamp = Stopwatch.GetTimestamp();
            //var freq = Stopwatch.Frequency;
        }
        public void Draw()
        {
            // Clear color and depth buffers
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 modelViewProjection = Camera.TranslationMatrix * Camera.LookAtTransformationMatrix * Camera.ProjectionMatrix;

            GL.UniformMatrix4(_mvp, false, ref modelViewProjection);

            GL.BindVertexArray(_vertexArray);
            GL.LineWidth(5f);
            if (ShowGrid)
            {
                //GL.DrawArrays(PrimitiveType.Lines, 0, _numberOfLinesToDraw*2/* + _grid.Length*/);
                //if (_drawableGrid != null)
                //{
                //    _drawableGrid.Draw(modelViewProjection);
                //}
                if (DrawableParts != null)
                {
                    //for (int i = 5; i < 6; i++)
                    //{
                    //    DrawableParts[i].DrawAll(modelViewProjection);
                    //}
                    foreach (var part in DrawableParts.Values)
                    {
                        part.DrawAll(modelViewProjection);
                    }
                }
            }
            else
            {
                //GL.DrawArrays(PrimitiveType.Lines, _grid.Length, _numberOfLinesToDraw * 2);
            }

            _gl.SwapBuffers();
            Application.DoEvents();
        }

        private void CreateVertexBuffer()
        {
            //_vertexBuffer = GL.GenBuffer();
            //_buffers = new int[2];
            //GL.GenBuffers(2, _buffers);
            //GL.DeleteBuffers()
            //_vertexBuffer = _buffers[0];

            //GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);

            //var handle = GCHandle.Alloc(_vertices, GCHandleType.Pinned);
            //try
            //{
            //    GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(28 * _vertices.Length), handle.AddrOfPinnedObject(),
            //        BufferUsageHint.StaticDraw);
            //}
            //finally
            //{
            //    handle.Free();
            //}
            //GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        }

        private void CreateVertexArray(int vertexBuffer)
        {
            _vertexArray = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 28, IntPtr.Zero);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 28, new IntPtr(12));
            GL.EnableVertexAttribArray(1);
            GL.BindVertexArray(_vertexArray);
        }

        public void DisposeShader()
        {
            _shader.Dispose();
        }
    }
}
