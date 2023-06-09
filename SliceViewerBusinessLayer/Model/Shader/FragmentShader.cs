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
float colormap_red(float x) {
    if (x < 0.7) {
        return 4.0 * x - 1.5;
    } else {
        return -4.0 * x + 4.5;
    }
}

float colormap_green(float x) {
    if (x < 0.5) {
        return 4.0 * x - 0.5;
    } else {
        return -4.0 * x + 3.5;
    }
}

float colormap_blue(float x) {
    if (x < 0.3) {
       return 4.0 * x + 0.5;
    } 
    else {
       return -4.0 * x + 2.5;
    }
}

vec4 colormap(float x) {
    float r = clamp(colormap_red(x), 0.0, 1.0);
    float g = clamp(colormap_green(x), 0.0, 1.0);
    float b = clamp(colormap_blue(x), 0.0, 1.0);
    return vec4(r, g, b, 0.8);
}

void main() 
{ 
	FragColor = colormap(fragcolor);
}

";
    }
}
