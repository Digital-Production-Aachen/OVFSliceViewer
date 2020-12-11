namespace LayerViewer
{
    public class VmVectorWithPower
    {
        public VmVectorWithPower(float x, float y, float power, float minPower = 250, float maxPower = 350)
        {
            X = x;
            Y = y;
            Power = power;
            MinPower = minPower;
            MaxPower = maxPower;
        }
        public float X { get; set; }
        public float Y { get; set; }
        public float Power { get; set; }
        public float MinPower { get; set; }
        public float MaxPower { get; set; }

        public float RgbR => 1 - (MaxPower - Power) / (MaxPower - MinPower);

        public float RgbB => (MaxPower - Power) / (MaxPower - MinPower);
    }
}
