using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayerViewer
{
    public class Painter: IMoveable
    {
        protected GLControl _gl;
        protected Form _parent;
        protected bool _isDrawing = true;
        protected bool _eventRemoved = false;
        protected float _deltaX;
        protected float _deltaY;
        protected Matrix4 _view;
        //protected Matrix4 _scale;
        protected Matrix4 _projection;
        protected Matrix4 _translate;
        protected float _fieldOfView;
        protected float _aspectRatio;
        protected int _numberOfLinesToDraw = 6;
        protected bool _is2D = true;
        protected float _zHeight;
        protected Thread _thread;
        int _numberOfRuns = 0;
        int _numberOfStops = 0;
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
            //_gl.Invalidated += (sender2, e2) => test();//_isDrawing = false;
            //_gl.Move += (sender2, e2) => _isDrawing = false;
            _test = new System.Windows.Forms.PaintEventHandler(this.GLControl_Paint);
            _gl.Paint += _test;
            //_gl.Resize += new EventHandler(glControl_Resize);
            
            InitCamera();
            _fieldOfView = ((float)Math.PI / 180) * 80;
            _aspectRatio = Convert.ToSingle(_gl.Width) / Convert.ToSingle(_gl.Height);
            _projection = Matrix4.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio /*640.0f / 480.0f*/, 0.1f, 100f);
            _translate = Matrix4.CreateTranslation(0, 0, 0);
        }
        void glControl_Resize(object sender, EventArgs e)
        {
            OpenTK.GLControl c = sender as OpenTK.GLControl;

            if (c.ClientSize.Height == 0)
                c.ClientSize = new System.Drawing.Size(c.ClientSize.Width, 1);

            GL.Viewport(c.Left, c.Right, c.ClientSize.Width, c.ClientSize.Height);

            float aspect_ratio = c.ClientSize.Width / (float)c.ClientSize.Height;
            //Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64);
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadMatrix(ref perpective);
        }
        public void test()
        {
            _isDrawing = false;
            Console.WriteLine("test");
        }

        protected float Scalefactor = 1.25f;
        public void Scale(float scalefactor)
        {
            var zObject = _vertices[0].Position.Z;
            Scalefactor *= scalefactor;
            _camera.Z = zObject + scalefactor;
            _camera.Z = _camera.Z - zObject < 0.1f ? zObject + 0.11f : _camera.Z;
            _camera.Z = _camera.Z - zObject > 100f ? zObject + 99f : _camera.Z;
            InitCamera();
        }

        protected void InitCamera()
        {
            var target = _camera;
            target.Z = 0;
            _view = Matrix4.LookAt(_camera, target, OpenTK.Vector3.UnitY);
        }

        protected Vector3 _camera = new Vector3(0f, 0, 1.25f);
        protected Vector3 _unmovedCamera = new Vector3(0, 0, 1f);
        public void Move(System.Numerics.Vector2 delta)
        {
            //var temp = _translate.ExtractTranslation();

            //_camera.X -= deltaX/_gl.Width;
            //_camera.Y -= deltaY/_gl.Height;
            var test = new Vector4(2f * delta.X / 1.678f / 1564f, 2f * delta.Y / 1.678f / 825f, 0, 1);
            //test = test * _projection ;

            if (_isMoving)
            {
                var deltaZ = _camera.Z - _vertices[0].Position.Z;
                var deltaXInPicture = (2 * delta.X / _gl.Width) * Math.Tan(_fieldOfView / 2) * deltaZ * _aspectRatio;
                var deltaYInPicture = (2 * delta.Y / _gl.Height) * Math.Tan(_fieldOfView / 2) * deltaZ;
                _camera.X = _unmovedCamera.X - Convert.ToSingle(deltaXInPicture);
                _camera.Y = _unmovedCamera.Y + Convert.ToSingle(deltaYInPicture);
            }
            else
            {
                _camera.X -= test.X;
                _camera.Y -= test.Y;
            }

            InitCamera();

            //_translate = Matrix4.CreateTranslation(temp.X+deltaX, temp.Y+deltaY, temp.Z);
        }
        public void StartMove()
        {
            _isMoving = true;
            _unmovedCamera = _camera;
        }
        public void StopMove()
        {
            _isMoving = false;
        }
        protected bool _isMoving = false;

        Vector3 _targetCenter = new Vector3();
        protected void TargetCenter()
        {
            var minX = _vertices.Min(x => x.Position.X);
            var maxX = _vertices.Max(x => x.Position.X);

            var minY = _vertices.Min(x => x.Position.Y);
            var maxY = _vertices.Max(x => x.Position.Y);

            var activeTranslation = _translate.ExtractTranslation();

            _targetCenter.X = minX + (maxX - minX) / 2;
            _targetCenter.Y = minY + (maxY - minY) / 2;
        }

        public void SetLinesToDraw(Vertex[] vertices, int numberOfLinesToDraw = 0)
        {
            _vertices = vertices;
            SetNumberOfLinesToDraw(numberOfLinesToDraw);
            //TargetCenter();

            var handle = GCHandle.Alloc(_vertices, GCHandleType.Pinned);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(28 * _vertices.Length), handle.AddrOfPinnedObject(), BufferUsageHint.StaticDraw);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (_is2D)
            {
                var zHeight = _vertices[0].Position.Z;

                if (zHeight != _zHeight)
                {
                    //MoveCamera(zHeight - _zHeight);
                    _camera.Z += (zHeight - _zHeight);
                    _zHeight = zHeight;
                    InitCamera();
                }
            }
        }
        public void SetNumberOfLinesToDraw(int numberOfLinesToDraw)
        {
            if (numberOfLinesToDraw == 0)
            {
                _numberOfLinesToDraw = _vertices.Count();
            }
            else _numberOfLinesToDraw = numberOfLinesToDraw;
        }

        private void GLControl_Paint(object sender, PaintEventArgs e)
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

            // The most basic of fragment shaders.
            //const string fragmentShaderCode = "in vec4 fragcolor; out vec4 FragColor; void main() { FragColor = fragcolor; }";
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
        private void Draw()
        {
            float angle = 0;
            do
            {
                var timeStamp = Stopwatch.GetTimestamp();
                //var angle += 0;//(float)((timeStamp - lastTimestamp) / (double)freq / 2);
                //lastTimestamp = timeStamp;
                // Clear color and depth buffers
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                var model = Matrix4.CreateFromAxisAngle(OpenTK.Vector3.UnitY, 0);   // Create rotation matrix
                //Matrix4 modelViewProjection = _scale * _translate * model * _view * _projection;
                Matrix4 modelViewProjection = _translate * model * _view * _projection;
                GL.UniformMatrix4(_mvp, false, ref modelViewProjection);

                GL.BindVertexArray(_vertexArray);
                GL.LineWidth(5f);
                GL.DrawArrays(PrimitiveType.Lines, 0, _numberOfLinesToDraw * 2);
                _gl.SwapBuffers();
                Application.DoEvents();
            } while (_isDrawing);
        }

        int _vertexBuffer;
        int _vertexArray;
        Vertex[] _vertices = new Vertex[6]{
                new Vertex { Color = new OpenTK.Vector4(255,1,1,0), Position = new OpenTK.Vector3(0.0f, 1f, 0)},
                new Vertex { Color = new OpenTK.Vector4(255,220,0,0), Position = new OpenTK.Vector3(0.0f, -1f, 0)},
                new Vertex { Color = new OpenTK.Vector4(255,0,0,0), Position = new OpenTK.Vector3(0.0f, -1f, 0)},
                new Vertex { Color = new OpenTK.Vector4(255,1,1,0), Position = new OpenTK.Vector3(-1f, 0.0f, 0)},
                new Vertex { Color = new OpenTK.Vector4(255,1,1,0), Position = new OpenTK.Vector3(-1f, 0.0f, 0)},
                new Vertex { Color = new OpenTK.Vector4(255,1,1,0), Position = new OpenTK.Vector3(0.0f, 1f, 0)}
        };
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
