#version 150 core
uniform mat4 Mvp; 
uniform vec4 mainColor;
uniform vec4 contourColor;
uniform vec4 supportColor;

in float colorIndex;
in vec3 position; 
out vec4 fragcolor; 
void main()
{
	vec4 colorOut = vec4(1,0,0,0);
	vec4 colorMinPower = vec4(0,1,0,0);
	if(colorIndex <= 1)
	{
		colorOut = mix(colorOut, colorMinPower, colorIndex);
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