using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace OVFSliceViewerCore.Model
{
    public interface IRenderData
    {
        int Start { get; }
        int End { get; }
        OVFSliceViewerCore.Classes.Vertex[] Vertices { get; }
        int SingleVertexSize { get; }

        Func<bool> UseColorIndex { get; }
        PrimitiveType PrimitiveType { get; }

        List<ColorIndexRange> ColorIndexRange { get; }
    }
}
