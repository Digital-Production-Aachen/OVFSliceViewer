using OVFSliceViewer.Classes.ShaderNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using OpenTK;
using OpenVectorFormat;
using Google.Protobuf.Collections;
using static OpenVectorFormat.VectorBlock.Types;

namespace OVFSliceViewer.Classes
{
    public class DrawableObject
    {
        Shader _shader;
        int _vertexBuffer;
        int _vertexArray;
        protected Vertex[] _vertices = new Vertex[0];
        int _beginDrawingAt = 0;
        int _stopDrawingAt = 0;

        public DrawableObject(Shader shader, int buffer)
        {
            _shader = shader;
            _vertexBuffer = buffer;
            //CreateVertexBuffer();
            CreateVertexArray();

        }

        public void SetRangeToDraw(int end, int start = 0)
        {
            _beginDrawingAt = start;
            _stopDrawingAt = end;
        }
        public void ChangePicture(Vertex[] vertices)
        {
            _vertices = vertices;
            _stopDrawingAt = vertices.Length;
            RebindBufferObject();
        }
        protected void RebindBufferObject()
        {
            var handle = GCHandle.Alloc(_vertices, GCHandleType.Pinned);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(28 * _vertices.Length), handle.AddrOfPinnedObject(), BufferUsageHint.StaticDraw);
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        //public void SetNumberOfLinesToDraw(int numberOfLines)
        //{
        //    _stopDrawingAt = numberOfLines*2;
        //}

        public void Draw(Matrix4 modelViewProjection)
        {
            GL.UniformMatrix4(_shader.GetUniformLocation(), false, ref modelViewProjection);

            GL.BindVertexArray(_vertexArray);
            GL.LineWidth(5f);
            GL.DrawArrays(PrimitiveType.Lines, _beginDrawingAt, _stopDrawingAt);
        }

        private void CreateVertexBuffer()
        {
            //GL.GenBuffers(2, out _vertexBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);

            var handle = GCHandle.Alloc(_vertices, GCHandleType.Pinned);
            try
            {
                GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(28 * _vertices.Length), handle.AddrOfPinnedObject(),
                    BufferUsageHint.StaticDraw);
            }
            finally
            {
                handle.Free();
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        }

