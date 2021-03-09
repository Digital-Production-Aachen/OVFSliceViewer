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
            //VectorBlockToViewModel();
        }

        public Vertex[] GetVertices()
        {
            //var grid = Grid().ToList();
            //var gridcount = grid.Count();
            var gridcount = 0;
            var linecount = _lines.Count();
            var vertices = new Vertex[gridcount + linecount*2];
            //new List<Vertex>();

            //for (int i = 0; i < gridcount; i++)
            //{
            //    vertices[i] = grid[i];
            //}
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

            //foreach (var item in _lines)
            //{
            //    list.Add(new Vertex
            //    {
            //        Color = item.Start.Color,
            //        Position = item.Start.Position
            //    });

            //    list.Add(new Vertex
            //    {
            //        Color = item.Ende.Color,
            //        Position = item.Ende.Position
            //    });
            //}

            return vertices;
        }


        public List<VmLine> GetViewModel()
        {
            return _lines;
        }

        //private void VectorBlockToViewModel()
        //{
        //    List<VmLine> list = new List<VmLine>();
        //    var points = new RepeatedField<float>();
        //    var color = new Vector4(1f, 0f, 0f, 0f);
        //    switch(HightlightIndex)
        //    {
        //        case 0: break;//0 = nothing
        //        case 1: if (_vectorBlock.LpbfMetadata.PartArea == PartArea.Contour)  color = new Vector4(87f / 255f, 171f / 255f, 39f / 255f, 0f); //1 = Contour 
        //            break;
        //        case 2: if (_vectorBlock.LpbfMetadata.StructureType == StructureType.Support)  color = new Vector4(87f / 255f, 171f / 255f, 39f / 255f, 0f); //2 = Support
        //            break;
        //        default: break;
        //    }

        //    _vectorFactory.SetColor(color);
        //    switch (_vectorBlock.VectorDataCase)
        //    {
        //        case VectorBlock.VectorDataOneofCase.LineSequence:
        //            list = LineSequenceToViewModel();
        //            break;
        //        case VectorBlock.VectorDataOneofCase.Hatches:
        //            list = HatchesToViewModel();
        //            break;
        //        case VectorBlock.VectorDataOneofCase.LineSequenceParaAdapt:
        //            list = LineSequenceParaAdaptToViewModel(_vectorBlock.LineSequenceParaAdapt);
        //            break;
        //        case VectorBlock.VectorDataOneofCase.HatchParaAdapt:
        //            list = HatchesParaAdaptToViewModel();
        //            break;
        //        default:
        //            break;
        //    }
        //    _lines.AddRange(list);
        //}

        

        public void Dispose()
        {
            _lines = null;
            _vectorBlock = null;
        }
    }
}
