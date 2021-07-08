using Google.Protobuf.Collections;
using OpenTK;
using OpenVectorFormat;
using OVFSliceViewer.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using static OpenVectorFormat.VectorBlock.Types;

namespace LayerViewer.Model
{
    public class OVFPart : AbstrPart
    {
        public readonly int Id;

        RenderObject _contour;
        //RenderObject _support;
        RenderObject _volume;
        public OVFPart(int id, IScene scene)
        {
            Id = id;

            _contour = new RenderObject(scene.Camera);
            _volume = new RenderObject(scene.Camera);

            RenderObjects.Add(_contour);
            RenderObjects.Add(_volume);
        }

        public void Render()
        {
            RenderObjects.ForEach(x => x.Render());
        }
        public void Reset()
        {
            RenderObjects.ForEach(x => x.Reset());
        }
        public void AddWorkplane(WorkPlane workplane)
        {
            var vectorblocks = workplane.VectorBlocks.Where(x => x.MetaData.PartKey == Id).ToList();

            vectorblocks.ForEach(x => AddVectorblock(x, workplane.ZPosInMm));
        }
        public void AddVectorblock(VectorBlock vectorBlock, float height)
        {
            var vertices = GetVerticesFromVectorblock(vectorBlock, height);

            switch (vectorBlock.LpbfMetadata.PartArea)
            {
                case VectorBlock.Types.PartArea.Volume:
                    _volume.AddVertices(vertices);
                    break;
                case VectorBlock.Types.PartArea.Contour:
                    _contour.AddVertices(vertices);
                    break;
                case VectorBlock.Types.PartArea.TransitionContour:
                    _contour.AddVertices(vertices);
                    break;
                default:
                    break;
            }
        }
        private List<Vertex> GetVerticesFromVectorblock(VectorBlock vectorblock, float height)
        {
            List<Vertex> list = new List<Vertex>();
            var points = new RepeatedField<float>();
            var color = 0;
            //new Vector4(87f / 255f, 171f / 255f, 39f / 255f, 0f)

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
        private List<Vertex> LineSequenceToViewModel(VectorBlock vectorblock, float height)
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
        private List<Vertex> LineSequenceParaAdaptToViewModel(LineSequenceParaAdapt lineSequenceParaAdapt, float height)
        {
            var points = lineSequenceParaAdapt.PointsWithParas;
            var vertices = new List<Vertex>();

            for (int i = 5; i <= points.Count(); i += 3)
            {
                var startVertex = new Vertex(new Vector3(points[i - 5], points[i - 4], height), points[i - 3]);
                var endVertex = new Vertex(new Vector3(points[i - 2], points[i - 1], height), points[i]);

                vertices.Add(startVertex);
                vertices.Add(endVertex);
            }
            return vertices;
        }

        private List<Vertex> HatchesToViewModel(VectorBlock vectorblock, float height)
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

        private List<Vertex> HatchesParaAdaptToViewModel(VectorBlock vectorblock, float height)
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
