using OpenTK;
using OpenTK.Mathematics;
using OpenVectorFormat;
using OVFSliceViewerCore.Classes;
using System.Collections.Generic;
using System.Linq;
using static OpenVectorFormat.VectorBlock.Types;

namespace OVFSliceViewerCore.Model
{
    public static class VectorBlockExtensions
    {
        public static List<Vertex> GetVerticesFromVectorblock(this VectorBlock vectorblock, float height)
        {
            List<Vertex> list = new List<Vertex>();

            switch (vectorblock.VectorDataCase)
            {
                case VectorBlock.VectorDataOneofCase.LineSequence:
                    list = LineSequenceToViewModel(vectorblock, height);
                    break;
                case VectorBlock.VectorDataOneofCase.Hatches:
                    list = HatchesToViewModel(vectorblock, height);
                    break;
                case VectorBlock.VectorDataOneofCase.LineSequenceParaAdapt:
                    list = LineSequenceParaAdaptToViewModel(vectorblock.LineSequenceParaAdapt, height);
                    break;
                case VectorBlock.VectorDataOneofCase.HatchParaAdapt:
                    list = HatchesParaAdaptToViewModel(vectorblock, height);
                    break;
                default:
                    break;
            }
            return list;
        }
        private static List<Vertex> LineSequenceToViewModel(VectorBlock vectorblock, float height)
        {
            var points = vectorblock.LineSequence.Points;
            var vertices = new List<Vertex>();

            for (int i = 3; i <= points.Count(); i += 2)
            {
                var startVertex = new Vertex(new Vector3(points[i - 3], points[i - 2], height), 0);
                var endVertex = new Vertex(new Vector3(points[i - 1], points[i], height), 0);

                vertices.Add(startVertex);
                vertices.Add(endVertex);
            }
            return vertices;
        }
        private static List<Vertex> LineSequenceParaAdaptToViewModel(LineSequenceParaAdapt lineSequenceParaAdapt, float height)
        {
            var points = lineSequenceParaAdapt.PointsWithParas;
            var vertices = new List<Vertex>();

            for (int i = 5; i <= points.Count(); i += 3)
            {
                var startVertex = new Vertex(new Vector3(points[i - 5], points[i - 4], height), ((points[i - 3] - 250f) / 100f));
                var endVertex = new Vertex(new Vector3(points[i - 2], points[i - 1], height), ((points[i] - 250f) / 100f));

                vertices.Add(startVertex);
                vertices.Add(endVertex);
            }
            return vertices;
        }

        private static List<Vertex> HatchesToViewModel(VectorBlock vectorblock, float height)
        {
            var points = vectorblock.Hatches.Points;
            var vertices = new List<Vertex>();
            for (int i = 3; i <= points.Count; i += 4)
            {
                var startVertex = new Vertex(new Vector3(points[i - 3], points[i - 2], height), 0);
                var endVertex = new Vertex(new Vector3(points[i - 1], points[i - 0], height), 0);

                vertices.Add(startVertex);
                vertices.Add(endVertex);
            }
            return vertices;
        }

        private static List<Vertex> HatchesParaAdaptToViewModel(VectorBlock vectorblock, float height)
        {
            var list = new List<Vertex>();

            foreach (var item in vectorblock.HatchParaAdapt.HatchAsLinesequence)
            {
                list.AddRange(LineSequenceParaAdaptToViewModel(item, height));
            }

            return list;
        }
    }
}
