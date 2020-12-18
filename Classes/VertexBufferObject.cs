using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;


namespace OVFSliceViewer.Classes
{
    //public class VertexBufferObject
    //{
    //    int _vertexBufferObject;

    //    public VertexBufferObject()
    //    {
    //        _vertexBufferObject = GL.GenBuffer();
    //    }

    //    private void CreateVertexBuffer()
    //    {
    //        _vertexBufferObject = GL.GenBuffer();
    //        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

    //        var handle = GCHandle.Alloc(_vertices, GCHandleType.Pinned);
    //        try
    //        {
    //            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(28 * _vertices.Length), handle.AddrOfPinnedObject(),
    //                BufferUsageHint.DynamicDraw);
    //        }
    //        finally
    //        {
    //            handle.Free();
    //        }
    //        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
    //    }
    //}
}
