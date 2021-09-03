using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenVectorFormat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LayerViewer.Model
{
    public class OVFScene: IDisposable
    {
        public ISceneController SceneController { get; protected set; }
        public OVFScene(ISceneController sceneController)
        {
            SceneController = sceneController;
        }
        protected OVFFileLoader _ovfFileLoader;

        public Dictionary<int, OVFPart> PartsInViewer = new Dictionary<int, OVFPart>();
        private bool disposedValue;

        public int NumberOfWorkplanes { get; set; }
        public int NumberOfLinesInWorkplane { get; set; }
        public int CurrentWorkplane { get; set; }
        public int CurrentNumberOfDrawnLines { get; set; }

        public List<KeyValuePair<int, int>> LinesInPart { get; set; } = new List<KeyValuePair<int, int>>();

        public void LoadOVF(FileInfo fileInfo)
        {
            PartsInViewer.Clear();
            _ovfFileLoader = new OVFFileLoader(fileInfo);
            var parts = _ovfFileLoader.GetPartsList();
            foreach (var part in parts)
            {
                PartsInViewer.Add(part, new OVFPart(part, SceneController));
            }
            NumberOfWorkplanes = _ovfFileLoader.NumberOfWorkplanes;
        }

        public void Render()
        {
            //GL.ClearColor(Color4.LightGray);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.LineWidth(5f);

            foreach (var part in PartsInViewer.Values)
            {
                if (part.IsActive)
                {
                    part.RenderObjects.ForEach(y => y.Render());
                }
            }
            Console.WriteLine(GL.GetError());
            Console.WriteLine(GL.GetError());
        }

        public void LoadWorkplaneToBuffer(int index)
        {
            ResetLinesInPart();
            ResetParts();
            var workplane = _ovfFileLoader.GetWorkplane(index);

            foreach (var vectorblock in workplane.VectorBlocks)
            {
                AddVectorblockToParts(vectorblock, workplane.ZPosInMm);
            }
            BindDataToAllParts();
            CurrentWorkplane = index;
            NumberOfLinesInWorkplane = LinesInPart.Sum(x => x.Value);
        }
        protected void ResetParts()
        {
            foreach (var part in PartsInViewer.Values)
            {
                part.Reset();
            }
        }

        protected void BindDataToAllParts()
        {
            foreach (var part in PartsInViewer.Values)
            {
                part.BindData();
            }
        }
        protected void AddVectorblockToParts(VectorBlock vectorblock, float height)
        {
            var part = vectorblock.MetaData.PartKey;

            var ovfPart = PartsInViewer[part] as OVFPart;
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

            foreach (var part in PartsInViewer.Values)
            {
                min = Vector3.ComponentMin(part.BoundingBox.Min, min);
                max = Vector3.ComponentMax(part.BoundingBox.Max, max);
            }

            var center = min + (max - min) / 2;
            return center.Xy;
            //SceneController.MoveToPosition2D(center.Xy);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                foreach (var part in PartsInViewer.Values)
                {
                    part.Dispose();
                }
                PartsInViewer = null;

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
