using SliceViewerBusinessLayer.Model.Shader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OVFSliceViewerCore.Model.RenderData
{
    public class VoxelRenderDataObject : RenderDataObject
    {
        public VoxelRenderDataObject(Func<bool> useColorIndex, IModelViewProjection mvp) : base(useColorIndex, mvp)
        {

        }

        protected override void CreateShader(IModelViewProjection mvp)
        {
            //ToDo: this thing
            _shader = new STLGLProgramm(this, mvp, STLShader.Shader, FragmentShader.Shader);
        }
    }
}
