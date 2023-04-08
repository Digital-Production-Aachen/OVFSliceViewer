using OVFSliceViewerBusinessLayer.Classes;
using System.Collections.Generic;

namespace OVFSliceViewerBusinessLayer.Model
{
    public interface ISceneController
    {
        Camera Camera { get; }

        List<AbstrPart> GetParts();
       
    }
}
