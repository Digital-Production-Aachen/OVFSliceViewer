// Modified file from https://github.com/gradientspace/geometry3Sharp

using OpenTK;

namespace Geometry3SharpLight
{
    class IntrRay3Triangle3
    {
        public static bool Intersects(ref Ray3f ray, ref Vector3 V0, ref Vector3 V1, ref Vector3 V2, out double rayT)
        {
            // Compute the offset origin, edges, and normal.
            Vector3 diff = ray.Origin - V0;
            Vector3 edge1 = V1 - V0;
            Vector3 edge2 = V2 - V0;
            Vector3 normal = Vector3.Cross(edge1, edge2); // TODO: removed ref here

            rayT = double.MaxValue;

            // Solve Q + t*D = b1*E1 + b2*E2 (Q = kDiff, D = ray direction,
            // E1 = kEdge1, E2 = kEdge2, N = Cross(E1,E2)) by
            //   |Dot(D,N)|*b1 = sign(Dot(D,N))*Dot(D,Cross(Q,E2))
            //   |Dot(D,N)|*b2 = sign(Dot(D,N))*Dot(D,Cross(E1,Q))
            //   |Dot(D,N)|*t = -sign(Dot(D,N))*Dot(Q,N)
            double DdN = Vector3.Dot(ray.Direction, normal);
            double sign;
            if (DdN > 1E-08)
            {
                sign = 1;
            }
            else if (DdN < -1E-08)
            {
                sign = -1;
                DdN = -DdN;
            }
            else
            {
                // Ray and triangle are parallel, call it a "no intersection"
                // even if the ray does intersect.
                return false;
            }

            Vector3 cross = Vector3.Cross(diff, edge2);
            double DdQxE2 = sign * Vector3.Dot(ray.Direction, cross);
            if (DdQxE2 >= 0)
            {
                cross = Vector3.Cross(edge1, diff);
                double DdE1xQ = sign * Vector3.Dot(ray.Direction, cross);
                if (DdE1xQ >= 0)
                {
                    if (DdQxE2 + DdE1xQ <= DdN)
                    {
                        // Line intersects triangle, check if ray does.
                        double QdN = -sign * Vector3.Dot(diff, normal);
                        if (QdN >= 0)
                        {
                            // Ray intersects triangle.
                            double inv = (1) / DdN;
                            rayT = QdN * inv;
                            return true;
                        }
                        // else: t < 0, no intersection
                    }
                    // else: b1+b2 > 1, no intersection
                }
                // else: b2 < 0, no intersection
            }
            // else: b1 < 0, no intersection

            return false;
        }
    }
}
