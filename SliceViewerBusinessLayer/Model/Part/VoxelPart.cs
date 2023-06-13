using OVFSliceViewerCore.Model.RenderData;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using Geometry3SharpLight;
using AutomatedBuildChain.Proto;

namespace OVFSliceViewerCore.Model.Part
{
    public class VoxelPart : AbstrPart
    {
        private VoxelRenderDataObject _renderData;
        public VoxelPart(IList<AutomatedBuildChain.Proto.Voxel> voxel, ISceneController scene, Func<bool> useColorIndex)
        {
            _renderData = new VoxelRenderDataObject(useColorIndex, scene.Camera);
            CreateRenderData(voxel);
        }
        private void CreateRenderData(IList<AutomatedBuildChain.Proto.Voxel> voxel)
        {
            List<Vertex> vertices = new List<Vertex>();
            //float minX = 0, maxX = 0, minY = 0, maxY = 0, minZ = 0, maxZ = 0;
            var max = voxel.Max(x => x.ClusterID) + 1;
            foreach (var v in voxel)
            {
                vertices.Add(new Vertex(new Vector3(v.LowerLeftCorner.X, v.LowerLeftCorner.Y, v.LowerLeftCorner.Z), v.ClusterID + 1));
                vertices.Add(new Vertex(new Vector3(v.Dimension.Height, v.Dimension.Width, v.Dimension.Depth), max));
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
