﻿namespace OVFSliceViewerBusinessLayer.Model
{
    public class SceneSettings
    {
        public bool ShowAs3dObject { get; set; } = false;
        public bool UseColorIndex { get; set; } = true;
        public int CurrentWorkplane { get; set; }
        public int CurrentNumberOfDrawnLines { get; set; }
    }
}
