using OpenVectorFormat;
using OpenVectorFormat.AbstractReaderWriter;
using OpenVectorFormat.FileReaderWriterFactory;
using OpenVectorFormat.ILTFileReaderAdapter;
using OpenVectorFormat.OVFReaderWriter;
using SliceViewerBusinessLayer.Model;
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
        FileReader _ovfFileReader;
        public readonly IFileReaderWriterProgress Progress;
        public Job Jobshell { get; protected set; }
        public OVFFileLoader(IFileReaderWriterProgress fileReaderWriterProgress)
        {
            Progress = fileReaderWriterProgress ?? new FileReaderWriterProgress();
        }
        public async Task OpenFile(FileInfo file)
        {
            _fileInfo = file;
            FileReader reader;

            if (_fileInfo.Extension.ToLower() == ".gcode")
            {
                reader = new GCodeReader();
            }
            else
            {
                reader = FileReaderFactory.CreateNewReader(_fileInfo.Extension.ToLower());
            }
            try
            {
                Task task = reader.OpenJobAsync(_fileInfo.FullName, Progress);
                await task;
            }
            catch (Exception e)
            {
                var temp = e;
                throw;
            }
            await OpenFile(reader);
        }
        public async Task OpenFile(FileReader reader)
        {
            _ovfFileReader = reader;

            Jobshell = _ovfFileReader.JobShell;
            await GetOVFFileInfo();
        }
        public void CloseFile()
        {
            if (_ovfFileReader == null)
            {
                return;
            }
            _ovfFileReader.Dispose();
            _ovfFileReader = null;
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
        private async Task GetOVFFileInfo()
        {
            int numberOfWorkplanes = _ovfFileReader.JobShell.NumWorkPlanes;
            OVFFileInfo = new OVFFileInfo();
            await OVFFileInfo.ReadData
                (
                _ovfFileReader,
                numberOfWorkplanes
                );
        }

        public VectorBlock GetVectorBlock(int workplaneIndex, int vectorblockIndex)
        {
            return _ovfFileReader.GetVectorBlockAsync(workplaneIndex, vectorblockIndex).GetAwaiter().GetResult();
        }
    }
}
