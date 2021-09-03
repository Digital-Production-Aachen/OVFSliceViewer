using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LayerViewer.Model
{
    public class RenderObject : IRenderData, IDisposable
    {
        protected AbstrShader _shader;
        public int Start { get; protected set; } = 0;
        public int End { get; protected set; }
        public OVFSliceViewer.Classes.Vertex[] Vertices { get; protected set; } = new OVFSliceViewer.Classes.Vertex[0];
        public BoundingBox BoundingBox { get; protected set; }
        public int SingleVertexSize { get; protected set; } = Marshal.SizeOf(typeof(OVFSliceViewer.Classes.Vertex));
        public virtual PrimitiveType PrimitiveType { get; /*protected*/ set; } = PrimitiveType.Lines;
        IModelViewProjection _mvp;
        private bool disposedValue;

        public RenderObject(IModelViewProjection mvp)
        {
            _mvp = mvp;
            CreateShader(mvp);
            BoundingBox = new BoundingBox();
        }

        protected virtual void CreateShader(IModelViewProjection mvp)
        {
            _shader = new Shader(this, mvp);
        }

        public void AddVertices(IList<OVFSliceViewer.Classes.Vertex> vertices)
        {
            BoundingBox.AddVertices(vertices);
            var temp = Vertices.ToList();
            temp.AddRange(vertices);
            Vertices = temp.ToArray();

            End = Vertices.Length;
        }
        public void BindNewData() => _shader.BindNewData();

        public void Reset()
        {
            Vertices = new OVFSliceViewer.Classes.Vertex[0];
            End = 0;
        }
        public void Render()
        {
            var test2 = _mvp.ModelViewProjection;
            var test = Vertices.Select(x => test2 * new Vector4(x.Position, 0)).ToList();
            //_shader.BindNewData();
            _shader.Render();
        }

        public void ChangeColor()
        {

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }
                Vertices = null;
                _shader.Dispose();
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~RenderObject()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
