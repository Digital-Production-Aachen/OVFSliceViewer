using AutomatedBuildChain.Proto;
using OpenVectorFormat.AbstractReaderWriter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenTK.Graphics.OpenGL.GL;

namespace OVFSliceViewerCore.Model.Voxel
{
    public class VoxelReader
    {
        public VoxelList Voxel;
        protected string _filePath;
        public async virtual Task<List<AutomatedBuildChain.Proto.Voxel>> ReadVoxel(string filePath) 
        {
            VoxelList protoFile;
            using (var input = File.OpenRead(filePath))
            {
                protoFile = VoxelList.Parser.ParseFrom(input);
            }
            return protoFile.VoxelList_.ToList();
        }
    }
}
