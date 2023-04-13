using OpenTK;
using OpenTK.Mathematics;

namespace OVFSliceViewerCore
{
    public interface IRotateable
    {
        void Rotate(Vector2 delta);
    }
}