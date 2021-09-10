using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenVectorFormat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LayerViewer.Model
{

    //ToDo new Class SceneState which handles currentworkplane, state etc.
    public class OVFScene: IDisposable
    {
        public ISceneController SceneController { get; protected set; }
        public OVFScene(ISceneController sceneController)
        {
            SceneController = sceneController;
        }
        protected OVFFileLoader _ovfFileLoader;
        public OVFFileInfo OVFFileInfo { get; protected set; }

        public Dictionary<int, OVFPart> PartsInScene = new Dictionary<int, OVFPart>();
        

        public int NumberOfLinesInWorkplane => OVFFileInfo.NumberOfVerticesInWorkplane[CurrentWorkplane];
        public int CurrentWorkplane { get; private set; }
        public int CurrentNumberOfDrawnLines { get; private set; }
        private bool _stateChanged = false;

        public List<KeyValuePair<int, int>> LinesInPart { get; set; } = new List<KeyValuePair<int, int>>();

        public void LoadOVF(FileInfo fileInfo)
        {
            PartsInScene.Values.ToList().ForEach(x => x.Dispose());
            PartsInScene.Clear();
            _ovfFileLoader = new OVFFileLoader(fileInfo);
            OVFFileInfo = _ovfFileLoader.OVFFileInfo;

            foreach (var partKey in OVFFileInfo.PartKeys)
            {
                PartsInScene.Add(partKey, new OVFPart(partKey, OVFFileInfo.PartNamesMap[partKey], SceneController));
            }
        }
        public void Render()
        {
            //GL.ClearColor(Color4.LightGray);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.LineWidth(5f);

            if (_stateChanged)
            {
                foreach (var part in PartsInScene.Values)
                {
                    part.ResetNumberOfLinesToDraw();
                }

                var vectorBlockInfos = OVFFileInfo.GetVectorblockDisplayData(CurrentWorkplane);

                int numberOfVertices = 0;
                for (int i = 0; i < vectorBlockInfos.Count; i++)
                {
                    if (numberOfVertices >= CurrentNumberOfDrawnLines)
                    {
                        break;
                    }

                    var info = vectorBlockInfos[i];

                    PartsInScene[info.PartKey].IncreaseNumberOfLinesToDraw(Math.Min(CurrentNumberOfDrawnLines-numberOfVertices, info.NumberOfVertices));
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
            var workplane = _ovfFileLoader.GetWorkplane(index);

            foreach (var vectorblock in workplane.VectorBlocks)
            {
                AddVectorblockToParts(vectorblock, workplane.ZPosInMm);
            }
            BindDataToAllParts();
            CurrentWorkplane = index;
            CurrentNumberOfDrawnLines = NumberOfLinesInWorkplane;
        }
        public void ChangeNumberOfLinesToDraw(int numberOfLinesToDraw)
        {
            CurrentNumberOfDrawnLines = numberOfLinesToDraw;
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
}
