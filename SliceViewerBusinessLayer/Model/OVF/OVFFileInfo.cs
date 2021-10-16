using Google.Protobuf.Collections;
using OpenVectorFormat;
using OpenVectorFormat.OVFReaderWriter;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OVFSliceViewerBusinessLayer.Model
{
    public class OVFFileInfo
    {
        public List<int> PartKeys { get; protected set; }
        public Dictionary<int, string> PartNamesMap { get; protected set; } = new Dictionary<int, string>();
        public List<VectorblockDisplayData> VectorblockDisplayData { get; protected set; } = new List<VectorblockDisplayData>();
        public List<int> NumberOfVerticesInWorkplane { get; protected set; } = new List<int>();
        public List<int>[] ContourVectorblocksInWorkplaneLUT { get; protected set; } = new List<int>[1];
        public int NumberOfWorkplanes { get; protected set; } = 0;
        public int NumberOfVectorblocks => VectorblockDisplayData.Count;

        public OVFFileInfo()
        {
            ContourVectorblocksInWorkplaneLUT[0] = new List<int>() { 0 };
        }
        public async Task ReadData(MapField<int, Part> partsMap, int numberOfWorkplanes, OVFFileReader ovfFileReader)
        {
            PartKeys = partsMap.Keys.ToList();
            NumberOfWorkplanes = numberOfWorkplanes;

            InitContourVectorblocksInWorkplaneMap();

            VectorblockDisplayData = await ReadVectorblockDisplayData(numberOfWorkplanes, ovfFileReader);
            SetNumberOfVerticesInWorkplane();

            foreach (var part in partsMap)
            {
                PartNamesMap.Add(part.Key, part.Value.Name);
            }
        }

        private void InitContourVectorblocksInWorkplaneMap()
        {
            ContourVectorblocksInWorkplaneLUT = new List<int>[NumberOfWorkplanes];
            for (int i = 0; i < ContourVectorblocksInWorkplaneLUT.Length; i++)
            {
                ContourVectorblocksInWorkplaneLUT[i] = new List<int>();
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

        private async Task<List<VectorblockDisplayData>> ReadVectorblockDisplayData(int numberOfWorkplanes, OVFFileReader ovfFileReader)
        {
            List<VectorblockDisplayData> list = new List<VectorblockDisplayData>();
            for (int i = 0; i < numberOfWorkplanes; i++)
            {
                var workplane = await ovfFileReader.GetWorkPlaneAsync(i);

                for (int j = 0; j < workplane.NumBlocks; j++)
                {
                    var vectorblockDisplayData = new VectorblockDisplayData()
                    {
                        WorkplaneNumber = i,
                        VectorblockNumber = j,
                        NumberOfVertices = GetVectorblockNumberOfVertices(workplane.VectorBlocks[j]),
                        PartKey = workplane.VectorBlocks[j].MetaData.PartKey
                    };

                    list.Add(vectorblockDisplayData);
                    if (workplane.VectorBlocks[j].LpbfMetadata.PartArea == VectorBlock.Types.PartArea.Contour)
                    {
                        ContourVectorblocksInWorkplaneLUT[i].Add(j);
                    }
                }

                if (workplane.NumBlocks == 0)
                {
                    var vectorblockDisplayData = new VectorblockDisplayData()
                    {
                        WorkplaneNumber = i,
                        VectorblockNumber = 0,
                        NumberOfVertices = 0,
                        PartKey = ovfFileReader.JobShell.PartsMap.FirstOrDefault().Key
                    };

                    list.Add(vectorblockDisplayData);
                }
            }

            return list;
        }
        int GetVectorblockNumberOfVertices(VectorBlock vectorblock)
        {
            int numberOfLines = 0;

            switch (vectorblock.VectorDataCase)
            {
                case VectorBlock.VectorDataOneofCase.LineSequence:
                    numberOfLines = (vectorblock.LineSequence.Points.Count / 2) - 1;
                    break;
                case VectorBlock.VectorDataOneofCase.Hatches:
                    numberOfLines = vectorblock.Hatches.Points.Count / 2 / 2;
                    break;
                case VectorBlock.VectorDataOneofCase.LineSequenceParaAdapt:
                    numberOfLines = vectorblock.LineSequenceParaAdapt.PointsWithParas.Count / 3 - 1;
                    break;
                case VectorBlock.VectorDataOneofCase.HatchParaAdapt:
                    numberOfLines = vectorblock.HatchParaAdapt.HatchAsLinesequence.Select(x => x.PointsWithParas.Count / 3).Sum();
                    break;
                default:
                    break;
            }

            return numberOfLines * 2;
        }
    }
}
