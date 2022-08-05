// Irrlich Enginge
// Intersection methods are from Irrlicht Engine and have been modified

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace SliceViewerBusinessLayer.Classes
{
    class Plane3
    {
		public Vector3 normal;
		public float d;

		public Plane3(float a, float b, float c, float d)
		{
			normal = new Vector3(a, b, c);
			this.d = d;
		}

		public Plane3(Vector3 p0, Vector3 p1, Vector3 p2)
		{
			var v1 = p1 - p0;
			var v2 = p2 - p0;

			normal = Vector3.Normalize(Vector3.Cross(v1, v2));

			d = -Vector3.Dot(normal, p0);
		}

		public bool GetIntersectionWithLine(ref Vector3 linePoint, ref Vector3 lineVect, ref Vector3 outIntersection)
		{
			float t2 = Vector3.Dot(normal, lineVect);

			if (t2 == 0)
				return false;

			float t = -(Vector3.Dot(normal, linePoint) + d) / t2;
			outIntersection = linePoint + (lineVect* t);
			return true;
		}


		public bool GetIntersectionWithPlane(Plane3 other, ref Vector3 outLinePoint, ref Vector3 outLineVect)
		{
			float fn00 = normal.Length;
			float fn01 = Vector3.Dot(normal, other.normal);
			float fn11 = other.normal.Length;
			float det = fn00 * fn11 - fn01 * fn01;

			if (Math.Abs(det) < 1E-15 )
				return false;

			float invdet = (float)(1.0 / det);
			float fc0 = (fn11 * -d + fn01 * other.d) * invdet;
			float fc1 = (fn00 * -other.d + fn01 * d) * invdet;

			outLineVect = Vector3.Cross(normal, other.normal);
			outLinePoint = normal * (float) fc0 + other.normal * (float) fc1;
			return true;
		}


		public bool GetIntersectionWithPlanes(Plane3 o1, Plane3 o2, ref Vector3 outPoint)
		{
			Vector3 linePoint = Vector3.Zero;
			Vector3	lineVect = Vector3.Zero;
			if (GetIntersectionWithPlane(o1, ref linePoint, ref lineVect))
				return o2.GetIntersectionWithLine(ref linePoint, ref lineVect, ref outPoint);

			return false;
		}

}
}
