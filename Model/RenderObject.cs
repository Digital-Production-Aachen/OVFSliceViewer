using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LayerViewer.Model
{
    public class RenderObject
    {
        protected readonly AbstrShader _shader;
        public int Start { get; protected set; } = 0;
        public int End { get; protected set; }
        public OVFSliceViewer.Classes.Vertex[] Vertices { get; protected set; }

        public int SingleVertexSize = Marshal.SizeOf(typeof(OVFSliceViewer.Classes.Vertex));
        public readonly PrimitiveType PrimitiveType = PrimitiveType.Lines;

        public RenderObject(Func<RenderObject, AbstrShader> creator)
        {
            _shader = creator(this);
        }

        public void SetVertices(IList<OVFSliceViewer.Classes.Vertex> vertices)
        {
            Vertices = vertices.ToArray();
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
}
