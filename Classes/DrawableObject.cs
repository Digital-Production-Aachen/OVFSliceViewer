using OVFSliceViewer.Classes.ShaderNamespace;
using System;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using OpenTK;

namespace OVFSliceViewer.Classes
{
    public class DrawableObject
    {
        Shader _shader;
        int _vertexBuffer;
        int _vertexArray;
        protected Vertex[] _vertices = new Vertex[0];
        int _beginDrawingAt = 0;
        int _stopDrawingAt = 0;
        int _vertexSize = Marshal.SizeOf(typeof(Vertex));
        public DrawableObject(Shader shader, int buffer)
        {
            _shader = shader;
            _vertexBuffer = buffer;
            //CreateVertexBuffer();
            CreateVertexArray();

        }

        public void SetRangeToDraw(int end, int start = 0)
        {
            _beginDrawingAt = start;
            _stopDrawingAt = end;
        }
        public void ChangePicture(Vertex[] vertices)
        {
            _vertices = vertices;
            _stopDrawingAt = vertices.Length;
            RebindBufferObject();
        }
        protected void RebindBufferObject()
        {
            var handle = GCHandle.Alloc(_vertices, GCHandleType.Pinned);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_vertexSize * _vertices.Length), handle.AddrOfPinnedObject(), BufferUsageHint.StaticDraw);
            handle.Free();
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        //public void SetNumberOfLinesToDraw(int numberOfLines)
        //{
        //    _stopDrawingAt = numberOfLines*2;
        //}

        public void Draw(Matrix4 modelViewProjection)
        {
            GL.UniformMatrix4(_shader.GetUniformLocation(), false, ref modelViewProjection);

            GL.BindVertexArray(_vertexArray);
            GL.LineWidth(2.5f);
            GL.DrawArrays(PrimitiveType.Lines, _beginDrawingAt, _stopDrawingAt-_beginDrawingAt);
        }

        private void CreateVertexBuffer()
        {
            //GL.GenBuffers(2, out _vertexBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);

            var handle = GCHandle.Alloc(_vertices, GCHandleType.Pinned);
            try
            {
                GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_vertexSize * _vertices.Length), handle.AddrOfPinnedObject(),
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
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, _vertexSize, new IntPtr(4));
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 1, VertexAttribPointerType.Float, false, _vertexSize, IntPtr.Zero);
            GL.EnableVertexAttribArray(1);
            //1 xyz rgba
            //x yzr gb 
        }
    }
}
