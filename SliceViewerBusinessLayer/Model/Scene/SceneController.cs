using OpenTK.Graphics.OpenGL;
using OVFSliceViewerCore.Classes;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OpenVectorFormat.AbstractReaderWriter;
using OVFSliceViewerCore.Model.Voxel;
using static System.Formats.Asn1.AsnWriter;

namespace OVFSliceViewerCore.Model
{
    public class SceneController: ISceneController, IDisposable
    {
        public Camera Camera { get; protected set; }
        public List<IScene> Scenes { get; protected set; } = new List<IScene>();

        private ICanvas _canvas;

        public SceneController(ICanvas canvas)
        {
            Camera = new Camera(this, canvas.Width, canvas.Height);
            _canvas = canvas;
        }

        public void Render()
        {
            if (Scenes.Count() == 0)
            {
                return;
            }
            
            _canvas.MakeCurrent();
            
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.ShadeModel(ShadingModel.Smooth);

            try
            {
                Scenes.ForEach(x => x.Render());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                throw;
            }

            
            _canvas.SwapBuffers();
        }
        public async Task<IScene> LoadFile(FileReader file)
        {
            var scene = new OVFScene(this);
            await scene.LoadFile(file);
            Scenes.Add(scene);
            return scene;
        }
        public async Task<IScene> LoadFile(string path)
        {
            var fileInfo = new FileInfo(path);
            //if (Scene != null)
            //{
            //    CloseFile();
            //}
            IScene scene;
            if (fileInfo.Extension.ToLower() == ".vx")
            {
                scene = new VoxelScene(this);
                await scene.LoadFile(fileInfo);
            }
            else if (FileHasNoParts(fileInfo))
            {
                scene = new STLScene(this);
                await scene.LoadFile(fileInfo);

                float maxSize = 0;
                foreach (STLPart part in scene.PartsInScene)
                {
                    maxSize = Math.Max(part.maxSize, maxSize); // look far largest part
                }

                // set zFar plane so that even largest part can bee seen
                Camera.zFar = Math.Max(maxSize * 3f, 100f);
                // set start position of camera
                Camera.setCameraPosition(0, 0, maxSize);
            }
            else
            {
                scene = new OVFScene(this);

                //if (fileInfo.Extension.ToLower() == ".ovf" || fileInfo.Extension.ToLower() == ".gcode")
                //{
                await scene.LoadFile(fileInfo);
                //}
            }

            Scenes.Add(scene);
            return scene;
        }

        public void CloseFile()
        {
            foreach (var scene in Scenes)
            {
                scene.CloseFile();
            }
            Clear();
        }
        private void Clear()
        {
            if (Scenes.Count == 0)
            {
                return;
            }
            _canvas.MakeCurrent();
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);
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
            if (Scenes.Count == 0)
            {
                return;
            }
            Camera.MoveToPosition2D(Scenes.FirstOrDefault().GetCenter());
        }


        private bool disposedValue;

        public List<AbstrPart> GetParts()
        {
            var temp = Scenes.Select(x => x.PartsInScene).SelectMany(x => x).ToList();
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

                foreach (var scene in Scenes)
                {
                    scene.Dispose();
                }

                

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

        public static bool FileHasNoParts(FileInfo file)
        {
            return file.Extension.ToLower() == ".stl" || file.Extension.ToLower() == ".obj" || file.Extension.ToLower() == ".lgdff";
        }
    }
}
