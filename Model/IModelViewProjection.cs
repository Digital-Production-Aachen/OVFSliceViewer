using OpenTK;

namespace LayerViewer.Model
{
    public interface IModelViewProjection
    {
        Matrix4 ModelViewProjection { get; }
    }
}
