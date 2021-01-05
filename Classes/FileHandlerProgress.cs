using OpenVectorFormat;
using OpenVectorFormat.AbstractReaderWriter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OVFSliceViewer.Classes
{
    public class FileHandlerProgress : IFileReaderWriterProgress
    {
        public bool IsCancelled { get; set; }
        public bool IsFinished { get; set; }

        public void Update(string message, int progressPerCent)
        {
            
        }
    }
}
