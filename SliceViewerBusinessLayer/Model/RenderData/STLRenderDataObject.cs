using SliceViewerBusinessLayer.Model.Shader;
using System;

namespace OVFSliceViewerBusinessLayer.Model
{
    public class STLRenderDataObject : RenderDataObject
    {
        public STLRenderDataObject(Func<bool> useColorIndex, IModelViewProjection mvp) : base(useColorIndex, mvp)
        {
            this.PrimitiveType = OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles;
        }

        protected override void CreateShader(IModelViewProjection mvp)
        {
            _shader = new STLGLProgramm(this, mvp, STLShader.Shader, FragmentShader.Shader);
        }
    }
}
