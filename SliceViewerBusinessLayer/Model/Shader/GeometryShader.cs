namespace SliceViewerBusinessLayer.Model.Shader
{
    public static class GeometryShaderCode
    {
        public static string Shader =
            @"
#version 330 core
uniform mat4 Mvp; 
uniform vec3 cameraPosition;
layout (triangles) in;
layout (triangle_strip, max_vertices = 3) out;
in vec4 outColor[];
out vec4 fragcolor;

void main()
{
    vec4 p1 = gl_in[0].gl_Position;
    vec4 p2 = gl_in[1].gl_Position;
    vec4 p3 = gl_in[2].gl_Position;

    vec4 p12 = p2 - p1;
    vec4 p23 = p3 - p2;

    vec3 triangleDirection = cross(p12.xyz, p23.xyz);
    float lightingCoefficient = clamp(dot(cameraPosition, -triangleDirection), 0.0, 1.0);


    for(int i=0; i<3; i++)
    {
        fragcolor = outColor[i] * lightingCoefficient;
	    gl_Position = gl_in[i].gl_Position;
        EmitVertex();
    }
}
";
    }
}
