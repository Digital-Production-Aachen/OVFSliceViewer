using OpenTK.Graphics.OpenGL4;
using OVFSliceViewerCore.Model.Shader;
using System.Collections.Generic;
using System.Diagnostics;

namespace OVFSliceViewerCore.Model
{
    public class VoxelGLProgramm : GLProgramm
    {
        public VoxelGLProgramm(IRenderData renderObject, IModelViewProjection mvp, string vertexShader, string fragmentShader, string geometryShader) : base(renderObject, mvp, vertexShader, fragmentShader, geometryShader)
        {
            //_geometryPath = geometryPath;
        }

        protected int _cameraPositionPointer => GL.GetUniformLocation(_handle, "cameraPosition");

        public override void Use()
        {
            ShaderController.GetInstance.UseVoxelShader(_mvp.CameraDirection);
            //base.Use();
            //GL.Uniform3(_cameraPositionPointer, );
        }
        //protected override void CreateVertexArray()
        //{
        //    GL.CreateVertexArrays(1, out _vertexArray);
        //    GL.BindVertexArray(_vertexArray);
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        //    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 4 * sizeof(float), 4);
        //    GL.EnableVertexAttribArray(0);
        //    GL.VertexAttribPointer(1, 1, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
        //    GL.EnableVertexAttribArray(1);

        //    //ToDo: test
        //    //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 4 * sizeof(float), 20);
        //    //GL.EnableVertexAttribArray(2);
        //    //GL.VertexAttribPointer(3, 1, VertexAttribPointerType.Float, false, 4 * sizeof(float), 16);
        //    //GL.EnableVertexAttribArray(3);
        //}
    }
}