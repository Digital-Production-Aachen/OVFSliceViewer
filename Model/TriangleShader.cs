using OpenTK.Graphics.OpenGL4;
//using OpenTK.Mathematics;

namespace LayerViewer.Model
{
    public class TriangleShader: Shader
    {
        public TriangleShader(IRenderData renderObject, IModelViewProjection mvp) : base(renderObject, mvp) { }

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
