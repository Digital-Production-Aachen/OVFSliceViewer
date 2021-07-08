using OpenVectorFormat;
using OVFSliceViewer.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LayerViewer.Model
{
    public class OVFPart : AbstrPart
    {
        public readonly int Id;

        RenderObject _contour;
        //RenderObject _support;
        RenderObject _volume;
        public OVFPart(int id)
        {
            Id = id;
            Func<RenderObject, Shader> creator = (RenderObject r) => new Shader(r);

            _contour = new RenderObject(creator);
            _volume = new RenderObject(creator);

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

            vectorblocks.ForEach(x => AddVectorblock(x));
        }
        public void AddVectorblock(VectorBlock vectorBlock)
        {
            switch (vectorBlock.LpbfMetadata.PartArea)
            {
                case VectorBlock.Types.PartArea.Volume:

                    break;
                case VectorBlock.Types.PartArea.Contour:
                    break;
                case VectorBlock.Types.PartArea.TransitionContour:
                    break;
                default:
                    break;
            }
        }

        private List<Vertex> GetVerticesFromVectorblock(VectorBlock vectorBlock)
        {
            var list = new List<Vertex>();

            return list;
        }
    }
}
