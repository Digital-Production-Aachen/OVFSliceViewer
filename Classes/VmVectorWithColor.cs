using OpenTK;

namespace LayerViewer
{
    public class VmVectorWithColor
    {
        public VmVectorWithColor(Vector3 position, float power, float minPower = 250, float maxPower = 350)
        {
            Position = position;

            if (maxPower - minPower <= 0.001f)
            {
                Color = new Vector4(1f, 0f, 0f, 0f);
            }
            else
            {
                var rgbR = 1 - (maxPower - power) / (maxPower - minPower);
                var rgbG = (maxPower - power) / (maxPower - minPower);
                Color = new Vector4(rgbR, rgbG, 0f, 0f);
            }
        }
        public VmVectorWithColor(Vector3 position, Vector4 color)
        {
            Position = position;
            Color = color;
        }

        public Vector3 Position { get; protected set; }
        public Vector4 Color { get; protected set; }
    }
}
