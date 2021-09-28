using OpenTK;
using OpenTK.Graphics;
using System;

namespace OVFSliceViewerBusinessLayer.Model
{
    public class OVFRenderObject : RenderDataObject
    {
        public enum ViewerPartArea 
        {
            Contour,
            Volume,
            SupportContour,
            SupportVolume
        }
        public readonly ViewerPartArea Type;
        public OVFRenderObject(Func<bool> useColorIndex, IModelViewProjection mvp, ViewerPartArea viewerPartArea, Vector4 color ) : base(useColorIndex, mvp) 
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
        public ThreeDimensionalLinesRenderObject(Func<bool> useColorIndex, IModelViewProjection mvp, ViewerPartArea viewerPartArea, Vector4 color) : base(useColorIndex, mvp, viewerPartArea, color)
        {

        }

        protected override void CreateShader(IModelViewProjection mvp)
        {
            _shader = new GeometryShader(this, mvp);
        }
    }
}
