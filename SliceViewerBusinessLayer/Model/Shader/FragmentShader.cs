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


        public static string VoxelShader => @"
#version 150 core
in float fragcolor;
out vec4 FragColor; 

vec3 palette(float t){
    vec3 a = vec3(0.5);
    vec3 b = vec3(1.0);
    vec3 c = vec3(1.0);
    vec3 d = vec3(0.0, 0.333, 0.667);
    return a+ b*cos(6.28318*(c*t+d));
}

void main() 
{ 
	FragColor = vec4(palette(fragcolor),1.0);
}

";
    }
}
