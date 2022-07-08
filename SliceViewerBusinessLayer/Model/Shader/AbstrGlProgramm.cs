using OpenTK;
using OpenTK.Graphics.OpenGL4;
//using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace OVFSliceViewerBusinessLayer.Model
{
    public abstract class AbstrGlProgramm : IDisposable
    {
        public int _handle;
        protected string _vertexPath;
        protected string _fragmentPath;
        protected string _geometryPath;
        protected string _vertexShaderCode;
        protected string _fragmentShaderCode;
        protected string _geometryShaderCode;
        protected int _mvpPointer => GL.GetUniformLocation(_handle, "Mvp");


        public Vector4 Color { get; set; } = new Vector4(1, 0, 0, 0);
        protected AbstrGlProgramm(string vertexPath, string fragmentPath)
        {
            var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            _vertexPath = path + vertexPath;
            _fragmentPath = path + fragmentPath;

            _geometryPath = path + @"\Classes\Shader\shader.geometry";

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

            _handle = GL.CreateProgram();


            AttachShader(new List<int>() { VertexShader, FragmentShader });

            //GL.AttachShader(_handle, VertexShader);
            //GL.AttachShader(_handle, FragmentShader);

            GL.LinkProgram(_handle);
            GL.ValidateProgram(_handle);

            infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

            if (infoLogFrag != System.String.Empty)
                Debug.WriteLine(infoLogFrag);

            GL.DetachShader(_handle, VertexShader);
            GL.DetachShader(_handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
        }
        
        protected virtual void AttachShader(List<int> shaderHandles)
        {
            foreach (var shaderHandle in shaderHandles)
            {
                GL.AttachShader(_handle, shaderHandle);
            }
        }
        protected void ReadShader()
        {
            _vertexShaderCode = ReadShader(_vertexPath);
            _fragmentShaderCode = ReadShader(_fragmentPath);
            _geometryShaderCode = ReadShader(_geometryPath);

            Debug.WriteLine(GL.GetError());
        }
        protected string ReadShader(string path)
        {
            string code;
            using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
            {
                code = reader.ReadToEnd();
            }
            return code;
        }
        public virtual void Use()
        {
            GL.UseProgram(_handle);
        }
        public void Recompile()
        {
            GL.DeleteProgram(_handle);
            CompileShader();
        }

        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                //GL.DeleteProgram(_handle);

                disposedValue = true;
            }
        }
        ~AbstrGlProgramm()
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
