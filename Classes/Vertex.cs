using System.Runtime.InteropServices;

namespace OVFSliceViewer
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Vertex
    {
        //public Vertex(OpenTK.Vector3 _position, OpenTK.Vector4 _color)
        //{
        //    Position = _position;
        //    Color = _color;
        //}
        public OpenTK.Vector3 Position;
        public OpenTK.Vector4 Color;
    }
}
