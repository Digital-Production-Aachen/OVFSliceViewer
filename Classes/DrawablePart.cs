using OVFSliceViewer.Classes.ShaderNamespace;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenVectorFormat;
using Google.Protobuf.Collections;
using static OpenVectorFormat.VectorBlock.Types;

namespace OVFSliceViewer.Classes
{
    public class DrawablePart
    {
        public int Partnumber { get; protected set; }
        protected DrawableObject _contour;
        protected DrawableObject _volume;
        protected VectorBlock _vectorBlock;
        int _numberOfLayers;
        VectorWithColorFactory _vectorFactory = new VectorWithColorFactory(250, 350);
        float _height;
        List<VmLine> _contourLines = new List<VmLine>();
        List<VmLine> _volumeLines = new List<VmLine>();
        int[] numberOfContourLinesInWorkplane;
        public int HightlightIndex { get; set; } = 0;

        public DrawablePart(Shader shader, int contourBuffer, int volumeBuffer, int numberOfLayers, int partnumber)
        {
            _contour = new DrawableObject(shader, contourBuffer);
            _volume = new DrawableObject(shader, volumeBuffer);
            _numberOfLayers = numberOfLayers;
            numberOfContourLinesInWorkplane = new int[numberOfLayers];
            Partnumber = partnumber;
        }

        public void SetContourRangeToDraw2d(int numberOfPoints, int layer)
        {
            var startcount = 0;

            for (int i = 0; i < layer; i++)
            {
                startcount += numberOfContourLinesInWorkplane[i];
            }

            
            _contour.SetRangeToDraw(startcount*2 + numberOfPoints*2, startcount * 2);
        }
        public void SetContourRangeToDraw3d(int endlayer, int startlayer = 0)
        {
            var endcount = 0;
            var startcount = 0;

            for (int i = 0; i <= startlayer; i++)
            {
                startcount += numberOfContourLinesInWorkplane[i];
            }

            for(int i = startlayer; i <= endlayer; i++)
            {
                endcount += numberOfContourLinesInWorkplane[i];
            }
            _contour.SetRangeToDraw(endcount*2, startcount*2);
        }
        public void SetVolumeRangeToDraw(int end)
        {
            
            _volume.SetRangeToDraw(end);
        }

        public void UpdateContour()
        {
            var vertices = new List<Vertex>();

            foreach (var item in _contourLines)
            {
                var startVertex = new Vertex { Color = item.Start.Color, Position = item.Start.Position };
                var endVertex = new Vertex { Color = item.Ende.Color, Position = item.Ende.Position };

                vertices.Add(startVertex);
                vertices.Add(endVertex);
            }

            _contour.ChangePicture(vertices.ToArray());
            //_contourLines.RemoveAll(x => true);
            _contourLines = new List<VmLine>();
        }

        public void UpdateVolume()
        {
            var vertices = new List<Vertex>();

            foreach (var item in _volumeLines)
            {
                var startVertex = new Vertex { Color = item.Start.Color, Position = item.Start.Position };
                var endVertex = new Vertex { Color = item.Ende.Color, Position = item.Ende.Position };

                vertices.Add(startVertex);
                vertices.Add(endVertex);
            }

            _volume.ChangePicture(vertices.ToArray());
        }

        public void DrawAll(Matrix4 modelViewProjection)
        {
            //UpdateContour();
            _contour.Draw(modelViewProjection);
            _volume.Draw(modelViewProjection);
        }
        public void AddContour(VectorBlock vectorBlock, int layernumber, float height)
        {
            _height = height;
            _vectorBlock = vectorBlock;
            var lines = VectorBlockToViewModel();

            if (_vectorBlock.LpbfMetadata.PartArea == PartArea.Contour)
            {
                _contourLines.AddRange(lines);
            }
            numberOfContourLinesInWorkplane[layernumber] += lines.Count;
        }
        public void AddVectorBlock(VectorBlock vectorBlock, int layernumber, float height)
        {
            _height = height;
            _vectorBlock = vectorBlock;
            var lines = VectorBlockToViewModel();

            if (_vectorBlock.LpbfMetadata.PartArea == PartArea.Contour)
            {
                //_contourLines.AddRange(lines);
            }
            else
            {
                _volumeLines.AddRange(lines);
            }
        }
        public void RemoveVolume()
        {
            _volumeLines = new List<VmLine>();
            UpdateVolume();
        }
        private List<VmLine> VectorBlockToViewModel()
        {
            List<VmLine> list = new List<VmLine>();
            var points = new RepeatedField<float>();
            var color = new Vector4(1f, 0f, 0f, 0f);
            switch (HightlightIndex)
            {
                case 0: break;//0 = nothing
                case 1:
                    if (_vectorBlock.LpbfMetadata.PartArea == PartArea.Contour) color = new Vector4(87f / 255f, 171f / 255f, 39f / 255f, 0f); //1 = Contour 
                    break;
                case 2:
                    if (_vectorBlock.LpbfMetadata.StructureType == StructureType.Support) color = new Vector4(87f / 255f, 171f / 255f, 39f / 255f, 0f); //2 = Support
                    break;
                default: break;
            }

            _vectorFactory.SetColor(color);
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
            return list;
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
    }
}
