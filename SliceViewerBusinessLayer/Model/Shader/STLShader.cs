namespace SliceViewerBusinessLayer.Model.Shader
{
    public static class STLShader
    {
        public static string Shader =
            @"
#version 330 core
uniform mat4 Mvp; 
uniform int colorIndex;
uniform vec3 cameraPosition;

layout(location = 0) in vec3 position;
layout(location = 1) in float powerCoefficient;
out vec4 outColor;

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
    return vec4(r, g, b, 1.0);
}

void main()
{
	vec4 colorOut = vec4(1,0,0,0);
	vec4 colorMinPower = vec4(0,1,0,0);

    if(powerCoefficient > 0)
    {
        colorOut = colormap(powerCoefficient);
    }
    else
    {
        colorOut = colormap(colorIndex / 6.0);
    }
  
    //colorOut = colormap(colorIndex / 6.0);
	gl_Position = Mvp * vec4(position, 1); 
    outColor = colorOut;
}
";
    }
}
