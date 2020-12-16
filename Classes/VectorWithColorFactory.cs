
using OpenTK;

namespace OVFSliceViewer
{
    public class VectorWithColorFactory
    {
        float _minPower;
        float _maxPower;
        Vector4 _color;

        public VectorWithColorFactory(float minPower, float maxPower)
        {
            _minPower = minPower;
            _maxPower = maxPower;
        }

        public void SetColor(Vector4 color)
        {
            _color = color; 
        }
        public VmVectorWithColor GetVectorWithColor(Vector3 position)
        {
            return new VmVectorWithColor(position, _color);
        }
        public VmVectorWithColor GetVectorWithPowerColor(Vector3 position, float power)
        {
            return new VmVectorWithColor(position, power, _minPower, _maxPower);
        }

    }
}
