using OpenVectorFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayerViewer.Classes
{
    public class FileHandlerProgress : IFileHandlerProgress
    {
        public bool IsCancelled { get; set; }
        public bool IsFinished { get; set; }

        public void Update(string message, int progressPerCent)
        {
            
        }
    }
}
