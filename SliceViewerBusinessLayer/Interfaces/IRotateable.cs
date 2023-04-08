using OpenTK;
using OpenTK.Mathematics;

namespace OVFSliceViewerBusinessLayer
{
    public interface IRotateable
    {
        void Rotate(Vector2 delta);
    }
}