using OpenTK.Graphics.OpenGL4;

namespace LayerViewer.Model
{
    public class STLRenderObject: RenderObject
    {
        public STLRenderObject(IModelViewProjection mvp) : base(mvp) 
        {
            this.PrimitiveType = PrimitiveType.Triangles;
        }
        protected override void CreateShader(IModelViewProjection mvp)
        {
            _shader = new TriangleShader(this, mvp);
        }
    }
}
