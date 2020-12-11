using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayerViewer
{
    public class DrawingBuffer : Control
    {
        protected List<VmLine> Lines { get; set; } = new List<VmLine>();
        protected Matrix TranslationMatrix { get; set; } = new Matrix();
        protected Matrix ScaleMatrix { get; set; } = new Matrix();
        protected Matrix STMatrix { get; set; } = new Matrix(); // Scale - Transform - Matrix

        protected void SetSTMatrix()
        {
            STMatrix = ScaleMatrix.Clone();
            STMatrix.Multiply(TranslationMatrix);
        }

        public DrawingBuffer(){}

        public void WriteLines()
        {
            foreach (var item in Lines)
            {
                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberDecimalSeparator = ".";

                Console.WriteLine("new Vertex { Color = new Vector4(" + item.Start.RgbR + "f, 0f, "+ item.Start.RgbB +"f, 0f), Position = new Vector3("+ item.Start.X.ToString(nfi) +"f, "+ item.Start.Y.ToString(nfi) + "f, 0)},");
                Console.WriteLine("new Vertex { Color = new Vector4(" + item.Ende.RgbR + "f, 0f, " + item.Ende.RgbB + "f, 0f), Position = new Vector3(" + item.Ende.X.ToString(nfi) + "f, " + item.Ende.Y.ToString(nfi) + "f, 0)},");
            }
        }
        public void Clear()
        {
            Lines = new List<VmLine>();
        }
        public List<Vertex> GetVertices()
        {
            var list = new List<Vertex>();

            foreach (var item in Lines)
            {
                list.Add
                    (
                        new Vertex { Color = new OpenTK.Vector4(Convert.ToSingle(item.Start.RgbR), 0f, Convert.ToSingle(item.Start.RgbB), 0f), Position = new OpenTK.Vector3(item.Start.X, item.Start.Y, item.Height) }
                    );
                
                list.Add(new Vertex { Color = new OpenTK.Vector4(Convert.ToSingle(item.Ende.RgbR), 0f, Convert.ToSingle(item.Ende.RgbB), 0f), Position = new OpenTK.Vector3(item.Ende.X, item.Ende.Y, item.Height) });
            
            }

            return list;
        }
        public new void Scale(float scalefactor)
        {
            if (scalefactor > 1)
            {

            }
            Scalefactor *= scalefactor;
            ScaleMatrix.Scale(scalefactor, scalefactor);
            SetSTMatrix();
        }
        public float Scalefactor { get; set; } = 1;
        public void UpdateLines(List<VmLine> lines)
        {
            Lines.AddRange(lines);
        }
    }
}
