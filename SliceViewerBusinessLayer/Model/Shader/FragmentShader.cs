namespace SliceViewerBusinessLayer.Model.Shader
{
    public static class FragmentShader
    {
        public static string Shader =>
            @"
#version 150 core
in vec4 fragcolor;
out vec4 FragColor; 
void main() 
{ 
	FragColor = fragcolor;
}
        ";
    }
}
