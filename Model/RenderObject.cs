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
        protected readonly Shader _shader;
        public OVFSliceViewer.Classes.Vertex[] Vertices { get; protected set; }

        public int SingleVertexSize = Marshal.SizeOf(typeof(OVFSliceViewer.Classes.Vertex));
        public readonly PrimitiveType PrimitiveType = PrimitiveType.Lines;

        public RenderObject(Shader shader)
        {
            _shader = shader;
        }

        public void SetVertices(IList<OVFSliceViewer.Classes.Vertex> vertices)
        {
            Vertices = vertices.ToArray();
            _shader.BindNewData();
        }

        public void Render()
        {
            _shader.Render(0, Vertices.Length);
        }
    }
}
