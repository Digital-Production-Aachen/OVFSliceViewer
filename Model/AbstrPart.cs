using System;
using System.Collections.Generic;
using System.Linq;

namespace LayerViewer.Model
{
    public class AbstrPart: IDisposable
    {
        public AbstrPart()
        {
            BoundingBox = new BoundingBox();
            RenderObjects = new List<RenderObject>();
        }
        public BoundingBox BoundingBox { get; protected set; }
        public bool IsActive = true;
        private bool disposedValue;

        public virtual List<RenderObject> RenderObjects { get; protected set; }

        public virtual void AddVertices(IList<OVFSliceViewer.Classes.Vertex> vertices)
        {
            BoundingBox.AddVertices(vertices);
            RenderObjects.FirstOrDefault().AddVertices(vertices);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }
                foreach (var renderObject in RenderObjects)
                {
                    renderObject.Dispose();
                }
                RenderObjects = null;
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~AbstrPart()
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
