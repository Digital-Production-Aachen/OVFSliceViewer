using OpenTK;

namespace OVFSliceViewer
{
    public interface IRotateable
    {
        void Rotate(Vector2 delta);
    }
}