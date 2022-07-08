using OpenTK.Graphics.OpenGL4;
//using OpenTK.Mathematics;
using System.Diagnostics;

namespace OVFSliceViewerBusinessLayer.Model
{
    public class GeometryShader: GLProgramm
    {
        public GeometryShader(IRenderData renderObject, IModelViewProjection mvp,
            string vertexPath = @"Classes/Shader/shader.vert",
            string fragmentPath = @"Classes/Shader/shader.frag") : base(renderObject, mvp, vertexPath, fragmentPath){}

        protected override void CompileShader()
        {
            ReadShader();
            var VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, _vertexShaderCode);

            var FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, _fragmentShaderCode);

            GL.CompileShader(VertexShader);

            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            if (infoLogVert != System.String.Empty)
                Debug.WriteLine(infoLogVert);

            GL.CompileShader(FragmentShader);

            string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

            if (infoLogFrag != System.String.Empty)
                Debug.WriteLine(infoLogFrag);

            _handle = GL.CreateProgram();

            GL.AttachShader(_handle, VertexShader);
            GL.AttachShader(_handle, FragmentShader);

            GL.LinkProgram(_handle);
            GL.ValidateProgram(_handle);

            infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

            if (infoLogFrag != System.String.Empty)
                Debug.WriteLine(infoLogFrag);

            GL.DetachShader(_handle, VertexShader);
            GL.DetachShader(_handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
        }

    }



}
