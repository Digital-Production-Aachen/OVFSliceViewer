using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OVFSliceViewer
{
    public class Painter
    {
        protected GLControl _gl;
        protected Form _parent;
        protected bool _isDrawing = true;
        protected bool _eventRemoved = false;
        protected Matrix4 _view;
        protected int _numberOfLinesToDraw = 6;
        protected bool _is2D = true;
        protected float _zHeight;
        protected int _vertexBuffer;
        protected int _vertexArray;

        Vertex[] _vertices = new Vertex[6]{
                new Vertex { Color = new OpenTK.Vector4(1,0,0,0), Position = new OpenTK.Vector3(0, 0, 0)},
                new Vertex { Color = new OpenTK.Vector4(1,0,0,0), Position = new OpenTK.Vector3(20f, 0, 0)},
                new Vertex { Color = new OpenTK.Vector4(0,1,0,0), Position = new OpenTK.Vector3(0, 0, 0)},
                new Vertex { Color = new OpenTK.Vector4(0,1,0,0), Position = new OpenTK.Vector3(0, 20f, 0)},
                new Vertex { Color = new OpenTK.Vector4(0,0,1,0), Position = new OpenTK.Vector3(0, 0, 0)},
                new Vertex { Color = new OpenTK.Vector4(0,0,1,0), Position = new OpenTK.Vector3(0, 0, 20f)}
        };

        public Camera Camera { get; set; }
        public IRotateable Rotation => Camera;
        int _mvp;
        PaintEventHandler _test;
        public Painter(GLControl gl, Form parent)
        {
            _gl = gl;
            _parent = parent;
            Init();
        }

        protected void Init()
        {
            _parent.FormClosing += (sender2, e2) => _isDrawing = false;
            _test = new PaintEventHandler(this.GLControlPaint);
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
            var minX = _vertices.Min(x => x.Position.X);
            var maxX = _vertices.Max(x => x.Position.X);
            var minY = _vertices.Min(x => x.Position.Y);
            var maxY = _vertices.Max(x => x.Position.Y);

            var center = new Vector2(minX + (maxX - minX) / 2, minY + (maxY - minY) / 2);

            Camera.MoveToPosition2D(center);
        }

        public void SetLinesAndDraw(Vertex[] vertices, int numberOfLinesToDraw = 0)
        {
            SetLines(vertices, numberOfLinesToDraw);
            RebindBufferObject();
            Draw();
        }
        public void SetLines(Vertex[] vertices, int numberOfLinesToDraw = 0)
        {
            _vertices = null;
            _vertices = vertices;

            _numberOfLinesToDraw = numberOfLinesToDraw;

            MoveCameraWithLayer();
        }
        private void MoveCameraWithLayer()
        {
            if (_vertices.Any())
            {
                var zHeight = _vertices[0].Position.Z;
                if (zHeight != _zHeight)
                {
                    Camera.ChangeHeight(zHeight - _zHeight);
                    _zHeight = zHeight;
                }
            }
        }
        private void RebindBufferObject()
        {
            var handle = GCHandle.Alloc(_vertices, GCHandleType.Pinned);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(28 * _vertices.Length), handle.AddrOfPinnedObject(), BufferUsageHint.StaticDraw);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
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

            GL.ClearColor(Color4.Black);
            CreateVertexBuffer();
            CreateVertexArray(_vertexBuffer);

            const string vertexShaderCode =
                "#version 150 core\n" +
                "uniform mat4 Mvp; in vec4 color; in vec3 position; out vec4 fragcolor; void main() { gl_Position = Mvp * vec4(position, 1); fragcolor = color; }";

            const string fragmentShaderCode = "#version 150 core\n" +
                "in vec4 fragcolor; out vec4 FragColor; void main() { FragColor = fragcolor; }";

            var vertexShader = CreateVertexShader(vertexShaderCode);
            var fragmentShader = CreateFragmentShader(fragmentShaderCode);

            // Create the shader program
            int program = GL.CreateProgram();

            // Attach the shaders to the program
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            // Set FragColor as output fragment variable
            GL.BindFragDataLocation(program, 0, "FragColor");
            GL.BindAttribLocation(program, 0, "position");
            GL.BindAttribLocation(program, 1, "color");
            GL.LinkProgram(program);
            GL.ValidateProgram(program);
            GL.UseProgram(program);
            //GL.Enable(EnableCap.)
            GL.ShadeModel(ShadingModel.Flat);

            // Get the uniform location of mvp which we will use to set the value with later on.
            _mvp = GL.GetUniformLocation(program, "Mvp");


            //float angle = 0;

            //var lastTimestamp = Stopwatch.GetTimestamp();
            //var freq = Stopwatch.Frequency;
        }
        public void Draw()
        {
            //float angle = 0;

            //var timeStamp = Stopwatch.GetTimestamp();
            //var angle += 0;//(float)((timeStamp - lastTimestamp) / (double)freq / 2);
            //lastTimestamp = timeStamp;


            // Clear color and depth buffers
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Matrix4 modelViewProjection = Camera.TransformationMatrix * Camera.ProjectionMatrix;
            GL.UniformMatrix4(_mvp, false, ref modelViewProjection);

            GL.BindVertexArray(_vertexArray);
            GL.LineWidth(5f);
            GL.DrawArrays(PrimitiveType.Lines, 0, _numberOfLinesToDraw * 2);
            _gl.SwapBuffers();
            Application.DoEvents();

        }
        
        private void CreateVertexBuffer()
        {
            _vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);

            var handle = GCHandle.Alloc(_vertices, GCHandleType.Pinned);
            try
            {
                GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(28 * _vertices.Length), handle.AddrOfPinnedObject(),
                    BufferUsageHint.StaticDraw);
            }
            finally
            {
                handle.Free();
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        }

        private int CreateVertexShader(string code)
        {
            var shader = GL.CreateShader(ShaderType.VertexShader);

            GL.ShaderSource(shader, code);

            GL.CompileShader(shader);
            string infoLogVert = GL.GetShaderInfoLog(shader);
            //Console.WriteLine(infoLogVert);

            return shader;
        }

        private int CreateFragmentShader(string code)
        {
            var shader = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(shader, code);

            GL.CompileShader(shader);
            string infoLogVert = GL.GetShaderInfoLog(shader);
            //Console.WriteLine(infoLogVert);

            return shader;
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
    }
}
