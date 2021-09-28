using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenVectorFormat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace OVFSliceViewerBusinessLayer.Model
{
    //ToDo new Class SceneState which handles currentworkplane, state etc.
    public class OVFScene : IScene, IDisposable
    {
        public ISceneController SceneController { get; protected set; }
        public OVFScene(ISceneController sceneController)
        {
            SceneController = sceneController;
        }
        protected OVFFileLoader _ovfFileLoader;
        public OVFFileInfo OVFFileInfo { get; protected set; }

        public Dictionary<int, OVFPart> PartsInScene = new Dictionary<int, OVFPart>();

        public SceneSettings SceneSettings { get; private set; } = new SceneSettings();

        public int NumberOfLinesInWorkplane => OVFFileInfo.NumberOfVerticesInWorkplane[SceneSettings.CurrentWorkplane];
        private bool _stateChanged = false;

        public List<KeyValuePair<int, int>> LinesInPart { get; set; } = new List<KeyValuePair<int, int>>();

        public void LoadFile(FileInfo fileInfo)
        {
            PartsInScene.Values.ToList().ForEach(x => x.Dispose());
            PartsInScene.Clear();
            _ovfFileLoader = new OVFFileLoader(fileInfo);
            OVFFileInfo = _ovfFileLoader.OVFFileInfo;

            foreach (var partKey in OVFFileInfo.PartKeys)
            {
                PartsInScene.Add(partKey, new OVFPart(partKey, OVFFileInfo.PartNamesMap[partKey], SceneController, () => SceneSettings.UseColorIndex));
            }
        }
        public void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.LineWidth(5f);

            if (_stateChanged)
            {
                foreach (var part in PartsInScene.Values)
                {
                    part.ResetNumberOfLinesToDraw();
                }

                var vectorBlockInfos = OVFFileInfo.GetVectorblockDisplayData(SceneSettings.CurrentWorkplane);

                int numberOfVertices = 0;
                for (int i = 0; i < vectorBlockInfos.Count; i++)
                {
                    if (numberOfVertices >= SceneSettings.CurrentNumberOfDrawnLines)
                    {
                        break;
                    }

                    var info = vectorBlockInfos[i];

                    PartsInScene[info.PartKey].IncreaseNumberOfLinesToDraw(Math.Min(SceneSettings.CurrentNumberOfDrawnLines - numberOfVertices, info.NumberOfVertices));
                    numberOfVertices += info.NumberOfVertices;
                }

                _stateChanged = false;

                Debug.WriteLine("Seems like something went wrong here.");
            }

            foreach (var part in PartsInScene.Values)
            {
                if (part.IsActive)
                {
                    part.RenderObjects.ForEach(y => y.Render());
                }
            }


            Debug.WriteLine(GL.GetError());
            Debug.WriteLine(GL.GetError());
        }
        public void LoadWorkplaneToBuffer(int index)
        {
            ResetLinesInPart();
            ResetScene();
            if (SceneSettings.ShowAs3dObject) AddContourWorkplanes(index);
            AddIndexWorkplane(index);
        }
        private void AddContourWorkplanes(int index)
        {
            for (int i = 0; i < index; i++)
            {
                var workplaneShell = _ovfFileLoader.GetWorkplaneShell(i);

                for (int j = 0; j < OVFFileInfo.ContourVectorblocksInWorkplaneLUT[i].Count; j++)
                {
                    var vectorblock = _ovfFileLoader.GetVectorBlock(i, j);
                    AddVectorblockToParts(vectorblock, workplaneShell.ZPosInMm);
                }
            }
        }
        private void AddIndexWorkplane(int index)
        {
            var workplane = _ovfFileLoader.GetWorkplane(index);
            foreach (var vectorblock in workplane.VectorBlocks)
            {
                AddVectorblockToParts(vectorblock, workplane.ZPosInMm);
            }

            BindDataToAllParts();
            SceneSettings.CurrentNumberOfDrawnLines = NumberOfLinesInWorkplane;
            SceneSettings.CurrentWorkplane = index;
        }
        public void ChangeNumberOfLinesToDraw(int numberOfLinesToDraw)
        {
            SceneSettings.CurrentNumberOfDrawnLines = numberOfLinesToDraw;
            _stateChanged = true;
        }
        protected void ResetScene()
        {
            foreach (var part in PartsInScene.Values)
            {
                part.Reset();
            }
        }

        protected void BindDataToAllParts()
        {
            foreach (var part in PartsInScene.Values)
            {
                part.BindData();
            }
        }
        protected void AddVectorblockToParts(VectorBlock vectorblock, float height)
        {
            var part = vectorblock.MetaData.PartKey;

            var ovfPart = PartsInScene[part] as OVFPart;
            LinesInPart.Add(new KeyValuePair<int, int>(part, ovfPart.AddVectorblock(vectorblock, height)));
        }

        protected void ResetLinesInPart()
        {
            LinesInPart.Clear();
        }
        public Vector2 GetCenter()
        {
            Vector3 min = new Vector3(float.MaxValue);
            Vector3 max = -min;

            foreach (var part in PartsInScene.Values)
            {
                min = Vector3.ComponentMin(part.BoundingBox.Min, min);
                max = Vector3.ComponentMax(part.BoundingBox.Max, max);
            }

            var center = min + (max - min) / 2;
            return center.Xy;
            //SceneController.MoveToPosition2D(center.Xy);
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

                foreach (var part in PartsInScene.Values)
                {
                    part.Dispose();
                }
                PartsInScene = null;

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~OVFScene()
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

    public interface IScene
    {
        void LoadFile(FileInfo fileInfo);
    }
}
