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
        protected string _filePath;
        public async Task<AutomatedBuildChain.Proto.VoxelList> ReadVoxel(string filePath) 
        {
            VoxelList protoFile;
            using (var input = File.OpenRead(filePath))
            {
                protoFile = VoxelList.Parser.ParseFrom(input);
            }
            return protoFile;
        }

        public List<AutomatedBuildChain.Proto.Voxel> ReadStaticVocelData()
        {
            var result = new List<AutomatedBuildChain.Proto.Voxel>();

            result.Add(new AutomatedBuildChain.Proto.Voxel() 
            { 
                ClusterID = 0, 
                Dimension = new Dimension
                {
                    Depth = 10,
                    Height = 10,
                    Width = 10,
                },
                LowerLeftCorner = new ThreeDPoint { X = 0, Y = 0, Z = 0 }
            });

            result.Add(new AutomatedBuildChain.Proto.Voxel()
            {
                ClusterID = 1,
                Dimension = new Dimension
                {
                    Depth = 10,
                    Height = 10,
                    Width = 10,
                },
                LowerLeftCorner = new ThreeDPoint { X = 10, Y = 0, Z = 0 }
            });

            result.Add(new AutomatedBuildChain.Proto.Voxel()
            {
                ClusterID = 2,
                Dimension = new Dimension
                {
                    Depth = 10,
                    Height = 10,
                    Width = 10,
                },
                LowerLeftCorner = new ThreeDPoint { X = 10, Y = 10, Z = 0 }
            });

            result.Add(new AutomatedBuildChain.Proto.Voxel()
            {
                ClusterID = 3,
                Dimension = new Dimension
                {
                    Depth = 10,
                    Height = 10,
                    Width = 10,
                },
                LowerLeftCorner = new ThreeDPoint { X = 10, Y = 10, Z = 10 }
            });

            result.Add(new AutomatedBuildChain.Proto.Voxel()
            {
                ClusterID = 3,
                Dimension = new Dimension
                {
                    Depth = 10,
                    Height = 10,
                    Width = 10,
                },
                LowerLeftCorner = new ThreeDPoint { X = 20, Y = 10, Z = 10 }
            });

            result.Add(new AutomatedBuildChain.Proto.Voxel()
            {
                ClusterID = 4,
                Dimension = new Dimension
                {
                    Depth = 10,
                    Height = 10,
                    Width = 10,
                },
                LowerLeftCorner = new ThreeDPoint { X = 20, Y = 20, Z = 10 }
            });

            return result;
        }
    }
}
