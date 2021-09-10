using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace OVFSliceViewer
{
    public class CanvasWrapper : LayerViewer.Model.ICanvas
    {
        GLControl _canvas;
        public CanvasWrapper(GLControl gl)
        {
            _canvas = gl;
            _canvas.Show();
        }
        public void Init()
        {
            MakeCurrent();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            //GL.ClearColor(232f/255f, 241f/255f, 250f/255f, 1.0f);
            //GL.ClearColor(new Color4(45, 127, 131, 255));
        }

        public void SwapBuffers()
        {
            _canvas.SwapBuffers();
        }
        public void MakeCurrent()
        {
            _canvas.MakeCurrent();
        }

        public void Resize(System.Drawing.Size size)
        {
            if (_canvas.ClientSize.Height == 0)
                _canvas.ClientSize = size;

            GL.Viewport(size);

        }
        public System.Drawing.Size GetCanvasArea()
        {
            return _canvas.ClientSize;
        }
        public float Width => _canvas.Width;

        public float Height => _canvas.Height;
    }

}
