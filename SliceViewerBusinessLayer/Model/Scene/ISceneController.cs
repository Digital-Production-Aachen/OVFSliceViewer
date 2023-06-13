using OVFSliceViewerCore.Model.Part;
using OVFSliceViewerCore.Model.Scene;
using System.Collections.Generic;

namespace OVFSliceViewerCore.Model
{
    public interface ISceneController
    {
        Camera Camera { get; }

        List<AbstrPart> GetParts();
       
    }
}
