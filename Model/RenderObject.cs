using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LayerViewer.Model
{
    public class RenderObject : IRenderData
    {
        protected readonly AbstrShader _shader;
        public int Start { get; protected set; } = 0;
        public int End { get; protected set; }
        public OVFSliceViewer.Classes.Vertex[] Vertices { get; protected set; }

        public int SingleVertexSize { get; protected set; } = Marshal.SizeOf(typeof(OVFSliceViewer.Classes.Vertex));
        public PrimitiveType PrimitiveType { get; protected set; } = PrimitiveType.Lines;

        public RenderObject(IModelViewProjection mvp)
        {
            _shader = new Shader(this, mvp);
        }

        public void AddVertices(IList<OVFSliceViewer.Classes.Vertex> vertices)
        {
            var temp = Vertices.ToList();
            temp.AddRange(vertices);
            Vertices = temp.ToArray();

            _shader.BindNewData();
        }

        public void Reset()
        {
            End = 0;
        }
        public void Render()
        {
            _shader.Render();
        }
    }

    public interface IRenderData
    {
        int Start { get; }
        int End { get; }
        OVFSliceViewer.Classes.Vertex[] Vertices { get; }
        int SingleVertexSize { get; }
        PrimitiveType PrimitiveType { get; }
    }
}
