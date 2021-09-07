using Google.Protobuf.Collections;
using OpenTK;
using OpenVectorFormat;
using OVFSliceViewer.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static OpenVectorFormat.VectorBlock.Types;

namespace LayerViewer.Model
{
    public class OVFPart : AbstrPart
    {
        public readonly int Id;
        private List<OVFRenderObject> _ovfRenderObjects = new List<OVFRenderObject>();
        private Dictionary<int, OVFRenderObject> VectorBlockRenderObjectMap = new Dictionary<int, OVFRenderObject>();
        public OVFPart(int id, ISceneController scene)
        {
            Id = id;

            _ovfRenderObjects.Add(new OVFRenderObject(scene.Camera, OVFRenderObject.ViewerPartArea.Contour, new Vector4(0, 1, 0, 1)));
            _ovfRenderObjects.Add(new OVFRenderObject(scene.Camera, OVFRenderObject.ViewerPartArea.Volume, new Vector4(1, 0, 0, 1)));
            _ovfRenderObjects.Add(new OVFRenderObject(scene.Camera, OVFRenderObject.ViewerPartArea.SupportContour, new Vector4(1, 0, 0, 1)));
            _ovfRenderObjects.Add(new OVFRenderObject(scene.Camera, OVFRenderObject.ViewerPartArea.SupportVolume, new Vector4(1, 0, 0, 1)));

            RenderObjects.AddRange(_ovfRenderObjects);
        }

        public void Render()
        {
            RenderObjects.ForEach(x => x.Render());
        }
        public void Reset()
        {
            _addedVectorblocks = 0;
            VectorBlockRenderObjectMap.Clear();
            RenderObjects.ForEach(x => x.Reset());
        }
        public void AddWorkplane(WorkPlane workplane)
        {
            Reset();
            var vectorblocks = workplane.VectorBlocks.Where(x => x.MetaData.PartKey == Id).ToList();

            vectorblocks.ForEach(x => AddVectorblock(x, workplane.ZPosInMm));

            BindData();
        }
        public void BindData()
        {
            _ovfRenderObjects.ForEach(x => x.BindNewData());
        }

        private int _addedVectorblocks = 0;
        public int AddVectorblock(VectorBlock vectorBlock, float height)
        {
            var vertices = GetVerticesFromVectorblock(vectorBlock, height);
            BoundingBox.AddVertices(vertices);

            OVFRenderObject contourTarget;
            OVFRenderObject volumeTarget;

            if (vectorBlock.LpbfMetadata.StructureType != StructureType.Support)
            {
                contourTarget = _ovfRenderObjects.First(x => x.Type == OVFRenderObject.ViewerPartArea.Contour);
                volumeTarget = _ovfRenderObjects.First(x => x.Type == OVFRenderObject.ViewerPartArea.Volume);
            }
            else
            {
                contourTarget = _ovfRenderObjects.First(x => x.Type == OVFRenderObject.ViewerPartArea.SupportContour);
                volumeTarget = _ovfRenderObjects.First(x => x.Type == OVFRenderObject.ViewerPartArea.SupportVolume);
            }

            OVFRenderObject target = contourTarget;
            switch (vectorBlock.LpbfMetadata.PartArea)
            {
                case VectorBlock.Types.PartArea.Volume:
                    target = volumeTarget;
                    break;
                case VectorBlock.Types.PartArea.Contour:
                    target = contourTarget;
                    break;
                case VectorBlock.Types.PartArea.TransitionContour:
                    target = contourTarget;
                    break;
                default:
                    break;
            }

            target.AddVertices(vertices);
            VectorBlockRenderObjectMap.Add(_addedVectorblocks, target);
            _addedVectorblocks++;

            return vertices.Count / 2;
        }

        #region ModelToViewmodel
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
                var startVertex = new Vertex(new Vector3(points[i - 3], points[i - 2], height), 2);
                var endVertex = new Vertex(new Vector3(points[i - 1], points[i], height), 2);

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
                var startVertex = new Vertex(new Vector3(points[i - 3], points[i - 2], height), 2);
                var endVertex = new Vertex(new Vector3(points[i - 1], points[i - 0], height), 2);

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

        #endregion

        private int _vectorblockNumber = 0;
        public void IncreaseNumberOfLinesToDraw(int numberOfLines)
        {
            var renderObject = VectorBlockRenderObjectMap[_vectorblockNumber];
            renderObject.End = Math.Min(renderObject.End + numberOfLines, renderObject.Vertices.Length);
            _vectorblockNumber++;
        }

        public void ResetNumberOfLinesToDraw()
        {
            _ovfRenderObjects.ForEach(x => x.End = 0);
            _vectorblockNumber = 0;
        }
    }

    public static class VectorBlockExtensions
    {
        // ToDo: Move LinesequenceToViewModel etc. here
    }
}
