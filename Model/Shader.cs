using OpenTK.Graphics.OpenGL4;
//using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace LayerViewer.Model
{
    public class Shader : AbstrShader
    {
        protected int _vertexBuffer;
        protected int _vertexArray;
        protected readonly IRenderData _renderObject;
        protected IModelViewProjection _mvp;
        
        protected int _colorPointer => GL.GetUniformLocation(handle, "highlightColor");

        public Shader(IRenderData renderObject, IModelViewProjection mvp, string vertexPath = @"Classes/Shader/shader.vert", string fragmentPath = @"Classes/Shader/shader.frag") : base(vertexPath, fragmentPath)
        {
            _renderObject = renderObject;
            _mvp = mvp;

            Use();
            CreateVertexBuffer();
            CreateVertexArray();
        }

        public override void Render()
        {
            this.Use();

            var mvp = _mvp.ModelViewProjection;
            //mvp = Matrix4.Identity;
            GL.UniformMatrix4(_mvpPointer, false, ref mvp);

            GL.Uniform4(_colorPointer, Color);
            GL.BindVertexArray(_vertexArray);
            GL.LineWidth(2.5f);
            GL.DrawArrays(_renderObject.PrimitiveType, _renderObject.Start, _renderObject.Vertices.Length);
        }

        private void CreateVertexBuffer()
        {
            GL.CreateBuffers(1, out _vertexBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);

            var handle = GCHandle.Alloc(_renderObject.Vertices, GCHandleType.Pinned);
            try
            {
                GL.BufferData(BufferTarget.ArrayBuffer,
                    new IntPtr(_renderObject.SingleVertexSize * _renderObject.Vertices.Length),
                    handle.AddrOfPinnedObject(),
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
            var test = _renderObject.Vertices.ToArray();
            var handle = GCHandle.Alloc(_renderObject.Vertices, GCHandleType.Pinned);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            //GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(3* sizeof(float) * _renderObject.Vertices.Length), handle.AddrOfPinnedObject(), BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_renderObject.SingleVertexSize * _renderObject.Vertices.Length), handle.AddrOfPinnedObject(), BufferUsageHint.StaticDraw);
            handle.Free();
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        protected virtual void CreateVertexArray()
        {
            GL.CreateVertexArrays(1, out _vertexArray);
            GL.BindVertexArray(_vertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 4 * sizeof(float), 4);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 1, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);

            //GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }
    }
}
