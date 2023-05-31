using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Diagnostics;

namespace OVFSliceViewerCore.Model
{
    public class VoxelGLProgramm : GLProgramm
    {
        public VoxelGLProgramm(IRenderData renderObject, IModelViewProjection mvp, string vertexShader, string fragmentShader, string geometryPath = "\\Classes\\Shader\\shader.geometry") : base(renderObject, mvp, vertexShader, fragmentShader)
        {
            //_geometryPath = geometryPath;
        }

        protected int _cameraPositionPointer => GL.GetUniformLocation(_handle, "cameraPosition");

        public override void Use()
        {
            base.Use();
            GL.Uniform3(_cameraPositionPointer, _mvp.CameraDirection);
        }
        protected override void CreateVertexArray()
        {
            GL.CreateVertexArrays(1, out _vertexArray);
            GL.BindVertexArray(_vertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 4 * sizeof(float), 4);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 1, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);

            //ToDo: test
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 4 * sizeof(float), 4);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(3, 1, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.EnableVertexAttribArray(3);
        }

        protected override void AttachShader(List<int> shaderHandles)
        {
            var geometryShader = GL.CreateShader(ShaderType.GeometryShader);
            GL.ShaderSource(geometryShader, _geometryShaderCode);

            GL.CompileShader(geometryShader);
            string infoLogVert = GL.GetShaderInfoLog(geometryShader);
            if (infoLogVert != System.String.Empty)
                Debug.WriteLine(infoLogVert);


            shaderHandles.Add(geometryShader);
            base.AttachShader(shaderHandles);
        }
    }
}