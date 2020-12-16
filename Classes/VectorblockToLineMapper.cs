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

        public VectorblockToLineMapper()
        {
            _vectorFactory = new VectorWithColorFactory(250f, 350f);
        }

        public void CalculateVectorBlock(VectorBlock vectorBlock, float height)
        {
            _vectorBlock = vectorBlock;
            _height = height;
            VectorBlockToViewModel();
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
                    Color = _lines[i].Start.Color,
                    Position = _lines[i].Start.Position
                };
                vertices[2*i+1 + gridcount] = new Vertex
                {
                    Color = _lines[i].Ende.Color,
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

        private void VectorBlockToViewModel()
        {
            List<VmLine> list = new List<VmLine>();
            var points = new RepeatedField<float>();
            _vectorFactory.SetColor(new Vector4(1f, 0f, 0f, 0f));

            var partareaColor = _colorDictionary.TryGetColor(_vectorBlock.LPbfMetadata.PartArea);
            var skintypeColor = _colorDictionary.TryGetColor(_vectorBlock.LPbfMetadata.SkinType);

            _vectorFactory.SetColor(partareaColor);
            //_vectorFactory.SetColor(skintypeColor);

            switch (_vectorBlock.VectorDataCase)
            {
                case VectorBlock.VectorDataOneofCase.LineSequence:
                    list = LineSequenceToViewModel();
                    break;
                case VectorBlock.VectorDataOneofCase.Hatches:
                    list = HatchesToViewModel();
                    break;
                case VectorBlock.VectorDataOneofCase.LineSequenceParaAdapt:
                    list = LineSequenceParaAdaptToViewModel(_vectorBlock.LineSequenceParaAdapt);
                    break;
                case VectorBlock.VectorDataOneofCase.HatchParaAdapt:
                    list = HatchesParaAdaptToViewModel();
                    break;
                default:
                    break;
            }

            _lines.AddRange(list);
        }

        private List<VmLine> LineSequenceToViewModel()
        {
            var points = _vectorBlock.LineSequence.Points;
            var list = new List<VmLine>();

            for (int i = 3; i <= points.Count(); i += 2)
            {
                var positionStart = new Vector3(points[i - 3], points[i - 2], _height);
                var positionEnd = new Vector3(points[i - 1], points[i], _height);

                var vmVectorStart = _vectorFactory.GetVectorWithColor(positionStart);
                var vmVectorEnde = _vectorFactory.GetVectorWithColor(positionEnd);
                var vmVector = new VmLine() { Start = vmVectorStart, Ende = vmVectorEnde, Height = _height };

                list.Add(vmVector);
            }
            return list;
        }
        private List<VmLine> LineSequenceParaAdaptToViewModel(LineSequenceParaAdapt lineSequenceParaAdapt)
        {
            var points = lineSequenceParaAdapt.PointsWithParas;
            var list = new List<VmLine>();

            for (int i = 5; i <= points.Count(); i += 3)
            {
                var positionStart = new Vector3(points[i - 5], points[i - 4], _height);
                var positionEnd = new Vector3(points[i - 2], points[i - 1], _height);

                var vmVectorStart = _vectorFactory.GetVectorWithPowerColor(positionStart, points[i - 3]);
                var vmVectorEnde = _vectorFactory.GetVectorWithPowerColor(positionEnd, points[i]);

                var vmVector = new VmLine() { Start = vmVectorStart, Ende = vmVectorEnde, Height = _height };
                list.Add(vmVector);
            }
            return list;
        }

        private List<VmLine> HatchesToViewModel()
        {
            var points = _vectorBlock.Hatches.Points;
            var list = new List<VmLine>();
            for (int i = 3; i <= points.Count; i += 4)
            {
                var positionStart = new Vector3(points[i - 3], points[i - 2], _height);
                var positionEnd = new Vector3(points[i - 1], points[i - 0], _height);

                var vmVectorStart = _vectorFactory.GetVectorWithColor(positionStart);
                var vmVectorEnde = _vectorFactory.GetVectorWithColor(positionEnd);

                var vmLine = new VmLine() { Start = vmVectorStart, Ende = vmVectorEnde, Height = _height };

                list.Add(vmLine);
            }
            return list;
        }

        private List<VmLine> HatchesParaAdaptToViewModel()
        {
            var list = new List<VmLine>();

            foreach (var item in _vectorBlock.HatchParaAdapt.HatchAsLinesequence)
            {
                list.AddRange(LineSequenceParaAdaptToViewModel(item));
            }

            return list;
        }


        private Vertex[] Grid()
        {
            int gridDistance = 10;
            int gridSize = 100;

            var numberOfLines = gridSize / gridDistance;
            ColorDictionary colorDictionary = new ColorDictionary();

            var grid = new Vertex[4 * numberOfLines + 8];

            for (int i = 0; i < 2 * numberOfLines + 2; i += 2)
            {
                grid[i] = new Vertex
                {
                    Color = new Vector4(0f, 84f / 255f, 159f / 255f, 0f),
                    Position = new Vector3((-gridSize / 2) + i / 2 * gridDistance, -gridSize / 2, 0)
                };
                grid[i + 1] = new Vertex
                {
                    Color = new Vector4(0f, 84f / 255f, 159f / 255f, 0f),
                    Position = new Vector3((-gridSize / 2) + i / 2 * gridDistance, gridSize / 2, 0)
                };

                grid[i + 2 * numberOfLines + 4] = new Vertex
                {
                    Color = new Vector4(0f, 84f / 255f, 159f / 255f, 0f),
                    Position = new Vector3((-gridSize / 2), (-gridSize / 2) + i / 2 * gridDistance, 0)
                };
                grid[i + 2 * numberOfLines + +1 + 4] = new Vertex
                {
                    Color = new Vector4(0f, 84f / 255f, 159f / 255f, 0f),
                    Position = new Vector3((gridSize / 2), (-gridSize / 2) + i / 2 * gridDistance, 0)
                };
            }

            return grid;

        }

        public void Dispose()
        {
            _lines = null;
        }
    }
}
