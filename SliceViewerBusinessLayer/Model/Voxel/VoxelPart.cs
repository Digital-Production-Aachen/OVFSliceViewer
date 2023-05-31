using OVFSliceViewerCore.Classes;
using OVFSliceViewerCore.Model.RenderData;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using Geometry3SharpLight;
using AutomatedBuildChain.Proto;

namespace OVFSliceViewerCore.Model.Voxel
{
    public class VoxelPart : AbstrPart
    {
        private VoxelRenderDataObject _renderData;
        public VoxelPart(VoxelList voxel, ISceneController scene, Func<bool> useColorIndex) 
        {
            CreateRenderData(voxel);
        }
        private void CreateRenderData(VoxelList voxel)
        {
            List<Vertex> vertices = new List<Vertex>();
            //float minX = 0, maxX = 0, minY = 0, maxY = 0, minZ = 0, maxZ = 0;

            foreach (var v  in voxel.VoxelList_)
            {
                vertices.Add(new Vertex(new Vector3(v.LowerLeftCorner.X, v.LowerLeftCorner.Y, v.LowerLeftCorner.Z)));
                vertices.Add(new Vertex(new Vector3(v.Dimension.Height, v.Dimension.Width, v.Dimension.Depth)));

                //minX = Math.Min(minX, Min(triangle.VertexA.X, triangle.VertexB.X, triangle.VertexC.X));
                //maxX = Math.Max(maxX, Max(triangle.VertexA.X, triangle.VertexB.X, triangle.VertexC.X));
                //minY = Math.Min(minY, Min(triangle.VertexA.Y, triangle.VertexB.Y, triangle.VertexC.Y));
                //maxY = Math.Max(maxY, Max(triangle.VertexA.Y, triangle.VertexB.Y, triangle.VertexC.Y));
                //minZ = Math.Min(minZ, Min(triangle.VertexA.Z, triangle.VertexB.Z, triangle.VertexC.Z));
                //maxZ = Math.Max(maxZ, Max(triangle.VertexA.Z, triangle.VertexB.Z, triangle.VertexC.Z));
            }

            _renderData.AddVertices(vertices, 0);
            _renderData.End = vertices.Count;
            _renderData.BindNewData();
        }
        public override void Render()
        {
            _renderData.Render();
        }


    }
}
