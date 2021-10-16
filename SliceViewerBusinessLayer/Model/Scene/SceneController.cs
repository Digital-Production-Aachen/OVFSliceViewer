using OpenTK.Graphics.OpenGL;
using OVFSliceViewerBusinessLayer.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OVFSliceViewerBusinessLayer.Model
{
    public class SceneController: ISceneController, IDisposable
    {
        public Camera Camera { get; protected set; }
        public OVFScene Scene { get; protected set; }

        private ICanvas _canvas;

        public SceneController(ICanvas canvas)
        {
            Camera = new Camera(this, canvas.Width, canvas.Height);
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
        }
        public async Task<OVFScene> LoadFile(string path)
        {
            var fileInfo = new FileInfo(path);

            if (Scene != null)
            {
                CloseFile();
            }

            var scene = new OVFScene(this);

            if (fileInfo.Extension.ToLower() == ".ovf")
            {
                await scene.LoadFile(fileInfo);
            }

            Scene = scene;
            return scene;
        }

        public void CloseFile()
        {
            Scene.CloseFile();
            Clear();
        }
        private void Clear()
        {
            if (Scene == null)
            {
                return;
            }
            _canvas.MakeCurrent();
            GL.Clear(ClearBufferMask.ColorBufferBit);
            _canvas.SwapBuffers();
        }
        //public STLScene LoadFile(string path)
        //{
        //    var fileInfo = new FileInfo(path);

        //    var scene = new STLScene(this);


        //    if (fileInfo.Extension.ToLower() == ".stl")
        //    {
        //        scene.LoadSTL(fileInfo);
        //    }

        //    Scene = scene;
        //    return scene;
        //}

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

        public List<AbstrPart> GetParts()
        {
            var temp = Scene.PartsInScene.Values.ToList();
            return temp.Select(x => (AbstrPart)x).ToList();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                Scene?.Dispose();

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
        Camera Camera { get; }

        List<AbstrPart> GetParts();
    }
}
