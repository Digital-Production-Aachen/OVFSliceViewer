using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenTK.Mathematics;
using AutomatedBuildChain.Proto;
using OpenTK.Graphics.OpenGL;
using OVFSliceViewerCore.Model.Scene.IrrlichtEngine;
using OVFSliceViewerCore.Model.Part;
using OVFSliceViewerCore.Model.RenderData;
using OVFSliceViewerCore.Reader;

namespace OVFSliceViewerCore.Model.Scene
{
    public class STLScene : IScene, IDisposable
    {
        public ISceneController SceneController { get; protected set; }

        private List<STLPart> PartsInScene = new List<STLPart>();
        public IEnumerable<STLPart> GetPartsInScene => PartsInScene.AsEnumerable();
        IEnumerable<AbstrPart> IScene.PartsInScene => PartsInScene.AsEnumerable();
        public SceneSettings SceneSettings { get; private set; } = new SceneSettings();

        Vector3 IScene.LastPosition => Vector3.Zero;

        public OVFFileInfo OVFFileInfo { get; set; } = new OVFFileInfo() { NumberOfWorkplanes = 1 };

        public STLScene(ISceneController sceneController)
        {
            SceneController = sceneController;
        }

        public STLScene()
        {
        }

        public async Task LoadFile(FileInfo fileInfo)
        {
            var reader = new STLReader();
            bool isLgdff = fileInfo.Extension.ToLower() == ".lgdff";
            Dictionary<LABEL, List<int>> labelMap = new Dictionary<LABEL, List<int>>();

            if (fileInfo.Extension.ToLower() == ".stl")
                await reader.ReadStl(fileInfo.FullName);

            else if (fileInfo.Extension.ToLower() == ".obj")
                await reader.ReadObj(fileInfo.FullName);

            else if (isLgdff)
            {
                await reader.ReadLgdff(fileInfo.FullName, labelMap);
            }

            var part = new STLPart(reader.Mesh, SceneController, () => SceneSettings.UseColorIndex);
            if (isLgdff)
            {
                part.FunctionalTriangleIDs = labelMap;
            }
            PartsInScene.Add(part);
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

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~STLScene()
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

        void IScene.CloseFile()
        {

        }

        void IScene.Render()
        {
            PartsInScene.ForEach(x => x.Render());
        }

        Vector2 IScene.GetCenter()
        {
            return Vector2.Zero;
        }

        Task IScene.LoadWorkplaneToBuffer(int index) { return Task.CompletedTask; }

        int IScene.GetNumberOfLinesInWorkplane() { return 1; }

        void IScene.ChangeNumberOfLinesToDraw(int numberOfLinesToDraw) { }

        public void ColorNearestHitTriangles(Vector2 position, float colorIndex, int radius, LABEL? label = null)
        {
            var ray = SceneCollisionManager.GetRayFromScreenCoordinates(position, SceneController.Camera, radius);

            foreach (var part in PartsInScene)
            {
                for (int i = 0; i < ray.Length - 1; i++) // for every ray direction
                {
                    // ray[0] is starting point
                    var triangle = part.FindNearestHitTriangle(ray[0], ray[i + 1]);
                    if (triangle != -1) // -1 is invalid triangle id
                    {
                        part.ColorTriangle(triangle, colorIndex);
                        part.SetLabelForTriangle(triangle, label);
                    }
                }

                part.RenderObjects[0].BindNewData();
            }
        }


    }
}
