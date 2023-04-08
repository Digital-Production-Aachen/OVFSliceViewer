using OVFSliceViewerBusinessLayer.Classes;
using SliceViewerBusinessLayer.Model.STL;
//using System.Windows.Forms;
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using OpenTK;
using Geometry3SharpLight;
using System.Diagnostics;
using SliceViewerBusinessLayer.Classes;
using OpenTK.Mathematics;

namespace OVFSliceViewerBusinessLayer.Model
{
    
    public class STLPart: AbstrPart
    {
        private List<Triangle3f> _triangles;
        private STLRenderDataObject _renderData;
        private AABBTree3 spatial;

        // essentially the length of the longest edges from a bounding box
        public float maxSize = 0;

        public STLPart(List<Triangle3f> triangles, ISceneController scene, Func<bool> useColorIndex)
        {
            _triangles = triangles;
            spatial = new AABBTree3(_triangles.ToArray());

            CreateRenderData(useColorIndex, scene);
        }

        private void CreateRenderData(Func<bool> useColorIndex, ISceneController scene)
        {
            _renderData = new STLRenderDataObject(useColorIndex, scene.Camera);
            RenderObjects.Add(_renderData);
            List<Vertex> vertices = new List<Vertex>();

            float minX = 0, maxX = 0, minY = 0, maxY = 0, minZ = 0, maxZ = 0;

            foreach (var triangle in _triangles)
            {
                vertices.Add(new Vertex(triangle.VertexA, triangle.colorIndex));
                vertices.Add(new Vertex(triangle.VertexB, triangle.colorIndex));
                vertices.Add(new Vertex(triangle.VertexC, triangle.colorIndex));

                // save min, max values from vertices
                minX = Math.Min(minX, Min(triangle.VertexA.X, triangle.VertexB.X, triangle.VertexC.X));
                maxX = Math.Max(maxX, Max(triangle.VertexA.X, triangle.VertexB.X, triangle.VertexC.X));

                minY = Math.Min(minY, Min(triangle.VertexA.Y, triangle.VertexB.Y, triangle.VertexC.Y));
                maxY = Math.Max(maxY, Max(triangle.VertexA.Y, triangle.VertexB.Y, triangle.VertexC.Y));

                minZ = Math.Min(minZ, Min(triangle.VertexA.Z, triangle.VertexB.Z, triangle.VertexC.Z));
                maxZ = Math.Max(maxZ, Max(triangle.VertexA.Z, triangle.VertexB.Z, triangle.VertexC.Z));
            }

            maxSize = Max( maxX - minX, maxY - minY, maxZ - minZ );

            _renderData.AddVertices(vertices, 0);

            _renderData.End = vertices.Count;
            _renderData.BindNewData();
        }

        public static float Min(float a, float b, float c)
        {
            return Math.Min(a, Math.Min(b, c));
        }

        public static float Max(float a, float b, float c)
        {
            return Math.Max(a, Math.Max(b, c));
        }

        public override void Render()
        {
            _renderData.Render();
        }

        public int FindNearestHitTriangle(Vector3 orig, Vector3 dir)
        {
            return spatial.FindNearestHitTriangle(new Ray3f(orig, dir));
        }

        public void ColorTriangle(int triId, float colorIndex)
        {
            var vertex = RenderObjects[0].Vertices[triId * 3];
            vertex.ColorIndex = colorIndex;
            RenderObjects[0].Vertices[triId * 3] = vertex;

            vertex = RenderObjects[0].Vertices[triId * 3 + 1];
            vertex.ColorIndex = colorIndex;
            RenderObjects[0].Vertices[triId * 3 + 1] = vertex;

            vertex = RenderObjects[0].Vertices[triId * 3 + 2];
            vertex.ColorIndex = colorIndex;
            RenderObjects[0].Vertices[triId * 3 + 2] = vertex;
        }

        

        public void WriteAsObj(string path)
        {
            StringWriter vertices = new StringWriter();
            StringWriter verticesNormals = new StringWriter();
            StringWriter faces = new StringWriter();

            for (int triId = 0; triId < _triangles.Count; triId++)
            {
                // write vertices
                for (int i = 0; i < 3; i++)
                {
                    var vertex = RenderObjects[0].Vertices[triId * 3 + i];
                    Vector3 v = vertex.Position;
                    Vector3 c = ColorMap(vertex.ColorIndex);
                    vertices.Write("v {0} {1} {2} {3} {4} {5}\n", v.X, v.Y, v.Z, c.X, c.Y, c.Z);
                }

                // write normals
                Vector3 n = _triangles[triId].Normal;
                verticesNormals.Write("vn {0} {1} {2}\n", n.X, n.Y, n.Z);

                // write face/triangle
                faces.Write("f {0}//{1} {2}//{3} {4}//{5}\n", (triId * 3 + 1), (triId + 1),(triId * 3 + 2),
                                                              (triId + 1), (triId * 3 + 3), (triId + 1));
            }


            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    string s = vertices.ToString();
                    s = s.Replace(',', '.');
                    sw.Write(s);

                    s = verticesNormals.ToString();
                    s = s.Replace(',', '.');
                    sw.Write(s);

                    s = faces.ToString();
                    s = s.Replace(',', '.');
                    sw.Write(s);
                }
            }
        }

        // colorMap function used to display color in opengl
        private Vector3 ColorMap(float colorIndex)
        {
            float colormap_red(float x)
            {
                if (x < 0.7)
                {
                    return (float)(4.0 * x - 1.5);
                }
                else
                {
                    return (float)(-4.0 * x + 4.5);
                }
            }

            float colormap_green(float x)
            {
                if (x < 0.5)
                {
                    return (float)(4.0 * x - 0.5);
                }
                else
                {
                    return (float)(-4.0 * x + 3.5);
                }
            }

            float colormap_blue(float x)
            {
                if (x < 0.3)
                {
                    return (float)(4.0 * x + 0.5);
                }
                else
                {
                    return (float)(-4.0 * x + 2.5);
                }
            }

            float r = (float)MathHelper.Clamp(colormap_red(colorIndex), 0.0, 1.0);
            float g = (float)MathHelper.Clamp(colormap_green(colorIndex), 0.0, 1.0);
            float b = (float)MathHelper.Clamp(colormap_blue(colorIndex), 0.0, 1.0);
            return new Vector3(r, g, b);
        }
    }
}
