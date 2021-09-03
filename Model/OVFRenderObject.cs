using OpenTK;
using OpenTK.Graphics;

namespace LayerViewer.Model
{
    public class OVFRenderObject : RenderObject
    {
        public enum ViewerPartArea 
        {
            Contour,
            Volume,
            SupportContour,
            SupportVolume
        }
        public readonly ViewerPartArea Type;
        public OVFRenderObject(IModelViewProjection mvp, ViewerPartArea viewerPartArea, Vector4 color ) : base(mvp) 
        {
            Type = viewerPartArea;
            SetColor(color);
        }
        public void SetColor(Vector4 color)
        {
            _shader.Color = color;
        }
    }

    public class ThreeDimensionalLinesRenderObject: OVFRenderObject
    {
        public ThreeDimensionalLinesRenderObject(IModelViewProjection mvp, ViewerPartArea viewerPartArea, Vector4 color) : base(mvp, viewerPartArea, color)
        {

        }

        protected override void CreateShader(IModelViewProjection mvp)
        {
            _shader = new GeometryShader(this, mvp);
        }
    }
}
