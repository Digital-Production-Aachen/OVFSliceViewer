﻿using OpenTK;
using SliceViewerBusinessLayer.Model.STL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OVFSliceViewerBusinessLayer.Model
{
    public class STLScene : IScene, IDisposable
    {
        public ISceneController SceneController { get; protected set; }

        private List<STLPart> PartsInScene = new List<STLPart>();
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

            await reader.ReadStl(fileInfo.FullName);

            var part = new STLPart(reader.Mesh, SceneController, () => SceneSettings.UseColorIndex);
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

        void IScene.LoadWorkplaneToBuffer(int index){}

        int IScene.GetNumberOfLinesInWorkplane(){ return 1; }

        void IScene.ChangeNumberOfLinesToDraw(int numberOfLinesToDraw){}
    }
}
