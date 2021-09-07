using OpenVectorFormat;
using OpenVectorFormat.AbstractReaderWriter;
using OpenVectorFormat.FileReaderWriterFactory;
using OpenVectorFormat.OVFReaderWriter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LayerViewer.Model
{
    public class OVFFileLoader
    {
        FileInfo _fileInfo;
        OVFFileReader _ovfFileReader;
        public readonly IFileReaderWriterProgress Progress;
        public OVFFileLoader(FileInfo file)
        {
            _fileInfo = file;
            Progress = new FileReaderWriterProgress();
            _ovfFileReader = new OVFFileReader();
            try
            {
                Task task = _ovfFileReader.OpenJobAsync(_fileInfo.FullName, Progress);
                task.GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                var temp = e;
                throw;
            }
            GetOVFFileInfo();
        }

        //protected List<int> GetPartsList()
        //{
        //    var parts = new List<int>();

        //    foreach (var part in _ovfFileReader.JobShell.PartsMap)
        //    {
        //        parts.Add(part.Key);
        //    }

        //    return parts;
        //}
        public WorkPlane GetWorkplane(int index)
        {
            return _ovfFileReader.GetWorkPlaneAsync(index).GetAwaiter().GetResult();
        }


        public OVFFileInfo OVFFileInfo { get; protected set; }
        private void GetOVFFileInfo()
        {
            int numberOfWorkplanes = _ovfFileReader.JobShell.NumWorkPlanes;
            OVFFileInfo = new OVFFileInfo
                (
                    _ovfFileReader.JobShell.PartsMap, 
                    numberOfWorkplanes, 
                    ReadVectorblockDisplayData(numberOfWorkplanes).GetAwaiter().GetResult()
                );
        }

        public async Task<List<VectorblockDisplayData>> ReadVectorblockDisplayData(int numberOfWorkplanes)
        {
            List<VectorblockDisplayData> list = new List<VectorblockDisplayData>();
            for (int i = 0; i < numberOfWorkplanes; i++)
            {
                var workplane = await _ovfFileReader.GetWorkPlaneAsync(i);

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

            return numberOfLines*2;
        }
    }
}
