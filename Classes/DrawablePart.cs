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
        public VectorWithColorFactory VectorFactory { get; protected set; } = new VectorWithColorFactory(250, 350);
        float _height;

        List<VmLine> _contourLines = new List<VmLine>();
        List<VmLine> _volumeLines = new List<VmLine>();
        public int[] NumberOfContourLinesInWorkplane { get; protected set; }
        public int HightlightIndex { get; set; } = 0;

        public DrawablePart(Shader shader, int numberOfLayers, int partnumber)
        {
            _contour = new DrawableObject(shader);
            _volume = new DrawableObject(shader);
            _numberOfLayers = numberOfLayers;
            NumberOfContourLinesInWorkplane = new int[numberOfLayers];
            Partnumber = partnumber;
        }

        public void SetContourRangeToDraw2d(int numberOfPoints, int layer)
        {
            var startcount = 0;

            for (int i = 0; i < layer; i++)
            {
                startcount += NumberOfContourLinesInWorkplane[i];
            }

            
            _contour.SetRangeToDraw(startcount*2 + numberOfPoints*2, startcount * 2);
        }
        public void SetContourRangeToDraw3d(int endlayer, int linesInLayer = 0)
        {
            var endcount = 0;
            var startcount = 0;

            for(int i = 0; i < endlayer; i++)
            {
                endcount += NumberOfContourLinesInWorkplane[i];
            }
            _contour.SetRangeToDraw(endcount*2 + linesInLayer*2, 0);
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
                var startVertex = new Vertex (item.Start.Position, item.Start.Color);
                var endVertex = new Vertex (item.Ende.Position, item.Ende.Color);

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
            //toDo: consolidate
            foreach (var item in _volumeLines)
            {
                var startVertex = new Vertex(item.Start.Position, item.Start.Color);
                var endVertex = new Vertex(item.Ende.Position, item.Ende.Color);

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

            if (_vectorBlock.LpbfMetadata == null || _vectorBlock.LpbfMetadata.PartArea == PartArea.Contour)
            {
                _contourLines.AddRange(lines);
            }
            NumberOfContourLinesInWorkplane[layernumber] += lines.Count;
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
            var color = 0;
            //new Vector4(87f / 255f, 171f / 255f, 39f / 255f, 0f)
            if (_vectorBlock.LpbfMetadata == null || _vectorBlock.LpbfMetadata.PartArea == PartArea.Contour)
            {
                color = 2;
            }
            if (_vectorBlock.LpbfMetadata != null && _vectorBlock.LpbfMetadata.StructureType == StructureType.Support)
            {
                color = 3;
            }


            VectorFactory.SetColor(color);
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

                var vmVectorStart = VectorFactory.GetVectorWithColor(positionStart);
                var vmVectorEnde = VectorFactory.GetVectorWithColor(positionEnd);
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

                var vmVectorStart = VectorFactory.GetVectorWithPowerColor(positionStart, points[i - 3]);
                var vmVectorEnde = VectorFactory.GetVectorWithPowerColor(positionEnd, points[i]);

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

                var vmVectorStart = VectorFactory.GetVectorWithColor(positionStart);
                var vmVectorEnde = VectorFactory.GetVectorWithColor(positionEnd);

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
