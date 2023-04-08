#version 330 core
uniform mat4 Mvp; 
uniform int colorIndex;

layout(location = 0) in vec3 position;
layout(location = 1) in float powerCoefficient;
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
    //float colorIndex = 0;

    if (powerCoefficient <= 1.0)
    {
        colorOut = vec4(1, 0, 0, 0);
    }
    else
    {
        if (colorIndex == 0)
        {
            colorOut = vec4(1, 0, 0, 0);
        }
        else if (colorIndex == 1)
        {
            colorOut = vec4(0, 1, 0, 0);
        }
        else if (colorIndex == 2)
        {
            colorOut = vec4(0, 0, 1, 0);
        }
        else if (colorIndex == 3)
        {
            colorOut = vec4(0.5, 0.5, 0, 0);
        }
        else
        {
            colorOut = vec4(1, 0, 0, 0);
        }
        
    }
     //colormap(smoothstep(-0.5, 1.5, 1.-colorIndex));
        //mix(colorOut, colorMinPower, colorIndex);
	
	gl_Position = Mvp * vec4(position, 1); 
	fragcolor = colorOut; 
}