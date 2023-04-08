using Google.Protobuf.Collections;
using OpenVectorFormat;
using System.Collections.Generic;
using System.Linq;

namespace LayerViewer.Model
{
    public class OVFFileInfo
    {
        public Dictionary<int, string> PartNamesMap = new Dictionary<int, string>();
        public List<int> PartKeys;
        public int NumberOfWorkplanes;
        public int NumberOfVectorblocks => VectorblockDisplayData.Count;

        public List<VectorblockDisplayData> VectorblockDisplayData;
        public List<int> NumberOfVerticesInWorkplane;

        public OVFFileInfo(MapField<int, Part> partsMap, int numberOfWorkplanes, List<VectorblockDisplayData> vectorblockDisplayData)
        {
            PartKeys = partsMap.Keys.ToList();
            NumberOfWorkplanes = numberOfWorkplanes;
            VectorblockDisplayData = vectorblockDisplayData;
            SetNumberOfVerticesInWorkplane();

            foreach (var part in partsMap)
            {
                PartNamesMap.Add(part.Key, part.Value.Name);
            }
        }

        private void SetNumberOfVerticesInWorkplane()
        {
            NumberOfVerticesInWorkplane = VectorblockDisplayData
                .Select(x => new
                {
                    x.WorkplaneNumber,
                    x.NumberOfVertices
                })
                .GroupBy(x => x.WorkplaneNumber)
                .Select(x => new 
                { 
                    x.Key, 
                    VerticesInWorkplane = x.Sum(y => y.NumberOfVertices) 
                })
                .OrderBy(x => x.Key)
                .Select(x => x.VerticesInWorkplane)
                .ToList();
        }

        public List<VectorblockDisplayData> GetVectorblockDisplayData(int workplaneNumber)
        {
            return VectorblockDisplayData.Where(x => x.WorkplaneNumber == workplaneNumber).ToList();
        }

        //public List<Tuple<int, int>> WorkplanepositionsInVectorblockDisplayData;
    }
}
