#version 150 core
uniform mat4 Mvp; 
uniform vec4 mainColor;
uniform vec4 contourColor;
uniform vec4 supportColor;

in float colorIndex;
in vec3 position; 
out vec4 fragcolor; 

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
	if(colorIndex <= 1)
	{
		colorOut = colormap(smoothstep(-0.5, 1.5, 1.-colorIndex));
        //mix(colorOut, colorMinPower, colorIndex);
	}
	else if(colorIndex < 3 )
	{
		colorOut = contourColor;
	}
	else if(colorIndex < 4 )
	{
		colorOut = supportColor;
	}
	gl_Position = Mvp * vec4(position, 1); 
	fragcolor = colorOut; 
}