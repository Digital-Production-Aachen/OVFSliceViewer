// Modified file from https://github.com/gradientspace/geometry3Sharp

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Geometry3SharpLight
{
    class IntrLine3AxisAlignedBox3
    {
		Line3f line;
		public Line3f Line
		{
			get { return line; }
			set { line = value; Result = IntersectionResult.NotComputed; }
		}

		AxisAlignedBox3f box;
		public AxisAlignedBox3f Box
		{
			get { return box; }
			set { box = value; Result = IntersectionResult.NotComputed; }
		}

		public int Quantity = 0;
		public IntersectionResult Result = IntersectionResult.NotComputed;
		public IntersectionType Type = IntersectionType.Empty;

		public bool IsSimpleIntersection
		{
			get { return Result == IntersectionResult.Intersects && Type == IntersectionType.Point; }
		}

		public double LineParam0, LineParam1;
		public Vector3 Point0 = Vector3.Zero;
		public Vector3 Point1 = Vector3.Zero;

		public IntrLine3AxisAlignedBox3(Line3f l, AxisAlignedBox3f b)
		{
			line = l; box = b;
		}

		/*public IntrLine3AxisAlignedBox3 Compute()
		{
			Find();
			return this;
		}*/


		/*public bool Find()
		{
			if (Result != IntersectionResult.NotComputed)
				return (Result == IntersectionResult.Intersects);

			// [RMS] if either line direction is not a normalized vector, 
			//   results are garbage, so fail query
			if (line.Direction.IsNormalized == false)
			{
				Type = IntersectionType.Empty;
				Result = IntersectionResult.InvalidQuery;
				return false;
			}

			LineParam0 = -double.MaxValue;
			LineParam1 = double.MaxValue;
			DoClipping(ref LineParam0, ref LineParam1, ref line.Origin, ref line.Direction, ref box,
					  true, ref Quantity, ref Point0, ref Point1, ref Type);

			Result = (Type != IntersectionType.Empty) ?
				IntersectionResult.Intersects : IntersectionResult.NoIntersection;
			return (Result == IntersectionResult.Intersects);
		}*/



		// [RMS TODO: lots of useless dot products below!! left over from obox conversion]
		/*public bool Test()
		{
			Vector3 AWdU = Vector3.Zero;
			Vector3 AWxDdU = Vector3.Zero;
			double RHS;

			Vector3 diff = line.Origin - box.Center;
			Vector3 WxD = line.Direction.Cross(diff);

			Vector3 extent = box.Extents;

			AWdU[1] = Math.Abs(line.Direction.Dot(Vector3.AxisY));
			AWdU[2] = Math.Abs(line.Direction.Dot(Vector3.AxisZ));
			AWxDdU[0] = Math.Abs(WxD.Dot(Vector3.AxisX));
			RHS = extent.Y * AWdU[2] + extent.Z * AWdU[1];
			if (AWxDdU[0] > RHS)
			{
				return false;
			}

			AWdU[0] = Math.Abs(line.Direction.Dot(Vector3.AxisX));
			AWxDdU[1] = Math.Abs(WxD.Dot(Vector3.AxisY));
			RHS = extent.X * AWdU[2] + extent.Z * AWdU[0];
			if (AWxDdU[1] > RHS)
			{
				return false;
			}

			AWxDdU[2] = Math.Abs(WxD.Dot(Vector3.AxisZ));
			RHS = extent.X * AWdU[1] + extent.Y * AWdU[0];
			if (AWxDdU[2] > RHS)
			{
				return false;
			}

			return true;
		}*/




		static public bool DoClipping(ref double t0, ref double t1,
						 ref Vector3 origin, ref Vector3 direction,
						 ref AxisAlignedBox3f box, bool solid, ref int quantity,
						 ref Vector3 point0, ref Vector3 point1,
						 ref IntersectionType intrType)
		{
			Vector3 BOrigin = origin - box.Center;
			Vector3 extent = box.Extents;

			double saveT0 = t0, saveT1 = t1;
			bool notAllClipped =
				Clip(+direction.X, -BOrigin.X - extent.X, ref t0, ref t1) &&
				Clip(-direction.X, +BOrigin.X - extent.X, ref t0, ref t1) &&
				Clip(+direction.Y, -BOrigin.Y - extent.Y, ref t0, ref t1) &&
				Clip(-direction.Y, +BOrigin.Y - extent.Y, ref t0, ref t1) &&
				Clip(+direction.Z, -BOrigin.Z - extent.Z, ref t0, ref t1) &&
				Clip(-direction.Z, +BOrigin.Z - extent.Z, ref t0, ref t1);

			if (notAllClipped && (solid || t0 != saveT0 || t1 != saveT1))
			{
				if (t1 > t0)
				{
					intrType = IntersectionType.Segment;
					quantity = 2;
					point0 = origin + Vector3.Multiply(direction, (float)t0);
					point1 = origin + Vector3.Multiply(direction, (float)t1);
				}
				else
				{
					intrType = IntersectionType.Point;
					quantity = 1;
					point0 = origin + Vector3.Multiply(direction, (float)t0);
				}
			}
			else
			{
				quantity = 0;
				intrType = IntersectionType.Empty;
			}

			return intrType != IntersectionType.Empty;
		}




		static public bool Clip(double denom, double numer, ref double t0, ref double t1)
		{
			// Return value is 'true' if line segment intersects the current test
			// plane.  Otherwise 'false' is returned in which case the line segment
			// is entirely clipped.

			if (denom > (double)0)
			{
				if (numer - denom * t1 > 1E-08)
				{
					return false;
				}
				if (numer > denom * t0)
				{
					t0 = numer / denom;
				}
				return true;
			}
			else if (denom < (double)0)
			{
				if (numer - denom * t0 > 1E-08)
				{
					return false;
				}
				if (numer > denom * t1)
				{
					t1 = numer / denom;
				}
				return true;
			}
			else
			{
				return numer <= (double)0;
			}
		}
	}
}
