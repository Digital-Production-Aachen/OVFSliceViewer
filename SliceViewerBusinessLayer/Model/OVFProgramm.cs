using OpenTK.Graphics.OpenGL4;
//using OpenTK.Mathematics;
using System.Diagnostics;

namespace OVFSliceViewerCore.Model
{
    public class OVFProgramm: GLProgramm
    {
        public OVFProgramm(IRenderData renderObject, IModelViewProjection mvp,string vertexPath,string fragmentPath) : base(renderObject, mvp, vertexPath, fragmentPath){}
    }



}
