using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System.Drawing;
using System;
using OpenTK.WinForms;
using System.Windows.Forms;
using System.Collections.Generic;
using OVFSliceViewerCore.Model.Scene;

namespace OVFSliceViewer
{
    public class CanvasWrapper : ICanvas
    {
        GLControl _canvas;
        public GLControl Canvas => _canvas;

        Dictionary<Keys, bool> _pressedKeys = new Dictionary<Keys, bool>();

        public CanvasWrapper(GLControl gl)
        {
            _canvas = gl;
            _canvas.Show();
            //gl.PreviewKeyDown += KeyDown;
        }

        public void KeyDown(Object sender, KeyEventArgs e)
        {
            _pressedKeys[e.KeyCode] = true;
        }
        public void KeyUp(Object sender, KeyEventArgs e)
        {
            _pressedKeys[e.KeyCode] = false;
        }
        public bool IsKeyPressed(Keys key)
        {
            if (_pressedKeys.ContainsKey(key))
                return _pressedKeys[key];
            else
                return false;
        }

        public void Init()
        {
            MakeCurrent();
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
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
