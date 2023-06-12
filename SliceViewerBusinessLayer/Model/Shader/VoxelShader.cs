namespace SliceViewerBusinessLayer.Model.Shader
{
    public static class VoxelShader
    {
        public static string Shader =
            @"
#version 330 core

uniform mat4 Mvp; 
layout (location = 0) in vec3 position;
layout (location = 1) in float color;
out float vertexColor;

void main()
{
    gl_Position = vec4(position, 1); 
    vertexColor = color; // Übergebe das Vertex-Attribut an den Geometry Shader
}
";
    }
}
