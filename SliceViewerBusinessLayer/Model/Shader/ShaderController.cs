using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SliceViewerBusinessLayer.Model.Shader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OVFSliceViewerCore.Model.Shader
{
    public class ShaderController
    {
        private ShaderController() { }
        private static ShaderController _instance = new ShaderController();
        public static ShaderController GetInstance => _instance;


        public int _currentShaderHandle = -1;
        public int _ovfShaderHandle = -1;
        public int _stlShaderHandle = -1;
        public int _voxelShaderHandle = -1;

        public int OVFColorPointer => GL.GetUniformLocation(_currentShaderHandle, "colorIndex");
        public int MvpPointer => GL.GetUniformLocation(_currentShaderHandle, "Mvp");

        private int _cameraPositionPointer;
        public void UseOVFShader()
        {
            if(_currentShaderHandle != _ovfShaderHandle || _ovfShaderHandle == -1)
            {
                if (_ovfShaderHandle == -1)
                {
                    _ovfShaderHandle = CompileShader(VertexShader.Shader, "", FragmentShader.Shader);
                    //OVFColorPointer = GL.GetUniformLocation(_ovfShaderHandle, "colorIndex");
                }
                    

                GL.UseProgram(_ovfShaderHandle);
                _currentShaderHandle = _ovfShaderHandle;
            }
        }

        public void UseSTLShader(Vector3 cameraDirection)
        {
            var error = GL.GetError();
            if (error != ErrorCode.NoError)
                Debug.WriteLine($"{error} before using STLShader");

            if (_currentShaderHandle != _stlShaderHandle || _stlShaderHandle == -1)
            {
                if(_stlShaderHandle == -1)
                {
                    _stlShaderHandle = CompileShader(STLShader.Shader, GeometryShaderCode.Shader, FragmentShader.Shader);
                    _cameraPositionPointer = GL.GetUniformLocation(_stlShaderHandle, "cameraPosition");
                    
                    error = GL.GetError();
                    if (error != ErrorCode.NoError)
                        Debug.WriteLine($"{error} After compiling and getting uniform location of stl shader \"cameraPosition\"");
                }

                GL.UseProgram(_stlShaderHandle);

                error = GL.GetError();
                if (error != ErrorCode.NoError)
                    Debug.WriteLine($"{error} after GL.UseProgram");

                _currentShaderHandle = _stlShaderHandle;

                

            }

            GL.Uniform3(_cameraPositionPointer, cameraDirection);
        }

        private int CompileShader(string vertexShaderCode, string geometryShaderCode, string fragmentShaderCode)
        {
            //ToDo: compile shader if one is given

            var VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, vertexShaderCode);

            int geometryShader = -1;

            if (!string.IsNullOrWhiteSpace(geometryShaderCode))
            {
                geometryShader = GL.CreateShader(ShaderType.GeometryShader);
                GL.ShaderSource(geometryShader, geometryShaderCode);
                GL.CompileShader(geometryShader);
                string infoLogGeom = GL.GetShaderInfoLog(geometryShader);
                if (infoLogGeom != System.String.Empty)
                    Debug.WriteLine(infoLogGeom);
            }

            var FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, fragmentShaderCode);

            GL.CompileShader(VertexShader);
            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            if (infoLogVert != System.String.Empty)
                Debug.WriteLine($"Error in vertex-shader compilation in  AbstrGlProgramm: {infoLogVert}");

            GL.CompileShader(FragmentShader);
            string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);
            if (infoLogFrag != System.String.Empty)
                Debug.WriteLine($"Error in fragment-shader compilation in  AbstrGlProgramm: {infoLogFrag}");

            var handle = GL.CreateProgram();

            if (!string.IsNullOrWhiteSpace(geometryShaderCode))
                AttachShader(new List<int>() { VertexShader, geometryShader, FragmentShader }, handle);
            else
                AttachShader(new List<int>() { VertexShader, FragmentShader }, handle);
            //GL.AttachShader(_handle, VertexShader);
            //GL.AttachShader(_handle, FragmentShader);

            GL.LinkProgram(handle);
            GL.ValidateProgram(handle);

            GL.DetachShader(handle, geometryShader);
            GL.DetachShader(handle, VertexShader);
            GL.DetachShader(handle, FragmentShader);
            if (!string.IsNullOrWhiteSpace(geometryShaderCode))
                GL.DeleteShader(geometryShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);

            return handle;
        }

        protected virtual void AttachShader(List<int> shaderHandles, int programHandle)
        {
            foreach (var shaderHandle in shaderHandles)
            {
                GL.AttachShader(programHandle, shaderHandle);
            }
        }

        public void UseVoxelShader(Vector3 cameraDirection)
        {
            var error = GL.GetError();
            if (error != ErrorCode.NoError)
                Debug.WriteLine($"{error} before using voxel shader");

            if (_currentShaderHandle != _voxelShaderHandle || _voxelShaderHandle == -1)
            {
                if (_voxelShaderHandle == -1)
                {
                    _voxelShaderHandle = CompileShader(VoxelShader.Shader, VoxelGeometryShader.Shader, FragmentShader.VoxelShader);
                    _cameraPositionPointer = GL.GetUniformLocation(_voxelShaderHandle, "cameraPosition");

                    error = GL.GetError();
                    if (error != ErrorCode.NoError)
                        Debug.WriteLine($"{error} After compiling and getting uniform location of stl shader \"cameraPosition\"");
                }

                GL.UseProgram(_voxelShaderHandle);

                error = GL.GetError();
                if (error != ErrorCode.NoError)
                    Debug.WriteLine($"{error} after GL.UseProgram");

                _currentShaderHandle = _voxelShaderHandle;



            }

            GL.Uniform3(_cameraPositionPointer, cameraDirection);
        }
    }
}
