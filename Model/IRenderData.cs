using OpenTK.Graphics.OpenGL4;

namespace LayerViewer.Model
{
    public interface IRenderData
    {
        int Start { get; }
        int End { get; }
        OVFSliceViewer.Classes.Vertex[] Vertices { get; }
        int SingleVertexSize { get; }
        PrimitiveType PrimitiveType { get; }
    }
}
