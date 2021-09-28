using OpenVectorFormat;
using OpenVectorFormat.AbstractReaderWriter;
using OpenVectorFormat.FileReaderWriterFactory;
using OpenVectorFormat.OVFReaderWriter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OVFSliceViewerBusinessLayer.Model
{
    public class OVFFileLoader
    {
        FileInfo _fileInfo;
        OVFFileReader _ovfFileReader;
        public readonly IFileReaderWriterProgress Progress;
        public Job Jobshell { get; protected set; }
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
            Jobshell = _ovfFileReader.JobShell;
            GetOVFFileInfo();
        }

        public WorkPlane GetWorkplaneShell(int index)
        {
            return _ovfFileReader.GetWorkPlaneShell(index);
        }
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
                    _ovfFileReader
                );
        }

        public VectorBlock GetVectorBlock(int workplaneIndex, int vectorblockIndex)
        {
            return _ovfFileReader.GetVectorBlockAsync(workplaneIndex, vectorblockIndex).GetAwaiter().GetResult();
        }
    }
}
