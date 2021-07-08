using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;

namespace LayerViewer.Model
{
    public class SceneController: IScene
    {
        public OVFSliceViewer.Camera Camera { get; protected set; }
        protected OVFFileLoader _ovfFileLoader;
        public Dictionary<int, AbstrPart> PartsInViewer;
        
        public SceneController(ICanvas canvas)
        {
            Camera = new OVFSliceViewer.Camera(canvas.Width, canvas.Height);
        }

        public void RenderAllParts()
        {
            foreach (var part in PartsInViewer.Values)
            {
                part.RenderObjects.ForEach(y => y.Render());
            }
        }
        
        public void LoadFile(string path)
        {
            var fileInfo = new FileInfo(path);

            if (fileInfo.Extension.ToLower() == ".ovf")
            {
                _ovfFileLoader = new OVFFileLoader(fileInfo);
            }
        }

        public void LoadWorkplaneToBuffer(int index)
        {
            var workplane = _ovfFileLoader.GetWorkplane(index);

            foreach (var part in PartsInViewer.Values)
            {
                if (part is OVFPart)
                {
                    var ovfPart = part as OVFPart;
                    ovfPart.AddWorkplane(workplane);
                }
            }
        }

        public List<string> GetPartNames()
        {
            return new List<string>() { "test" };
        }
    }

    public interface IScene
    {
        OVFSliceViewer.Camera Camera { get; }
    }
}
