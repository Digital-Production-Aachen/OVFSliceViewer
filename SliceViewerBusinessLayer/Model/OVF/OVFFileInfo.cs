using Google.Protobuf.Collections;
using OpenVectorFormat;
using OpenVectorFormat.AbstractReaderWriter;
using OpenVectorFormat.OVFReaderWriter;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OVFSliceViewerCore.Model
{
    public class OVFFileInfo
    {
        public List<int> PartKeys { get; protected set; }
        public Dictionary<int, string> PartNamesMap { get; protected set; } = new Dictionary<int, string>();
        public List<VectorblockDisplayData> VectorblockDisplayData { get; protected set; } = new List<VectorblockDisplayData>();
        public Dictionary<int, int> NumberOfVerticesInWorkplane { get; protected set; } = new Dictionary<int, int>();
        public List<int>[] ContourVectorblocksInWorkplaneLUT { get; protected set; } = new List<int>[1];
        public int NumberOfWorkplanes { get; set; } = 0;
        public int NumberOfVectorblocks => VectorblockDisplayData.Count;
        FileReader _ovfFileReader;

        public OVFFileInfo()
        {
            ContourVectorblocksInWorkplaneLUT[0] = new List<int>() { 0 };
        }
        public async Task ReadData(FileReader ovfFileReader, int endWorkplaneIndex=1, int startWorkplaneIndex = 0)
        {
            var partsMap = ovfFileReader.JobShell.PartsMap;
            PartKeys = partsMap.Keys.ToList();
            NumberOfWorkplanes = ovfFileReader.JobShell.NumWorkPlanes;
            _ovfFileReader = ovfFileReader;
            NumberOfVerticesInWorkplane = new Dictionary<int, int>();

            InitContourVectorblocksInWorkplaneMap();

            VectorblockDisplayData = await ReadVectorblockDisplayData(endWorkplaneIndex, startWorkplaneIndex);
            SetNumberOfVerticesInWorkplane();

            foreach (var part in partsMap)
            {
                PartNamesMap.Add(part.Key, part.Value.Name);
            }
        }
        public async Task ReadWorkplane(int workplaneNumber)
        {
            VectorblockDisplayData = await ReadVectorblockDisplayData(workplaneNumber+1, workplaneNumber);
            SetNumberOfVerticesInWorkplane();
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
            var workplaneNumbers = VectorblockDisplayData.Select(x => x.WorkplaneNumber).Distinct().ToList();

            foreach (var workplaneNumber in workplaneNumbers)
            {
                NumberOfVerticesInWorkplane[workplaneNumber] = VectorblockDisplayData.Where(x => x.WorkplaneNumber == workplaneNumber).Sum(x => x.NumberOfVertices);
            }
        }

        public List<VectorblockDisplayData> GetVectorblockDisplayData(int workplaneNumber)
        {
            return VectorblockDisplayData.Where(x => x.WorkplaneNumber == workplaneNumber).ToList();
        }

        private async Task<List<VectorblockDisplayData>> ReadVectorblockDisplayData(int endWorkplaneIndex, int startWorkplaneIndex = 0)
        {
            List<VectorblockDisplayData> list = new List<VectorblockDisplayData>();
            for (int i = startWorkplaneIndex; i < endWorkplaneIndex; i++)
            {
                var workplane = await _ovfFileReader.GetWorkPlaneAsync(i);

                for (int j = 0; j < workplane.NumBlocks; j++)
                {
                    int partkey = -1;
                    if (workplane.VectorBlocks[j].MetaData != null) partkey = workplane.VectorBlocks[j].MetaData.PartKey;
                    var vectorblockDisplayData = new VectorblockDisplayData()
                    {
                        WorkplaneNumber = i,
                        VectorblockNumber = j,
                        NumberOfVertices = GetVectorblockNumberOfVertices(workplane.VectorBlocks[j]),
                        PartKey = partkey
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
                        PartKey = _ovfFileReader.JobShell.PartsMap.FirstOrDefault().Key
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
