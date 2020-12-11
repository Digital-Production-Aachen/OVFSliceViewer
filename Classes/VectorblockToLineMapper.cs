using Google.Protobuf.Collections;
using LayerViewer.Classes;
using OpenTK;
using OpenVectorFormat;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenVectorFormat.VectorBlock.Types;

namespace LayerViewer
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

        public List<Vertex> GetVertices()
        {
            var list = new List<Vertex>();

            foreach (var item in _lines)
            {
                list.Add(new Vertex
                {
                    Color = item.Start.Color,
                    Position = item.Start.Position
                });

                list.Add(new Vertex
                {
                    Color = item.Ende.Color,
                    Position = item.Ende.Position
                });
            }

            return list;
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

            for (int i = 3; i < points.Count(); i += 2)
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

            for (int i = 5; i < points.Count(); i += 3)
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
            for (int i = 3; i < points.Count; i += 4)
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

    }
}
