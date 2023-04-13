using OpenTK.Graphics.OpenGL4;
//using OpenTK.Mathematics;

namespace OVFSliceViewerCore.Model
{
    public class TriangleShader: GLProgramm
    {
        public TriangleShader(IRenderData renderObject, IModelViewProjection mvp, string vertexShader, string fragmentShader) : base(renderObject, mvp, vertexShader, fragmentShader) { }

        protected override void CreateVertexArray()
        {
            GL.CreateVertexArrays(1, out _vertexArray);
            GL.BindVertexArray(_vertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 4 * sizeof(float), 4);
            GL.EnableVertexAttribArray(0);
            //GL.VertexAttribPointer(1, 1, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            //GL.EnableVertexAttribArray(1);
        }
    }



}
