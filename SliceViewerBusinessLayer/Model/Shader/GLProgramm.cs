using OpenTK;
using OpenTK.Graphics.OpenGL4;
//using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OVFSliceViewerBusinessLayer.Model
{
    public class GLProgramm : AbstrGlProgramm
    {
        protected int _vertexBuffer;
        protected int _vertexArray;
        protected readonly IRenderData _renderObject;
        protected IModelViewProjection _mvp;
        
        protected int _colorPointer => GL.GetUniformLocation(handle, "colorIndex");

        public GLProgramm(IRenderData renderObject, IModelViewProjection mvp, string vertexPath = @"\Classes\Shader\shader.vert", string fragmentPath = @"\Classes\Shader\shader.frag") : base(vertexPath, fragmentPath)
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
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.DepthTest);

            if (_renderObject.UseColorIndex())
            {               
                if (_renderObject.ColorIndexRange.Count == 1 || _renderObject.ColorIndexRange.Select(x => x.ColorIndex).Distinct().Count() == 1)
                {
                    RenderWithSingleDraw(_renderObject.ColorIndexRange[0].ColorIndex);
                }
                else
                {
                    RenderWithMultipleDraws();
                }
            }
            else
            {
                RenderWithSingleDraw();
            }
            //Debug.WriteLine(GL.GetError());
        }

        private void RenderWithSingleDraw(int colorIndex = 0)
        {
            var mvp = _mvp.ModelViewProjection;
            GL.UniformMatrix4(_mvpPointer, false, ref mvp);

            GL.Uniform1(_colorPointer, colorIndex);
            GL.BindVertexArray(_vertexArray);
            GL.LineWidth(2.5f);
            GL.DrawArrays(_renderObject.PrimitiveType, _renderObject.Start, _renderObject.End);
            Debug.WriteLine(GL.GetError());
        }
        private void RenderWithMultipleDraws()
        {
            int lastColorIndex = -1;
            var mvp = _mvp.ModelViewProjection;
            GL.UniformMatrix4(_mvpPointer, false, ref mvp);
            GL.BindVertexArray(_vertexArray);

            foreach (var item in _renderObject.ColorIndexRange.Where(x => x.Range.End > x.Range.Start && x.Range.Start < _renderObject.End).OrderBy(x => x.ColorIndex))
            {
                if (lastColorIndex != item.ColorIndex)
                {
                    GL.Uniform1(_colorPointer, item.ColorIndex);
                    lastColorIndex = item.ColorIndex;
                }
                
                GL.LineWidth(2.5f);
                GL.DrawArrays(_renderObject.PrimitiveType, Math.Min(item.Range.Start,_renderObject.End), Math.Max(0, Math.Min(item.Range.End, _renderObject.End) - item.Range.Start));
                var test = Math.Min(item.Range.End, _renderObject.End) - item.Range.Start;
            }
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
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_renderObject.SingleVertexSize * _renderObject.Vertices.Length), handle.AddrOfPinnedObject(), BufferUsageHint.StaticDraw);
            handle.Free();
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
        }
    }
}
