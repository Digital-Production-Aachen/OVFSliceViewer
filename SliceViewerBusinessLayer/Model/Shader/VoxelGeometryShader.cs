namespace SliceViewerBusinessLayer.Model.Shader
{
    public static class VoxelGeometryShader
    {
        public static string Shader =
            @"
#version 330 core

layout (lines) in;
in float vertexColor[];
layout (triangle_strip, max_vertices = 36) out;
out float fragcolor;

uniform mat4 Mvp;

void main()
{
    // Berechnung der maximalen Koordinate des Voxels
    
mat4 invertedMatrix = inverse(Mvp);
vec3 voxelMax = (gl_in[0].gl_Position + gl_in[1].gl_Position).xyz;
vec3 voxelMin = (gl_in[0].gl_Position).xyz;

    // Definition der Eckpunkte des Voxels
    vec3 cubeVertices[8];

    cubeVertices[0] = voxelMin;
    cubeVertices[1] = vec3(voxelMax.x, voxelMin.y, voxelMin.z);
    cubeVertices[2] = vec3(voxelMin.x, voxelMax.y, voxelMin.z);
    cubeVertices[3] = vec3(voxelMax.x, voxelMax.y, voxelMin.z);
    cubeVertices[4] = vec3(voxelMin.x, voxelMin.y, voxelMax.z);
    cubeVertices[5] = vec3(voxelMax.x, voxelMin.y, voxelMax.z);
    cubeVertices[6] = voxelMax;
    cubeVertices[7] = vec3(voxelMin.x, voxelMax.y, voxelMax.z);
    

    int indices[36] = int[36](
        //hinten
        0, 1, 3,
        0, 3, 2,

        //links
        0, 2, 7,
        0, 7, 4,
        
        //boden
        0, 1, 5,
        0, 5, 4,
        
        //vorne
        4, 5, 6,
        4, 6, 7,

        // oben
        2, 6, 7,
        2, 6, 3,
        
        //rechts
        1, 6, 3,
        1, 5, 6
    );

    for (int i = 0; i < 36; ++i)
    {
        int vertexIndex = indices[i];
        vec4 position = Mvp * vec4(cubeVertices[vertexIndex], 1.0);
        gl_Position = position;
        fragcolor = vertexColor[0] / vertexColor[1];
        EmitVertex();
if(i % 3 == 2){
    EndPrimitive();
}
    }
}
";
    }
}
