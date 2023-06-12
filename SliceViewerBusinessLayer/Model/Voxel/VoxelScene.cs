using OpenTK;
using SliceViewerBusinessLayer.Classes;
using SliceViewerBusinessLayer.Model.STL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenTK.Mathematics;
using AutomatedBuildChain.Proto;
using OVFSliceViewerCore.Model.Voxel;
using OpenTK.Graphics.OpenGL;

namespace OVFSliceViewerCore.Model
{
    public class VoxelScene : IScene, IDisposable
    {
        public ISceneController SceneController { get; protected set; }

        private List<VoxelPart> PartsInScene = new List<VoxelPart>();
        public IEnumerable<VoxelPart> GetPartsInScene => PartsInScene.AsEnumerable();
        IEnumerable<AbstrPart> IScene.PartsInScene => PartsInScene.AsEnumerable();
        public SceneSettings SceneSettings { get; private set; } = new SceneSettings();

        Vector3 IScene.LastPosition => Vector3.Zero;

        public OVFFileInfo OVFFileInfo { get; set; } = new OVFFileInfo() { NumberOfWorkplanes = 1 };

        public VoxelScene(ISceneController sceneController)
        {
            SceneController = sceneController;
        }

        public VoxelScene()
        {
        }

        public async Task LoadFile(FileInfo fileInfo)
        {
            var reader = new VoxelReader();
            Dictionary<LABEL, List<int>> labelMap = new Dictionary<LABEL, List<int>>();

            if (fileInfo.Extension.ToLower() == ".vx")
            {
                var voxel = await reader.ReadVoxel(fileInfo.FullName);
                var part = new VoxelPart(voxel.VoxelList_.Where(x=>x.ClusterID == -1).ToList(), SceneController, () => SceneSettings.UseColorIndex);
                part.Name = "part";
                var cluster = new VoxelPart(voxel.VoxelList_.Where(x => x.ClusterID != -1).ToList(), SceneController, () => SceneSettings.UseColorIndex);
                cluster.Name = "clusters";
                PartsInScene.Add(part);
                PartsInScene.Add(cluster);
            }                
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
        // ~VoxelScene()
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
            foreach(var part in PartsInScene)
            {
                if (part.IsActive)
                    part.Render();
            }
        }

        Vector2 IScene.GetCenter()
        {
            return Vector2.Zero;
        }

        Task IScene.LoadWorkplaneToBuffer(int index) { return Task.CompletedTask; }

        int IScene.GetNumberOfLinesInWorkplane() { return 1; }

        void IScene.ChangeNumberOfLinesToDraw(int numberOfLinesToDraw) { }

    }
}
