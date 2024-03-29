﻿using OpenTK.Graphics.OpenGL4;
//using OpenTK.Mathematics;
using System.Diagnostics;

namespace LayerViewer.Model
{
    public class GeometryShader: Shader
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

            handle = GL.CreateProgram();

            GL.AttachShader(handle, VertexShader);
            GL.AttachShader(handle, FragmentShader);

            GL.LinkProgram(handle);
            GL.ValidateProgram(handle);

            infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

            if (infoLogFrag != System.String.Empty)
                Debug.WriteLine(infoLogFrag);

            GL.DetachShader(handle, VertexShader);
            GL.DetachShader(handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
        }

    }



}
