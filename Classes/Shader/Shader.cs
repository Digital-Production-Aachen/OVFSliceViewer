using OpenTK.Graphics.OpenGL;
using System;
using System.IO;
using System.Text;

namespace OVFSliceViewer.Classes.ShaderNamespace
{
    public class Shader: IDisposable
    {      
        int Handle;
        private bool disposedValue = false;
        int _vertexShader;
        int _fragmentShader;
        int _mvp;
        int _mainColor;
        int _supportColor;
        int _contourColor;

        public Shader(string vertexPath = @"Classes/Shader/shader.vert", string fragmentPath = @"Classes/Shader/shader.frag")
        {
            string vertexShaderSource;
            string fragmentShaderSource;

            //vertexShaderSource = File.ReadAllText(vertexPath);

            using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
            {
                vertexShaderSource = reader.ReadToEnd();
            }
            using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8))
            {
                fragmentShaderSource = reader.ReadToEnd();
            }

            CreateProgram(vertexShaderSource, fragmentShaderSource);
        }

        private void CreateProgram(string vertexShaderSource, string fragmentShaderSource)
        {
            CreateVertexShader(vertexShaderSource);
            CreateFragmentShader(fragmentShaderSource);

            Handle = GL.CreateProgram();

            CombineShader();

            
        }

        public int GetUniformLocation()
        {
            return _mvp;
        }

        public int GetMainColorLocation()
        {
            return _mainColor;
        }

        public int GetSupportColorLocation()
        {
            return _supportColor;
        }

        public int GetContourColorLocation()
        {
            return _contourColor;
        }

        private int CreateVertexShader(string vertexShaderSource)
        {
            _vertexShader = GL.CreateShader(ShaderType.VertexShader);

            GL.ShaderSource(_vertexShader, vertexShaderSource);
            GL.CompileShader(_vertexShader);
            string infoLogVert = GL.GetShaderInfoLog(_vertexShader); // use for debug
            Console.WriteLine(infoLogVert);

            return _vertexShader;
        }

        private int CreateFragmentShader(string fragmentShaderSource)
        {
            _fragmentShader = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(_fragmentShader, fragmentShaderSource);
            GL.CompileShader(_fragmentShader);
            string infoLogVert = GL.GetShaderInfoLog(_fragmentShader);
            Console.WriteLine(infoLogVert);

            return _fragmentShader;
        }

        private void CombineShader()
        {
            GL.AttachShader(Handle, _vertexShader);
            GL.AttachShader(Handle, _fragmentShader);
            
            GL.BindFragDataLocation(Handle, 0, "FragColor");
            GL.BindAttribLocation(Handle, 0, "position");
            GL.BindAttribLocation(Handle, 1, "colorIndex");

            GL.LinkProgram(Handle);
            GL.ValidateProgram(Handle);
            //GL.UseProgram(Handle);

            _mvp = GL.GetUniformLocation(Handle, "Mvp");
            _mainColor = GL.GetUniformLocation(Handle, "mainColor");
            _contourColor = GL.GetUniformLocation(Handle, "contourColor");
            _supportColor = GL.GetUniformLocation(Handle, "supportColor");

            GL.DetachShader(Handle, _vertexShader);
            GL.DetachShader(Handle, _fragmentShader);
            GL.DeleteShader(_fragmentShader);
            GL.DeleteShader(_vertexShader);
        }

        public void Use()
        {
            
            GL.UseProgram(Handle);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Shader()
        {
            GL.DeleteProgram(Handle);
        }
    }
}
