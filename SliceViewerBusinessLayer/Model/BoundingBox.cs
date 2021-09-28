﻿using OpenTK;
using System.Collections.Generic;

namespace OVFSliceViewerBusinessLayer.Model
{
    public class BoundingBox
    {
        public Vector3 Min { get; private set; }
        public Vector3 Max { get; private set; }

        public BoundingBox()
        {
            Reset();
        }

        public void Reset()
        {
            Min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Max = -Min;
        }
        public void AddVertex(Vector3 vertex)
        {
            Min = Vector3.ComponentMin(Min, vertex);
            Max = Vector3.ComponentMax(Max, vertex);
        }
        public void AddVertices(IList<OVFSliceViewerBusinessLayer.Classes.Vertex> vertices)
        {
            foreach (var vertex in vertices)
            {
                AddVertex(vertex.Position);
            }
        }
    }
}
