// Modified file from https://github.com/gradientspace/geometry3Sharp

using OpenTK;

namespace Geometry3SharpLight
{
    public struct Ray3f
    {
        public Vector3 Origin;
        public Vector3 Direction;

        public Ray3f(Vector3 origin, Vector3 direction, bool bIsNormalized = false)
        {
            this.Origin = origin;
            this.Direction = direction;
            if (bIsNormalized == false)
                Direction.Normalize();
        }

        // parameter is distance along ray
        public Vector3 PointAt(float d)
        {
            return Origin + d * Direction;
        }

        public float Project(Vector3 p)
        {
            return Vector3.Dot((p - Origin), (Direction));
        }

        public float DistanceSquared(Vector3 p)
        {
            float t = Vector3.Dot((p - Origin), Direction);
            Vector3 proj = Origin + t * Direction;
            return (proj - p).LengthSquared;
        }
    }
}
