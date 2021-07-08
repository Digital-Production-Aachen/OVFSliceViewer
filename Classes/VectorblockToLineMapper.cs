using Google.Protobuf.Collections;
using OVFSliceViewer.Classes;
using OpenTK;
using OpenVectorFormat;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenVectorFormat.VectorBlock.Types;
using OVFSliceViewer.Classes;

namespace OVFSliceViewer
{
    public class VectorblockToLineMapper
    {
        VectorBlock _vectorBlock;
        float _height;
        List<VmLine> _lines = new List<VmLine>();
        VectorWithColorFactory _vectorFactory;
        float _power = 350;
        ColorDictionary _colorDictionary = new ColorDictionary();
        public int HightlightIndex { get; set; } = 0;

        public VectorblockToLineMapper()
        {
            _vectorFactory = new VectorWithColorFactory(250f, 350f);
        }

        public void CalculateVectorBlock(VectorBlock vectorBlock, float height)
        {
            _vectorBlock = null;
            _vectorBlock = vectorBlock;
            _height = height;
        }

        public Vertex[] GetVertices()
        {
            var gridcount = 0;
            var linecount = _lines.Count();
            var vertices = new Vertex[gridcount + linecount*2];

            for (int i = 0; i < linecount; i++)
            {
                vertices[2*i+gridcount] = new Vertex
                {
                    ColorIndex = _lines[i].Start.Color,
                    Position = _lines[i].Start.Position
                };
                vertices[2*i+1 + gridcount] = new Vertex
                {
                    ColorIndex = _lines[i].Ende.Color,
                    Position = _lines[i].Ende.Position
                };
            }

            return vertices;
        }


        public List<VmLine> GetViewModel()
        {
            return _lines;
        }     
        public void Dispose()
        {
            _lines = null;
            _vectorBlock = null;
        }
    }
}
