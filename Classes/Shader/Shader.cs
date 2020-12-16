using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Text;

namespace OVFSliceViewer.Classes.Shader
{
    public class Shader
    {
        string _vertexShaderSource;
        string _fragmentShaderSource;
        public Shader(string vertexPath, string fragmentPath)
        {
            using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
            {
                _vertexShaderSource = reader.ReadToEnd();
            }

            using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8))
            {
                _fragmentShaderSource = reader.ReadToEnd();
            }
        }
        public int CreateVertexShader()
        {
            var shader = GL.CreateShader(ShaderType.VertexShader);

            GL.ShaderSource(shader, _vertexShaderSource);
            GL.CompileShader(shader);
            //string infoLogVert = GL.GetShaderInfoLog(shader); // use for debug

            return shader;
        }

        public int CreateFragmentShader()
        {
            var shader = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(shader, _fragmentShaderSource);
            GL.CompileShader(shader);
            string infoLogVert = GL.GetShaderInfoLog(shader);

            return shader;
        }

    }
}
