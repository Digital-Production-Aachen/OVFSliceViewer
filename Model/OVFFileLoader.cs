using OpenVectorFormat.AbstractReaderWriter;
using OpenVectorFormat.OVFReaderWriter;
using System.Collections.Generic;
using System.IO;

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
            _ovfFileReader.OpenJobAsync(_fileInfo.FullName, Progress).GetAwaiter().GetResult();
        }

        public List<int> GetPartsList()
        {
            var parts = new List<int>();

            foreach (var part in _ovfFileReader.JobShell.PartsMap)
            {
                parts.Add(part.Key);
            }

            return parts;
        }

        public WorkPlane GetWorkplane(int index)
        {
            return _ovfFileReader.GetWorkPlaneAsync(index).GetAwaiter().GetResult();
        }
    }
}
