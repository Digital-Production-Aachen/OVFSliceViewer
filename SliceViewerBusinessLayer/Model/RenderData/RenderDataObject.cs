using OpenTK.Graphics.OpenGL4;
using OVFSliceViewerBusinessLayer.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace OVFSliceViewerBusinessLayer.Model
{
    public class RenderDataObject : IRenderData, IDisposable
    {
        protected AbstrShader _shader;
        public int Start { get; protected set; } = 0;
        public int End { get; set; } = 0;
        public Vertex[] Vertices { get; protected set; } = new Vertex[0];
        public BoundingBox BoundingBox { get; protected set; }
        public int SingleVertexSize { get; protected set; } = Marshal.SizeOf(typeof(Vertex));
        public virtual PrimitiveType PrimitiveType { get; set; } = PrimitiveType.Lines;
        IModelViewProjection _mvp;
        public Func<bool> UseColorIndex { get; set; } = () => true;
        public List<ColorIndexRange> ColorIndexRange { get; protected set; } = new List<ColorIndexRange>();

        public RenderDataObject(Func<bool> useColorIndex, IModelViewProjection mvp)
        {
            UseColorIndex = useColorIndex;
            _mvp = mvp;
            CreateShader(mvp);
            BoundingBox = new BoundingBox();
        }

        protected virtual void CreateShader(IModelViewProjection mvp)
        {
            _shader = new Shader(this, mvp);
        }

        public void AddVertices(IList<Vertex> vertices, int colorIndex)
        {
            if (vertices.Count == 0) return;

            BoundingBox.AddVertices(vertices);
            var temp = Vertices.ToList();
            ColorIndexRange colorIndexRange = new ColorIndexRange()
            {
                Range = new Range { Start = Vertices.Length, End = Vertices.Length + vertices.Count },
                ColorIndex = colorIndex
            };
            if (ColorIndexRange.Count > 0)
            {
                var lastColorIndexRange = ColorIndexRange[ColorIndexRange.Count - 1];
                if (lastColorIndexRange.ColorIndex == colorIndexRange.ColorIndex && lastColorIndexRange.Range.End == colorIndexRange.Range.Start)
                {
                    colorIndexRange.Range.Start = lastColorIndexRange.Range.Start;
                    ColorIndexRange[ColorIndexRange.Count - 1] = colorIndexRange;
                }
            }
            else
            {
                ColorIndexRange.Add(colorIndexRange);
            }
            temp.AddRange(vertices);
            Vertices = temp.ToArray();

            End = Vertices.Length;
        }
        public void BindNewData() => _shader.BindNewData();

        public void Reset()
        {
            Vertices = new Vertex[0];
            End = 0;
            Start = 0;
            ColorIndexRange.Clear();
        }
        public void Render()
        {
            if (Start < End)
            {
                _shader.Render();
            }
        }

        public void ChangeColor()
        {

        }


        private bool disposedValue;
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
