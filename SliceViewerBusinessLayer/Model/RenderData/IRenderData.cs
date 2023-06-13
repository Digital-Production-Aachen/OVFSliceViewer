using OpenTK.Graphics.OpenGL4;
using OVFSliceViewerCore.Model.RenderData;
using System;
using System.Collections.Generic;

namespace OVFSliceViewerCore.Model
{
    public interface IRenderData
    {
        int Start { get; }
        int End { get; }
        Vertex[] Vertices { get; }
        int SingleVertexSize { get; }

        Func<bool> UseColorIndex { get; }
        PrimitiveType PrimitiveType { get; }

        List<ColorIndexRange> ColorIndexRange { get; }
    }
}
