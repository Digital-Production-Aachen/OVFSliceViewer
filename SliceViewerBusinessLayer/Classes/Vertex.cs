using System.Runtime.InteropServices;

namespace OVFSliceViewerBusinessLayer.Classes
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Vertex
    {
        public Vertex(OpenTK.Vector3 position, float colorIndex = 0)
        {
            ColorIndex = colorIndex;
            Position = position;
        }
        public float ColorIndex; //4
        public OpenTK.Vector3 Position; //12
    }
}
