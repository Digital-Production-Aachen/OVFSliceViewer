using System;

namespace OVFSliceViewerBusinessLayer.Model
{
    public class STLRenderDataObject : RenderDataObject
    {
        public STLRenderDataObject(Func<bool> useColorIndex, IModelViewProjection mvp) : base(useColorIndex, mvp)
        {
            this.PrimitiveType = OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles;
        }
    }
}
