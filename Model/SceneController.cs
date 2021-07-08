using OpenVectorFormat;
using OpenVectorFormat.FileReaderWriterFactory;
using OpenVectorFormat.OVFReaderWriter;
using System.Collections.Generic;
using System.IO;

namespace LayerViewer.Model
{
    public class SceneController
    {
        protected OVFFileLoader _ovfFileLoader;
        public Dictionary<int, AbstrPart> PartsInViewer;
        
        public void RenderAllParts()
        {
            foreach (var part in PartsInViewer.Values)
            {
                part.RenderObjects.ForEach(y => y.Render());
            }
        }
        
        public void LoadFile(string path)
        {
            var fileInfo = new FileInfo(path);

            if (fileInfo.Extension.ToLower() == "ovf")
            {
                _ovfFileLoader = new OVFFileLoader(fileInfo);
            }
        }

        public void LoadWorkplaneToBuffer(int index)
        {
            var workplane = _ovfFileLoader.GetWorkplane(index);

            foreach (var part in PartsInViewer.Values)
            {
                if (part is OVFPart)
                {
                    var ovfPart = part as OVFPart;
                    ovfPart.AddWorkplane(workplane);
                }
            }
        }
    }


    public class OVFFileLoader
    {
        FileInfo _fileInfo;
        OVFFileReader _ovfFileReader;
        public readonly FileReaderWriterProgress Progress;
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
