﻿using Google.Protobuf.Collections;
using OpenTK;
using OpenVectorFormat;
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
        //public string Name { get; protected set; }
        public OVFPart(int id, string name, ISceneController scene)
        {
            Id = id;
            Name = name;

            _ovfRenderObjects.Add(new OVFRenderObject(scene.Camera, OVFRenderObject.ViewerPartArea.Contour, new Vector4(0.8f, 0.02745f, 0.117647f, 1)));
            _ovfRenderObjects.Add(new OVFRenderObject(scene.Camera, OVFRenderObject.ViewerPartArea.Volume, new Vector4(0.8f, 0.02745f, 0.117647f, 1)));
            _ovfRenderObjects.Add(new OVFRenderObject(scene.Camera, OVFRenderObject.ViewerPartArea.SupportContour, new Vector4(0.8f, 0.02745f, 0.117647f, 1)));
            _ovfRenderObjects.Add(new OVFRenderObject(scene.Camera, OVFRenderObject.ViewerPartArea.SupportVolume, new Vector4(0.8f, 0.02745f, 0.117647f, 1)));

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
            var vertices = vectorBlock.GetVerticesFromVectorblock(height);
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

            target.AddVertices(vertices, vectorBlock.LaserIndex);
            VectorBlockRenderObjectMap.Add(_addedVectorblocks, target);
            _addedVectorblocks++;

            return vertices.Count / 2;
        }

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
}
