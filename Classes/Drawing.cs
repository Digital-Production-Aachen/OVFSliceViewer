using OVFSliceViewer.Classes.ShaderNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using OpenTK;

namespace OVFSliceViewer.Classes
{
    public class DrawingVertexWithColor
    {
        Shader _shader;
        int _vertexBuffer;
        int _vertexArray;
        protected Vertex[] _vertices = new Vertex[0];
        protected int _numberOfLinesToDraw;

        public DrawingVertexWithColor(Shader shader)
        {
            _shader = shader;
            CreateVertexBuffer();
            CreateVertexArray();
        }

        public void ChangePicture(Vertex[] vertices)
        {
            _vertices = vertices;

            RebindBufferObject();
        }
        protected void RebindBufferObject()
        {
            var handle = GCHandle.Alloc(_vertices, GCHandleType.Pinned);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(28 * _vertices.Length), handle.AddrOfPinnedObject(), BufferUsageHint.StaticDraw);
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void SetNumberOfLinesToDraw(int numberOfLines)
        {
            _numberOfLinesToDraw = numberOfLines;
        }

        public void Draw(Matrix4 modelViewProjection)
        {
            GL.UniformMatrix4(_shader.GetUniformLocation(), false, ref modelViewProjection);

            GL.BindVertexArray(_vertexArray);
            GL.LineWidth(5f);
            GL.DrawArrays(PrimitiveType.Lines, 0, _numberOfLinesToDraw * 2);
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

        private void CreateVertexArray()
        {
            _vertexArray = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 28, IntPtr.Zero);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 28, new IntPtr(12));
            GL.EnableVertexAttribArray(1);
            GL.BindVertexArray(_vertexArray);
        }
    }
}
