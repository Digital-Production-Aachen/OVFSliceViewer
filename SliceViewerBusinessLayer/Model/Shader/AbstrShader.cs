using OpenTK;
using OpenTK.Graphics.OpenGL4;
//using OpenTK.Mathematics;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace OVFSliceViewerBusinessLayer.Model
{
    public abstract class AbstrShader : IDisposable
    {
        public int handle;
        protected string _vertexPath;
        protected string _fragmentPath;
        protected string _vertexShaderCode;
        protected string _fragmentShaderCode;
        protected int _mvpPointer => GL.GetUniformLocation(handle, "Mvp");


        public Vector4 Color { get; set; } = new Vector4(1, 0, 0, 0);
        protected AbstrShader(string vertexPath, string fragmentPath)
        {
            var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);


            _vertexPath = path + vertexPath;
            _fragmentPath = path + fragmentPath;

            CompileShader();
        }

        public abstract void Render();
        public abstract void BindNewData();

        protected virtual void CompileShader()
        {
            ReadShader();
            var VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, _vertexShaderCode);

            var FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, _fragmentShaderCode);

            GL.CompileShader(VertexShader);

            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            if (infoLogVert != System.String.Empty)
                Debug.WriteLine(infoLogVert);

            GL.CompileShader(FragmentShader);

            string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

            if (infoLogFrag != System.String.Empty)
                Debug.WriteLine(infoLogFrag);

            handle = GL.CreateProgram();

            GL.AttachShader(handle, VertexShader);
            GL.AttachShader(handle, FragmentShader);

            GL.LinkProgram(handle);
            GL.ValidateProgram(handle);

            infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

            if (infoLogFrag != System.String.Empty)
                Debug.WriteLine(infoLogFrag);

            GL.DetachShader(handle, VertexShader);
            GL.DetachShader(handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
        }
        protected void ReadShader()
        {
            using (StreamReader reader = new StreamReader(_vertexPath, Encoding.UTF8))
            {
                _vertexShaderCode = reader.ReadToEnd();
            }


            using (StreamReader reader = new StreamReader(_fragmentPath, Encoding.UTF8))
            {
                _fragmentShaderCode = reader.ReadToEnd();
            }

            Debug.WriteLine(GL.GetError());
        }
        public virtual void Use()
        {
            GL.UseProgram(handle);
        }
        public void Recompile()
        {
            GL.DeleteProgram(handle);
            CompileShader();
        }

        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(handle);

                disposedValue = true;
            }
        }
        ~AbstrShader()
        {
            Dispose(disposing: false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }



}
