using OpenTK;

namespace OVFSliceViewerBusinessLayer.Model
{
    public interface IModelViewProjection
    {
        Matrix4 ModelViewProjection { get; }
        Vector3 CameraDirection { get; }
    }
}