        private void CreateVertexArray()
        {
            _vertexArray = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 28, IntPtr.Zero);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 28, new IntPtr(12));
            GL.EnableVertexAttribArray(1);
            GL.BindVertexArray(_vertexArray);
        }
    }


    public class DrawablePart
    {
        protected DrawableObject _contour;
        protected DrawableObject _volume;
        protected VectorBlock _vectorBlock;
        int _numberOfLayers;
        VectorWithColorFactory _vectorFactory = new VectorWithColorFactory(250, 350);
        float _height;
        List<VmLine> _contourLines = new List<VmLine>();
        List<VmLine> _volumeLines = new List<VmLine>();
        int[] numberOfContourLinesInWorkplane;


        public int HightlightIndex { get; set; } = 0;

        public DrawablePart(Shader shader, int contourBuffer, int volumeBuffer, int numberOfLayers)
        {
            _contour = new DrawableObject(shader, contourBuffer);
            _volume = new DrawableObject(shader, volumeBuffer);
            _numberOfLayers = numberOfLayers;
            numberOfContourLinesInWorkplane = new int[numberOfLayers];
        }

        public void SetRangeToDraw(int end, int start = 0)
        {
            var endcount = 0;
            var startcount = 0;

            for (int i = 0; i <= start; i++)
            {
                startcount += numberOfContourLinesInWorkplane[i];
            }

            for(int i = start; i <= end; i++)
            {
                endcount += numberOfContourLinesInWorkplane[i];
            }
            _contour.SetRangeToDraw(endcount*2, startcount*2);
        }

        public void UpdateContour()
        {
            var vertices = new List<Vertex>();

            foreach (var item in _contourLines)
            {
                var startVertex = new Vertex { Color = item.Start.Color, Position = item.Start.Position };
                var endVertex = new Vertex { Color = item.Ende.Color, Position = item.Ende.Position };

                vertices.Add(startVertex);
                vertices.Add(endVertex);
            }

            _contour.ChangePicture(vertices.ToArray());
            //_contourLines.RemoveAll(x => true);
            _contourLines = new List<VmLine>();
        }

        public void UpdateVolume()
        {
            var vertices = new List<Vertex>();

            foreach (var item in _volumeLines)
            {
                var startVertex = new Vertex { Color = item.Start.Color, Position = item.Start.Position };
                var endVertex = new Vertex { Color = item.Ende.Color, Position = item.Ende.Position };

                vertices.Add(startVertex);
                vertices.Add(endVertex);
            }

            _volume.ChangePicture(vertices.ToArray());
        }

        public void DrawAll(Matrix4 modelViewProjection)
        {
            //UpdateContour();
            _contour.Draw(modelViewProjection);
            _volume.Draw(modelViewProjection);
        }
        public void AddContour(VectorBlock vectorBlock, int layernumber, float height)
        {
            _height = height;
            _vectorBlock = vectorBlock;
            var lines = VectorBlockToViewModel();

            if (_vectorBlock.LpbfMetadata.PartArea == PartArea.Contour)
            {
                _contourLines.AddRange(lines);
            }
            numberOfContourLinesInWorkplane[layernumber] += lines.Count;
        }
        public void AddVectorBlock(VectorBlock vectorBlock, int layernumber, float height)
        {
            _height = height;
            _vectorBlock = vectorBlock;
            var lines = VectorBlockToViewModel();

            if (_vectorBlock.LpbfMetadata.PartArea == PartArea.Contour)
            {
                //_contourLines.AddRange(lines);
            }
            else
            {
                _volumeLines.AddRange(lines);
            }
        }
        public void RemoveVolume()
        {
            _volumeLines = new List<VmLine>();
            UpdateVolume();
        }
        private List<VmLine> VectorBlockToViewModel()
        {
            List<VmLine> list = new List<VmLine>();
            var points = new RepeatedField<float>();
            var color = new Vector4(1f, 0f, 0f, 0f);
            switch (HightlightIndex)
            {
                case 0: break;//0 = nothing
                case 1:
                    if (_vectorBlock.LpbfMetadata.PartArea == PartArea.Contour) color = new Vector4(87f / 255f, 171f / 255f, 39f / 255f, 0f); //1 = Contour 
                    break;
                case 2:
                    if (_vectorBlock.LpbfMetadata.StructureType == StructureType.Support) color = new Vector4(87f / 255f, 171f / 255f, 39f / 255f, 0f); //2 = Support
                    break;
                default: break;
            }

            _vectorFactory.SetColor(color);
            switch (_vectorBlock.VectorDataCase)
            {
                case VectorBlock.VectorDataOneofCase.LineSequence:
                    list = LineSequenceToViewModel();
                    break;
                case VectorBlock.VectorDataOneofCase.Hatches:
                    list = HatchesToViewModel();
                    break;
                case VectorBlock.VectorDataOneofCase.LineSequenceParaAdapt:
                    list = LineSequenceParaAdaptToViewModel(_vectorBlock.LineSequenceParaAdapt);
                    break;
                case VectorBlock.VectorDataOneofCase.HatchParaAdapt:
                    list = HatchesParaAdaptToViewModel();
                    break;
                default:
                    break;
            }
            return list;
        }
        private List<VmLine> LineSequenceToViewModel()
        {
            var points = _vectorBlock.LineSequence.Points;
            var list = new List<VmLine>();

            for (int i = 3; i <= points.Count(); i += 2)
            {
                var positionStart = new Vector3(points[i - 3], points[i - 2], _height);
                var positionEnd = new Vector3(points[i - 1], points[i], _height);

                var vmVectorStart = _vectorFactory.GetVectorWithColor(positionStart);
                var vmVectorEnde = _vectorFactory.GetVectorWithColor(positionEnd);
                var vmVector = new VmLine() { Start = vmVectorStart, Ende = vmVectorEnde, Height = _height };

                list.Add(vmVector);
            }
            return list;
        }
        private List<VmLine> LineSequenceParaAdaptToViewModel(LineSequenceParaAdapt lineSequenceParaAdapt)
        {
            var points = lineSequenceParaAdapt.PointsWithParas;
            var list = new List<VmLine>();

            for (int i = 5; i <= points.Count(); i += 3)
            {
                var positionStart = new Vector3(points[i - 5], points[i - 4], _height);
                var positionEnd = new Vector3(points[i - 2], points[i - 1], _height);

                var vmVectorStart = _vectorFactory.GetVectorWithPowerColor(positionStart, points[i - 3]);
                var vmVectorEnde = _vectorFactory.GetVectorWithPowerColor(positionEnd, points[i]);

                var vmVector = new VmLine() { Start = vmVectorStart, Ende = vmVectorEnde, Height = _height };
                list.Add(vmVector);
            }
            return list;
        }

        private List<VmLine> HatchesToViewModel()
        {
            var points = _vectorBlock.Hatches.Points;
            var list = new List<VmLine>();
            for (int i = 3; i <= points.Count; i += 4)
            {
                var positionStart = new Vector3(points[i - 3], points[i - 2], _height);
                var positionEnd = new Vector3(points[i - 1], points[i - 0], _height);

                var vmVectorStart = _vectorFactory.GetVectorWithColor(positionStart);
                var vmVectorEnde = _vectorFactory.GetVectorWithColor(positionEnd);

                var vmLine = new VmLine() { Start = vmVectorStart, Ende = vmVectorEnde, Height = _height };

                list.Add(vmLine);
            }
            return list;
        }

        private List<VmLine> HatchesParaAdaptToViewModel()
        {
            var list = new List<VmLine>();

            foreach (var item in _vectorBlock.HatchParaAdapt.HatchAsLinesequence)
            {
                list.AddRange(LineSequenceParaAdaptToViewModel(item));
            }

            return list;
        }
    }
}
