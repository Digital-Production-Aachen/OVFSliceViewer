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
        protected bool _eventRemoved = false;
        protected int _numberOfLinesToDraw = 0;
       
        public Shader Shader { get; private set; }
        protected Grid _grid = new Grid();
        protected DrawableObject _drawableGrid;
        
        public bool Is3d { get; set; } = false;
        public bool ShowGrid { get; set; } = true;
        public Dictionary<int, DrawablePart> DrawableParts { get; set; }
        public Dictionary<int, PointOrderManagement> LayerPointManager { get; set; } = new Dictionary<int, PointOrderManagement>();
        public void SetHighlightColors(int highlightIndex)
        {
            var highlightcolor = new Vector4(87f / 255f, 171f / 255f, 39f / 255f, 0f);
            Shader.ContourColor = new Vector4(1, 0, 0, 0);
            Shader.SupportColor = new Vector4(1, 0, 0, 0);
            if (highlightIndex == 1)
            {
                Shader.ContourColor = highlightcolor;
            }
            else if (highlightIndex == 2)
            {
                Shader.SupportColor = highlightcolor;
            }
            Shader.UseColors();
        }
        public Camera Camera { get; set; }

        PaintEventHandler _test;
        public Painter(GLControl gl, Form parent)
        {
            _gl = gl;
            _parent = parent;
            Init();
        }
        public Shader GetShader()
        {
            return Shader;
        }

        protected void Init()
        {
            _test = new PaintEventHandler(GLControlPaint);
            _gl.Paint += _test;
            _gl.Resize += CanvasResizeEvent;
            ResetCamera(_gl);
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
            Camera.MoveToPosition2D(CalculateCenter());
        }
        private Vector2 CalculateCenter()
        {
            //if (_vertices.Length > 4)
            //{
            //    float minX = _vertices[_grid.Length].Position.X;
            //    float maxX = _vertices[_grid.Length].Position.X;
            //    float minY = _vertices[_grid.Length].Position.Y;
            //    float maxY = _vertices[_grid.Length].Position.Y;

            //    for (int i = _grid.Length + 1; i < _vertices.Length; i++)
            //    {
            //        var x = _vertices[i].Position.X;
            //        var y = _vertices[i].Position.Y;
            //        if (x < minX)
            //        {
            //            minX = x;
            //        }
            //        else if (x > maxX)
            //        {
            //            maxX = x;
            //        }

            //        if (y < minY)
            //        {
            //            minY = y;
            //        }
            //        else if (y > maxY)
            //        {
            //            maxY = y;
            //        }

            //    }
            //    var center = new Vector2(minX + (maxX - minX) / 2, minY + (maxY - minY) / 2);
            //    return center;

            //}
            //else
            //{
            return Vector2.Zero;
            //}
        }

        void InitGrid()
        {
            _drawableGrid = new DrawableObject(Shader);
            var gridVertices = new Vertex[_grid.Length];
            _grid.GetGrid().CopyTo(gridVertices, 0);
            _drawableGrid.ChangePicture(gridVertices);
        }
        //public void SetLines(Vertex[] vertices, int numberOfLinesToDraw = 0)
        //{
        //    _vertices = null;
        //    _vertices = new Vertex[_grid.Length + vertices.Length];
        //    _grid.GetGrid().CopyTo(_vertices, 0);
        //    vertices.CopyTo(_vertices, _grid.Length);

        //    _numberOfLinesToDraw = numberOfLinesToDraw;

        //    MoveCameraWithLayer();
        //}
        //private void MoveCameraWithLayer()
        //{
        //    if (_vertices.Length > _grid.Length)
        //    {
        //        var zHeight = _vertices[_vertices.Length - 1].Position.Z;
        //        if (zHeight != _zHeight)
        //        {
        //            Camera.ChangeHeight(zHeight - _zHeight);
        //            _zHeight = zHeight;
        //        }
        //    }
        //}
        public void SetNumberOfLinesToDraw(int numberOfLinesToDraw)
        {
            if (numberOfLinesToDraw != 0)
            {
                _numberOfLinesToDraw = numberOfLinesToDraw;
            }
        }

        private void GLControlPaint(object sender, PaintEventArgs e)
        {
            if (!_eventRemoved)
            {
                _gl.Paint -= _test;
            }
            PrepareDrawing();
            Draw(0);
        }
        private void PrepareDrawing()
        {
            var display = _gl;
            display.Show();

            _gl.MakeCurrent();
            Shader = new Shader();

            GL.ClearColor(Color4.LightGray);

            Shader.Use();

            GL.ShadeModel(ShadingModel.Smooth);

            InitGrid();
            Shader.UseColors();
        }
        public void Draw(int layernumber = 0)
        {
            if (Shader != null)
            {
                // Clear color and depth buffers
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                Matrix4 modelViewProjection = Camera.TranslationMatrix * Camera.LookAtTransformationMatrix * Camera.ProjectionMatrix;

                GL.UniformMatrix4(Shader.GetTransformationMatrixLocation(), false, ref modelViewProjection);
                GL.LineWidth(5f);

                if (DrawableParts != null)
                {
                    var pointsPerPart = LayerPointManager[layernumber].GetPointNumbersToDraw(null, _numberOfLinesToDraw);

                    foreach (var part in DrawableParts.Values)
                    {
                        int numberOfPolylines = 0;
                        PartDrawInfo partDrawInfo;
                        pointsPerPart.TryGetValue(part.Partnumber, out partDrawInfo);
                        if (Is3d)
                        {
                            numberOfPolylines += partDrawInfo?.ContourNumberOfPoints ?? 0;
                            part.SetContourRangeToDraw3d(layernumber, numberOfPolylines);

                        }
                        else
                        {
                            numberOfPolylines += partDrawInfo?.ContourNumberOfPoints ?? 0;
                            part.SetContourRangeToDraw2d(numberOfPolylines, layernumber);

                        }

                        part.SetVolumeRangeToDraw(partDrawInfo?.HatchNumberOfPoints ?? 0);
                        part.DrawAll(modelViewProjection);
                    }
                }

                if (ShowGrid)
                {
                    _drawableGrid.Draw(modelViewProjection);
                }

                _gl.SwapBuffers();
                Application.DoEvents();
            }
        }

        //private void CreateVertexArray(int vertexBuffer)
        //{
        //    GL.CreateVertexArrays(1, out _vertexArray);
        //    GL.BindVertexArray(_vertexArray);
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);

        //    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 28, IntPtr.Zero);
        //    GL.EnableVertexAttribArray(0);
        //    GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 28, new IntPtr(12));
        //    GL.EnableVertexAttribArray(1);
        //    GL.BindVertexArray(_vertexArray);
        //}

        public void DisposeShader()
        {
            Shader.Dispose();
        }
    }
}
