using OpenTK;

namespace OVFSliceViewer
{
    public class VmVectorWithColor
    {
        public VmVectorWithColor(Vector3 position, float power, float minPower = 250, float maxPower = 350)
        {
            Position = position;

            if (maxPower - minPower <= 0.001f)
            {
                Color = 0;
            }
            else
            {
                var rgbG = (maxPower - power) / (maxPower - minPower);
                Color = rgbG;
            }
        }
        public VmVectorWithColor(Vector3 position, float color)
        {
            Position = position;
            Color = color;
        }

        public Vector3 Position { get; protected set; }
        public float Color { get; protected set; }
    }
}
