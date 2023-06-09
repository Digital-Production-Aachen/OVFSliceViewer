using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SliceViewerBusinessLayer.Model.Shader;
//using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace OVFSliceViewerCore.Model
{
    public abstract class AbstrGlProgramm : IDisposable
    {
        public int _handle;
        //protected string _vertexPath;
        //protected string _fragmentPath;
        //protected string _geometryPath;
        protected string _vertexShaderCode;
        protected string _fragmentShaderCode;
        protected string _geometryShaderCode;
        protected int _mvpPointer => GL.GetUniformLocation(_handle, "Mvp");


        public Vector4 Color { get; set; } = new Vector4(1, 0, 0, 0);
        protected AbstrGlProgramm(string vertexShader, string fragmentShader, string geometryShader = "")
        {
            var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            _vertexShaderCode = vertexShader;
            _fragmentShaderCode = fragmentShader;

            _geometryShaderCode = geometryShader;

            CompileShader();
        }

        public abstract void Render();
        public abstract void BindNewData();

        protected virtual void CompileShader()
        {
            //ToDo: compile shader if one is given

            ReadShader();

            
            


            var VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, _vertexShaderCode);

            int geometryShader = -1;

            if (!string.IsNullOrWhiteSpace(_geometryShaderCode))
            {
                geometryShader = GL.CreateShader(ShaderType.GeometryShader);
                GL.ShaderSource(geometryShader, _geometryShaderCode);
                GL.CompileShader(geometryShader);
                string infoLogGeom = GL.GetShaderInfoLog(geometryShader);
                if (infoLogGeom != System.String.Empty)
                    Debug.WriteLine(infoLogGeom);
            }

            var FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, _fragmentShaderCode);

            GL.CompileShader(VertexShader);
            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            if (infoLogVert != System.String.Empty)
                Debug.WriteLine($"Error in vertex-shader compilation in  AbstrGlProgramm: {infoLogVert}");

            GL.CompileShader(FragmentShader);
            string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);
            if (infoLogFrag != System.String.Empty)
                Debug.WriteLine($"Error in fragment-shader compilation in  AbstrGlProgramm: {infoLogFrag}");

            _handle = GL.CreateProgram();

            if (!string.IsNullOrWhiteSpace(_geometryShaderCode))
                AttachShader(new List<int>() { VertexShader, geometryShader, FragmentShader });
            else
                AttachShader(new List<int>() { VertexShader, FragmentShader });
            //GL.AttachShader(_handle, VertexShader);
            //GL.AttachShader(_handle, FragmentShader);

            GL.LinkProgram(_handle);
            GL.ValidateProgram(_handle);

            GL.DetachShader(_handle, geometryShader);
            GL.DetachShader(_handle, VertexShader);
            GL.DetachShader(_handle, FragmentShader);
            if(!string.IsNullOrWhiteSpace(_geometryShaderCode))
                GL.DeleteShader(geometryShader);
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
