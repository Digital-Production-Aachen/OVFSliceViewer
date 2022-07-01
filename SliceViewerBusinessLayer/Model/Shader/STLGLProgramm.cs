using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace OVFSliceViewerBusinessLayer.Model
{
    public class STLGLProgramm : GLProgramm
    {
        public STLGLProgramm(IRenderData renderObject, IModelViewProjection mvp, string vertexPath = "\\Classes\\Shader\\shader.vert", string fragmentPath = "\\Classes\\Shader\\shader.frag") : base(renderObject, mvp, vertexPath, fragmentPath)
        {
        }


        protected override void CreateVertexArray()
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
