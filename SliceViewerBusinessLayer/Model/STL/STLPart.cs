using OVFSliceViewerBusinessLayer.Classes;
using SliceViewerBusinessLayer.Model.STL;
using System;
using System.Collections.Generic;

namespace OVFSliceViewerBusinessLayer.Model
{
    
    public class STLPart: AbstrPart
    {
        private List<Triangle> _triangles;

        private STLRenderDataObject _renderData;
        public STLPart(List<Triangle> triangles, ISceneController scene, Func<bool> useColorIndex)
        {
            _triangles = triangles;

            CreateRenderData(useColorIndex, scene);
        }

        private void CreateRenderData(Func<bool> useColorIndex, ISceneController scene)
        {
            _renderData = new STLRenderDataObject(useColorIndex, scene.Camera);
            List<Vertex> vertices = new List<Vertex>();

            foreach (var triangle in _triangles)
            {
                vertices.Add(new Vertex(triangle.VertexA, 0));
                vertices.Add(new Vertex(triangle.VertexB, 0));
                vertices.Add(new Vertex(triangle.VertexC, 0));
            }
            //vertices.Add(new Vertex(new OpenTK.Vector3(-50, -50, 0 ), 0));
            //vertices.Add(new Vertex(new OpenTK.Vector3(50, -50, 0 ), 0));
            //vertices.Add(new Vertex(new OpenTK.Vector3(0, 50, 0 ), 0));

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
