using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System.Drawing;
using System;

namespace OVFSliceViewer
{
    public class CanvasWrapper : OVFSliceViewerBusinessLayer.Model.ICanvas
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
        }

        public void SwapBuffers()
        {
            _canvas.SwapBuffers();
        }
        public void MakeCurrent()
        {
            _canvas.MakeCurrent();
        }

        public void Resize(int width, int height)
        {
            if (_canvas.ClientSize.Height == 0)
                _canvas.ClientSize = new Size(width, height);
            
            GL.Viewport(_canvas.ClientSize);

        }
        public Tuple<int,int> GetCanvasArea()
        {
            return new Tuple<int, int>(_canvas.ClientSize.Width, _canvas.ClientSize.Height);
        }
        public float Width => _canvas.Width;

        public float Height => _canvas.Height;
    }

}
