using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mesh = System.Collections.Generic.List<SliceViewerBusinessLayer.Model.STL.Triangle>;

namespace SliceViewerBusinessLayer.Model.STL
{
    public class Triangle
    {
        public Vector3 Normal;
        public Vector3 VertexA;
        public Vector3 VertexB;
        public Vector3 VertexC;
    }
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
                PreReadingCommand();

                for (int i = 0; i < _length; i++)
                {
                    ReadTriangle(reader);
                    AddTriangleToMesh(i + 1);
                }
            }
            PostReadingCommand();
            return Mesh;
        }


        protected virtual void PreReadingCommand() { }
        protected virtual void PostReadingCommand() { }

        protected Triangle _triangle = new Triangle();
        protected void ReadTriangle(BinaryReader reader)
        {
            _triangle.Normal = ReadVertex(reader);
            _triangle.VertexA = ReadVertex(reader);
            _triangle.VertexB = ReadVertex(reader);
            _triangle.VertexC = ReadVertex(reader);

            reader.ReadUInt16();
        }
        protected virtual void AddTriangleToMesh(int id = 0)
        {
            Mesh.Add(_triangle);
        }

        Vector3 _vertex = Vector3.Zero;
        private Vector3 ReadVertex(BinaryReader reader)
        {
            _vertex.X = reader.ReadSingle();
            _vertex.Y = reader.ReadSingle();
            _vertex.Z = reader.ReadSingle();

            return _vertex;
        }
    }
}
