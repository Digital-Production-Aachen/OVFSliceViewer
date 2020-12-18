#version 150 core
uniform mat4 Mvp; 
in vec4 color; 
in vec3 position; 
out vec4 fragcolor; 
void main() {
	gl_Position = Mvp * vec4(position, 1); 
	fragcolor = color; 
}