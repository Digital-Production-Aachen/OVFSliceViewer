// Modified file from https://github.com/gradientspace/geometry3Sharp

using OpenTK;
using OpenTK.Mathematics;

namespace Geometry3SharpLight
{
    class IntrRay3AxisAlignedBox3
    {
        /// <summary>
        /// Find intersection of ray with AABB, without having to construct any new classes.
        /// Returns ray T-value of first intersection (or double.MaxVlaue on miss)
        /// </summary>
        public static bool FindRayIntersectT(ref Ray3f ray, ref AxisAlignedBox3f box, out double RayParam)
        {
            double RayParam0 = 0.0;
            double RayParam1 = double.MaxValue;
            int Quantity = 0;
            Vector3 Point0 = Vector3.Zero;
            Vector3 Point1 = Vector3.Zero;
            IntersectionType Type = IntersectionType.Empty;
            IntrLine3AxisAlignedBox3.DoClipping(ref RayParam0, ref RayParam1, ref ray.Origin, ref ray.Direction, ref box,
                      true, ref Quantity, ref Point0, ref Point1, ref Type);

            if (Type != IntersectionType.Empty)
            {
                RayParam = RayParam0;
                return true;
            }
            else
            {
                RayParam = double.MaxValue;
                return false;
            }
        }
    }
}
