using OpenTK;

namespace OVFSliceViewerBusinessLayer.Model
{
    public interface IModelViewProjection
    {
        Matrix4 ModelViewProjection { get; }
    }
}
