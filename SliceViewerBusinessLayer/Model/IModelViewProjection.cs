using OpenTK;
using OpenTK.Mathematics;

namespace OVFSliceViewerBusinessLayer.Model
{
    public interface IModelViewProjection
    {
        Matrix4 ModelViewProjection { get; }
        Vector3 CameraDirection { get; }
    }
}
