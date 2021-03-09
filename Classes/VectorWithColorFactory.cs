
using OpenTK;

namespace OVFSliceViewer
{
    public class VectorWithColorFactory
    {
        float _minPower;
        float _maxPower;
        float _colorIndex;

        public VectorWithColorFactory(float minPower, float maxPower)
        {
            _minPower = minPower;
            _maxPower = maxPower;
        }

        public void SetColor(float color)
        {
            _colorIndex = color; 
        }
        public VmVectorWithColor GetVectorWithColor(Vector3 position)
        {
            return new VmVectorWithColor(position, _colorIndex);
        }
        public VmVectorWithColor GetVectorWithPowerColor(Vector3 position, float power)
        {
            return new VmVectorWithColor(position, power, _minPower, _maxPower);
        }

    }
}
