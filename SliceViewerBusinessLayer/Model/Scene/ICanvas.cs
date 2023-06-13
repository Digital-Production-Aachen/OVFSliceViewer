using System;

namespace OVFSliceViewerCore.Model.Scene
{
    public interface ICanvas
    {
        void SwapBuffers();
        void MakeCurrent();

        void Resize(int width, int height);

        Tuple<int, int> GetCanvasArea();
        float Width { get; }
        float Height { get; }
    }
}
