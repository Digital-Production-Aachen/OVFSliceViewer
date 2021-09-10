using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace LayerViewer.Model
{
    public interface IRenderData
    {
        int Start { get; }
        int End { get; }
        OVFSliceViewer.Classes.Vertex[] Vertices { get; }
        int SingleVertexSize { get; }

        bool UseColorIndex { get; }
        PrimitiveType PrimitiveType { get; }

        List<ColorIndexRange> ColorIndexRange { get; }
    }
}
