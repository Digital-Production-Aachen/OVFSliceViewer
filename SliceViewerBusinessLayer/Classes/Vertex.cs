using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace OVFSliceViewerCore.Classes
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Vertex
    {
        public Vertex(Vector3 position, float colorIndex = 0)
        {
            ColorIndex = colorIndex;
            Position = position;
        }
        public float ColorIndex; //4
        public Vector3 Position; //12
    }
}
