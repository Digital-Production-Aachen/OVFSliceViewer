using OpenTK;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry3SharpLight;
using Mesh = System.Collections.Generic.List<Geometry3SharpLight.Triangle3f>;
using OpenTK.Mathematics;
using System.Text.RegularExpressions;
using AutomatedBuildChain.Proto;
using Google.Protobuf.Collections;
using Google.Protobuf;
using OVFSliceViewerBusinessLayer.Model;

namespace SliceViewerBusinessLayer.Model.STL
{
    //public class Triangle
    //{
    //    public Vector3 Normal;
    //    public Vector3 VertexA;
    //    public Vector3 VertexB;
    //    public Vector3 VertexC;
    //}
    public class STLReader
    {
        public Mesh Mesh { get; protected set; } = new Mesh();
        protected string _filePath;
        protected byte[] _fileHeader;
        protected uint _length;
        public STLReader() { }

        public async virtual Task<Mesh> ReadStl(string filePath)
        {
            _filePath = filePath;
            using (BinaryReader reader = new BinaryReader(new MemoryStream(File.ReadAllBytes(_filePath))))
            {
                _fileHeader = reader.ReadBytes(80);
                _length = reader.ReadUInt32();

                for (int i = 0; i < _length; i++)
                {
                    ReadTriangle(reader);
                }
            }
            return Mesh;
        }

        protected void ReadTriangle(BinaryReader reader)
        {
            var triangle = new Triangle3f();
            triangle.Normal = ReadVertex(reader);
            triangle.VertexA = ReadVertex(reader);
            triangle.VertexB = ReadVertex(reader);
            triangle.VertexC = ReadVertex(reader);

            reader.ReadUInt16();
            Mesh.Add(triangle);
        }

        Vector3 _vertex = Vector3.Zero;
        private Vector3 ReadVertex(BinaryReader reader)
        {
            _vertex.X = reader.ReadSingle();
            _vertex.Y = reader.ReadSingle();
            _vertex.Z = reader.ReadSingle();

            return _vertex;
        }

        public async virtual Task<Mesh> ReadObj(string filePath)
        {
            _filePath = filePath;
            string[] lines = File.ReadLines(_filePath).ToArray();
            return await ReadObj(lines);

        }
        public async virtual Task<Mesh> ReadObj(string[] lines)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<float> colors = new List<float>();

            // first read vertices and normals
            foreach (string line in lines)
            {
                string edited_line = line.Replace('.', ','); // to read float we need comma notation
                string[] parameters = edited_line.Split(new char[] { ' ' });

                switch (parameters[0])
                {
                    case "v":
                        float x = float.Parse(parameters[1]);
                        float y = float.Parse(parameters[2]);
                        float z = float.Parse(parameters[3]);
                        vertices.Add(new Vector3(x, y, z));


                        // only support colors that are specified because turning rgb colors
                        // into colorIndex used by openGL is not possible
                        if (parameters.Length == 7) // if colors are specified
                        {
                            Vector3 color = new Vector3(float.Parse(parameters[4]), float.Parse(parameters[5]), float.Parse(parameters[6]));
                            if (Equals(color, new Vector3(0.9f, 0, 0)))
                            {
                                colors.Add(0.9f);
                            }
                            else if (Equals(color, new Vector3(0.5f, 1, 0.5f)))
                            {
                                colors.Add(0.5f);
                            }
                            else if (Equals(color, new Vector3(1, 239f / 255f, 0)))
                            {
                                colors.Add(0.64f);
                            }
                            else
                            {
                                colors.Add(0);
                            }
                        }
                        else
                            colors.Add(0);

                        break;

                    case "vn":
                        x = float.Parse(parameters[1]);
                        y = float.Parse(parameters[2]);
                        z = float.Parse(parameters[3]);
                        normals.Add(new Vector3(x, y, z));
                        break;

                    default:
                        break;
                }
            }

            // creates faces (triangles) out of vertices and normals
            foreach (string line in lines)
            {
                string[] parameters = line.Split(new char[] { ' ' });

                if (parameters[0] == "f")
                {
                    Triangle3f triangle = new Triangle3f();

                    // vertex 1
                    string[] vertex_data = parameters[1].Split(new char[] { '/' });
                    triangle.VertexA = vertices[Convert.ToInt32(vertex_data[0]) - 1];
                    Vector3 normal = normals[Convert.ToInt32(vertex_data[2]) - 1];

                    // vertex 2
                    vertex_data = parameters[2].Split(new char[] { '/' });
                    triangle.VertexB = vertices[Convert.ToInt32(vertex_data[0]) - 1];
                    normal += normals[Convert.ToInt32(vertex_data[2]) - 1];

                    // vertex 3
                    vertex_data = parameters[3].Split(new char[] { '/' });
                    triangle.VertexC = vertices[Convert.ToInt32(vertex_data[0]) - 1];
                    normal += normals[Convert.ToInt32(vertex_data[2]) - 1];

                    normal.Normalize();
                    triangle.Normal = normal;

                    triangle.colorIndex = colors[Convert.ToInt32(vertex_data[0]) - 1];

                    Mesh.Add(triangle);
                }
            }

            return Mesh;
        }
        public async virtual Task<Mesh> ReadLgdff(string filePath, Dictionary<LABEL, List<int>> labelMap)
        {
            LabeledGeometryDefinitionFileFormat protoFile;

            using (var input = File.OpenRead(filePath))
            {
                protoFile = LabeledGeometryDefinitionFileFormat.Parser.ParseFrom(input);
            }

            string objString = Encoding.ASCII.GetString(protoFile.Obj.ToByteArray());
            string[] objLines = objString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var obj = await ReadObj(objLines);

            foreach (var label in protoFile.Map)
            {
                labelMap.Add(label.Label, label.TriangleIDs.ToList());
            }
            return obj;
        }


        public static bool Equals(Vector3 a, Vector3 b, float epsilon = 1e-1f)
        {
            return Math.Abs(a.X - b.X) < epsilon
            && Math.Abs(a.Y - b.Y) < epsilon
                && Math.Abs(a.Z - b.Z) < epsilon;
        }
    }
}
