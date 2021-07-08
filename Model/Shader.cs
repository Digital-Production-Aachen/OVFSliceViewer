using OpenTK.Graphics.OpenGL4;
//using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace LayerViewer.Model
{
    public class Shader: AbstrShader
    {
        protected int _vertexBuffer;
        protected int _vertexArray;
        protected readonly RenderObject _renderObject;

        public Shader(RenderObject renderObject, string vertexPath = @"/../Classes/Shader/shader.vert", string fragmentPath = @"/../Classes/Shader/shader.frag") : base(vertexPath, fragmentPath)
        {
            _renderObject = renderObject;
            CreateVertexBuffer();
            CreateVertexArray();

            Use();
        }

        public override void Render()
        {
            this.Use();
            //SetUniforms(guiObject);

            GL.BindVertexArray(_vertexArray);
            GL.DrawArrays(_renderObject.PrimitiveType, _renderObject.Start, _renderObject.End);
        }

        private void CreateVertexBuffer()
        {
            GL.CreateBuffers(1, out _vertexBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);

            var handle = GCHandle.Alloc(_renderObject.Vertices, GCHandleType.Pinned);
            try
            {
                GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_renderObject.Vertices.Length * sizeof(float)), handle.AddrOfPinnedObject(),
                    BufferUsageHint.StaticDraw);
            }
            finally
            {
                handle.Free();
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        }

        public override void BindNewData()
        {
            var handle = GCHandle.Alloc(_renderObject.Vertices, GCHandleType.Pinned);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_renderObject.SingleVertexSize * _renderObject.Vertices.Length), handle.AddrOfPinnedObject(), BufferUsageHint.StaticDraw);
            handle.Free();
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        protected void CreateVertexArray()
        {
            GL.CreateVertexArrays(1, out _vertexArray);
            GL.BindVertexArray(_vertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, _renderObject.SingleVertexSize, new IntPtr(4));
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 1, VertexAttribPointerType.Float, false, _renderObject.SingleVertexSize, IntPtr.Zero);
            GL.EnableVertexAttribArray(1);
        }
    }

    public abstract class AbstrShader
    {
        public int handle;
        string _vertexPath;
        string _fragmentPath;
        string _vertexShaderCode;
        string _fragmentShaderCode;

        protected AbstrShader(string vertexPath, string fragmentPath)
        {
            _vertexPath = vertexPath;
            _fragmentPath = fragmentPath;

            CompileShader();
        }

        public abstract void Render();
        public abstract void BindNewData();

        private void CompileShader()
        {
            bool isDebug = true;

            if (isDebug)
            {
                ReadShaderDebug();
            }
            else
            {
                ReadShaderDeploy();
            }



            var VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, _vertexShaderCode);

            var FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, _fragmentShaderCode);

            GL.CompileShader(VertexShader);

            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            if (infoLogVert != System.String.Empty)
                System.Console.WriteLine(infoLogVert);

            GL.CompileShader(FragmentShader);

            string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

            if (infoLogFrag != System.String.Empty)
                System.Console.WriteLine(infoLogFrag);

            handle = GL.CreateProgram();

            GL.AttachShader(handle, VertexShader);
            GL.AttachShader(handle, FragmentShader);

            GL.LinkProgram(handle);

            GL.DetachShader(handle, VertexShader);
            GL.DetachShader(handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
        }
        private void ReadShaderDebug()
        {
            using (StreamReader reader = new StreamReader(_vertexPath, Encoding.UTF8))
            {
                _vertexShaderCode = reader.ReadToEnd();
            }


            using (StreamReader reader = new StreamReader(_fragmentPath, Encoding.UTF8))
            {
                _fragmentShaderCode = reader.ReadToEnd();
            }
        }
        private void ReadShaderDeploy()
        {
            _vertexShaderCode = _vertexPath;
            _fragmentShaderCode = _fragmentPath;
        }
        public virtual void Use(double? iTime = null, int index = 0)
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
            GL.DeleteProgram(handle);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }



}
