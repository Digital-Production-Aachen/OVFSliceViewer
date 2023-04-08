namespace LayerViewer.Model
{
    public interface ICanvas
    {
        void SwapBuffers();
        void MakeCurrent();
        
        void Resize(System.Drawing.Size size);

        System.Drawing.Size GetCanvasArea();
        float Width { get; }
        float Height { get; }
    }
}
