using SliceViewerBusinessLayer.Model.STL;
using System.Collections.Generic;

namespace OVFSliceViewerBusinessLayer.Model
{
    
    public class STLPart
    {
        private List<Triangle> _triangles;

        public STLPart(List<Triangle> triangles)
        {
            _triangles = triangles;
        }

    }
}
