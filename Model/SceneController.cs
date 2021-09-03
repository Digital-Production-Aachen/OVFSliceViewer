using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;

namespace LayerViewer.Model
{
    public class SceneController: ISceneController, IDisposable
    {
        public OVFSliceViewer.Camera Camera { get; protected set; }
        private ICanvas _canvas;

        public OVFScene Scene;// = new OVFScene();
        

        public SceneController(ICanvas canvas)
        {
            Camera = new OVFSliceViewer.Camera(this, canvas.Width, canvas.Height);
            _canvas = canvas;
        }

        public void Render()
        {
            if (Scene == null)
            {
                return;
            }
            _canvas.MakeCurrent();
            Scene.Render();
            _canvas.SwapBuffers();
            Console.WriteLine(GL.GetError());
        }
        
        public OVFScene LoadFile(string path)
        {
            var fileInfo = new FileInfo(path);

            var scene = new OVFScene(this);
            

            if (fileInfo.Extension.ToLower() == ".ovf")
            {
                scene.LoadOVF(fileInfo);
            }

            Scene = scene;
            return scene;
        }
        
        public List<string> GetPartNames()
        {
            return new List<string>() { "test" };
        }

        public void CenterView()
        {
            if (Scene == null)
            {
                return;
            }
            Camera.MoveToPosition2D(Scene.GetCenter());
        }


        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                Scene.Dispose();

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SceneController()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public interface ISceneController
    {
        OVFSliceViewer.Camera Camera { get; }
    }
}
