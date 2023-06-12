using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using SliceViewerBusinessLayer.Model.Shader;
using System;

namespace OVFSliceViewerCore.Model
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
            _shader = new OVFProgramm(this, mvp, VertexShader.Shader, FragmentShader.Shader);
        }
    }
}
