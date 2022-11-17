// Modified file from https://github.com/gradientspace/geometry3Sharp

using OpenTK;

namespace Geometry3SharpLight
{
    public struct Line3f
    {
        public Vector3 Origin;
        public Vector3 Direction;

        public Line3f(Vector3 origin, Vector3 direction)
        {
            this.Origin = origin;
            this.Direction = direction;
        }

        // parameter is distance along Line
        public Vector3 PointAt(float d)
        {
            return Origin + d * Direction;
        }

        public float Project(Vector3 p)
        {
            return Vector3.Dot((p - Origin), Direction);
        }

        public float DistanceSquared(Vector3 p)
        {
            float t = Vector3.Dot((p - Origin), Direction);
            Vector3 proj = Origin + t * Direction;
            return (proj - p).LengthSquared;
        }

        public Vector3 ClosestPoint(Vector3 p)
        {
            float t = Vector3.Dot((p - Origin), Direction);
            return Origin + t * Direction;
        }
    }
}
