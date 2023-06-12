using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OVFSliceViewerCore.Model.Shader;
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
        protected int _mvpPointer => ShaderController.GetInstance.MvpPointer;


        public Vector4 Color { get; set; } = new Vector4(1, 0, 0, 0);
        protected AbstrGlProgramm(string vertexShader, string fragmentShader, string geometryShader = "")
        {
            var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            _vertexShaderCode = vertexShader;
            _fragmentShaderCode = fragmentShader;

            _geometryShaderCode = geometryShader;

            //CompileShader();
        }

        public abstract void Render();
        public abstract void BindNewData();

        

        
        
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
        public abstract void Use();

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
