using System.Collections.Generic;

namespace LayerViewer.Model
{
    public class AbstrPart
    {
        public bool IsActive;
        public List<RenderObject> RenderObjects { get; protected set; }
    }
}
