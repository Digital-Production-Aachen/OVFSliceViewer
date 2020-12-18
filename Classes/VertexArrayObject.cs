using OVFSliceViewer.Classes.ShaderNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace OVFSliceViewer.Classes
{
    //public class VertexArrayObject
    //{
    //    Shader _shader;
    //    Vertex[] _vertices;
    //    int _vertexArrayObject;
    //    public VertexArrayObject(Shader shader)
    //    {
    //        _shader = shader;
    //        _vertexArrayObject = GL.GenVertexArray();

    //        GL.BindVertexArray(_vertexArrayObject);
    //        // 2. copy our vertices array in a buffer for OpenGL to use
    //        //GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
    //        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.DynamicDraw);
    //        // 3. then set our vertex attributes pointers
    //        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
    //        GL.EnableVertexAttribArray(0);
    //    }

    //    public void Draw()
    //    {
    //        _shader.Use();
    //        GL.BindVertexArray(_vertexArrayObject);
    //        //someOpenGLFunctionThatDrawsOurTriangle();
    //    }

    //}
}
