// Modified file from https://github.com/gradientspace/geometry3Sharp

using System;
using OpenTK;
using OpenTK.Mathematics;

namespace Geometry3SharpLight
{
    public struct AxisAlignedBox3f : IComparable<AxisAlignedBox3f>, IEquatable<AxisAlignedBox3f>
    {
        public Vector3 Min;
        public Vector3 Max;

        public static readonly AxisAlignedBox3f Empty = new AxisAlignedBox3f(false);
        public static readonly AxisAlignedBox3f Zero = new AxisAlignedBox3f(0);
        public static readonly AxisAlignedBox3f UnitPositive = new AxisAlignedBox3f(1);
        public static readonly AxisAlignedBox3f Infinite =
            new AxisAlignedBox3f(float.MinValue, float.MinValue, float.MinValue, float.MaxValue, float.MaxValue, float.MaxValue);


        public AxisAlignedBox3f(bool bIgnore)
        {
            Min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        }

        public AxisAlignedBox3f(float xmin, float ymin, float zmin, float xmax, float ymax, float zmax)
        {
            Min = new Vector3(xmin, ymin, zmin);
            Max = new Vector3(xmax, ymax, zmax);
        }

        public AxisAlignedBox3f(float fCubeSize)
        {
            Min = new Vector3(0, 0, 0);
            Max = new Vector3(fCubeSize, fCubeSize, fCubeSize);
        }

        public AxisAlignedBox3f(float fWidth, float fHeight, float fDepth)
        {
            Min = new Vector3(0, 0, 0);
            Max = new Vector3(fWidth, fHeight, fDepth);
        }

        public AxisAlignedBox3f(Vector3 vMin, Vector3 vMax)
        {
            Min = new Vector3(Math.Min(vMin.X, vMax.X), Math.Min(vMin.Y, vMax.Y), Math.Min(vMin.Z, vMax.Z));
            Max = new Vector3(Math.Max(vMin.X, vMax.X), Math.Max(vMin.Y, vMax.Y), Math.Max(vMin.Z, vMax.Z));
        }
        public AxisAlignedBox3f(ref Vector3 vMin, ref Vector3 vMax)
        {
            Min = new Vector3(Math.Min(vMin.X, vMax.X), Math.Min(vMin.Y, vMax.Y), Math.Min(vMin.Z, vMax.Z));
            Max = new Vector3(Math.Max(vMin.X, vMax.X), Math.Max(vMin.Y, vMax.Y), Math.Max(vMin.Z, vMax.Z));
        }


        public AxisAlignedBox3f(Vector3 vCenter, float fHalfWidth, float fHalfHeight, float fHalfDepth)
        {
            Min = new Vector3(vCenter.X - fHalfWidth, vCenter.Y - fHalfHeight, vCenter.Z - fHalfDepth);
            Max = new Vector3(vCenter.X + fHalfWidth, vCenter.Y + fHalfHeight, vCenter.Z + fHalfDepth);
        }
        public AxisAlignedBox3f(ref Vector3 vCenter, float fHalfWidth, float fHalfHeight, float fHalfDepth)
        {
            Min = new Vector3(vCenter.X - fHalfWidth, vCenter.Y - fHalfHeight, vCenter.Z - fHalfDepth);
            Max = new Vector3(vCenter.X + fHalfWidth, vCenter.Y + fHalfHeight, vCenter.Z + fHalfDepth);
        }


        public AxisAlignedBox3f(Vector3 vCenter, float fHalfSize)
        {
            Min = new Vector3(vCenter.X - fHalfSize, vCenter.Y - fHalfSize, vCenter.Z - fHalfSize);
            Max = new Vector3(vCenter.X + fHalfSize, vCenter.Y + fHalfSize, vCenter.Z + fHalfSize);
        }

        public AxisAlignedBox3f(Vector3 vCenter)
        {
            Min = Max = vCenter;
        }

        public float Width
        {
            get { return Math.Max(Max.X - Min.X, 0); }
        }
        public float Height
        {
            get { return Math.Max(Max.Y - Min.Y, 0); }
        }
        public float Depth
        {
            get { return Math.Max(Max.Z - Min.Z, 0); }
        }

        public float Volume
        {
            get { return Width * Height * Depth; }
        }
        public float DiagonalLength
        {
            get
            {
                return (float)Math.Sqrt((Max.X - Min.X) * (Max.X - Min.X)
              + (Max.Y - Min.Y) * (Max.Y - Min.Y) + (Max.Z - Min.Z) * (Max.Z - Min.Z));
            }
        }
        public float MaxDim
        {
            get { return Math.Max(Width, Math.Max(Height, Depth)); }
        }

        public Vector3 Diagonal
        {
            get { return new Vector3(Max.X - Min.X, Max.Y - Min.Y, Max.Z - Min.Z); }
        }
        public Vector3 Extents
        {
            get { return new Vector3((float)((Max.X - Min.X) * 0.5), (float)((Max.Y - Min.Y) * 0.5), (float)((Max.Z - Min.Z) * 0.5)); }
        }
        public Vector3 Center
        {
            get { return new Vector3((float)(0.5 * (Min.X + Max.X)), (float)(0.5 * (Min.Y + Max.Y)), (float)(0.5 * (Min.Z + Max.Z))); }
        }


        public static bool operator ==(AxisAlignedBox3f a, AxisAlignedBox3f b)
        {
            return a.Min == b.Min && a.Max == b.Max;
        }
        public static bool operator !=(AxisAlignedBox3f a, AxisAlignedBox3f b)
        {
            return a.Min != b.Min || a.Max != b.Max;
        }
        public override bool Equals(object obj)
        {
            return this == (AxisAlignedBox3f)obj;
        }
        public bool Equals(AxisAlignedBox3f other)
        {
            return this == other;
        }
        /*public int CompareTo(AxisAlignedBox3f other)
        {
            int c = this.Min.CompareTo(other.Min);
            if (c == 0)
                return this.Max.CompareTo(other.Max);
            return c;
        }*/
        public override int GetHashCode()
        {
            unchecked
            { // Overflow is fine, just wrap
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ Min.GetHashCode();
                hash = (hash * 16777619) ^ Max.GetHashCode();
                return hash;
            }
        }


        // See Box3.Corner for details on which corner is which
        public Vector3 Corner(int i)
        {
            float x = (((i & 1) != 0) ^ ((i & 2) != 0)) ? (Max.X) : (Min.X);
            float y = ((i / 2) % 2 == 0) ? (Min.Y) : (Max.Y);
            float z = (i < 4) ? (Min.Z) : (Max.Z);
            return new Vector3(x, y, z);
        }


        /// <summary>
        /// Returns point on face/edge/corner. For each coord value neg==min, 0==center, pos==max
        /// </summary>
        public Vector3 Point(int xi, int yi, int zi)
        {
            float x = (xi < 0) ? Min.X : ((xi == 0) ? (0.5f * (Min.X + Max.X)) : Max.X);
            float y = (yi < 0) ? Min.Y : ((yi == 0) ? (0.5f * (Min.Y + Max.Y)) : Max.Y);
            float z = (zi < 0) ? Min.Z : ((zi == 0) ? (0.5f * (Min.Z + Max.Z)) : Max.Z);
            return new Vector3(x, y, z);
        }


        //! value is subtracted from min and added to max
        public void Expand(float fRadius)
        {
            Min.X -= fRadius; Min.Y -= fRadius; Min.Z -= fRadius;
            Max.X += fRadius; Max.Y += fRadius; Max.Z += fRadius;
        }
        //! value is added to min and subtracted from max
        public void Contract(float fRadius)
        {
            Min.X += fRadius; Min.Y += fRadius; Min.Z += fRadius;
            Max.X -= fRadius; Max.Y -= fRadius; Max.Z -= fRadius;
        }

        public void Scale(float sx, float sy, float sz)
        {
            Vector3 c = Center;
            Vector3 e = Extents; e.X *= sx; e.Y *= sy; e.Z *= sz;
            Min = new Vector3(c.X - e.X, c.Y - e.Y, c.Z - e.Z);
            Max = new Vector3(c.X + e.X, c.Y + e.Y, c.Z + e.Z);
        }

        public void Contain(Vector3 v)
        {
            Min.X = Math.Min(Min.X, v.X);
            Min.Y = Math.Min(Min.Y, v.Y);
            Min.Z = Math.Min(Min.Z, v.Z);
            Max.X = Math.Max(Max.X, v.X);
            Max.Y = Math.Max(Max.Y, v.Y);
            Max.Z = Math.Max(Max.Z, v.Z);
        }

        public void Contain(AxisAlignedBox3f box)
        {
            Min.X = Math.Min(Min.X, box.Min.X);
            Min.Y = Math.Min(Min.Y, box.Min.Y);
            Min.Z = Math.Min(Min.Z, box.Min.Z);
            Max.X = Math.Max(Max.X, box.Max.X);
            Max.Y = Math.Max(Max.Y, box.Max.Y);
            Max.Z = Math.Max(Max.Z, box.Max.Z);
        }


        public void Contain(Vector3d v)
        {
            Min.X = Math.Min(Min.X, (float)v.X);
            Min.Y = Math.Min(Min.Y, (float)v.Y);
            Min.Z = Math.Min(Min.Z, (float)v.Z);
            Max.X = Math.Max(Max.X, (float)v.X);
            Max.Y = Math.Max(Max.Y, (float)v.Y);
            Max.Z = Math.Max(Max.Z, (float)v.Z);
        }

        /*public void Contain(AxisAlignedBox3d box)
        {
            Min.X = Math.Min(Min.X, (float)box.Min.X);
            Min.Y = Math.Min(Min.Y, (float)box.Min.Y);
            Min.Z = Math.Min(Min.Z, (float)box.Min.Z);
            Max.X = Math.Max(Max.X, (float)box.Max.X);
            Max.Y = Math.Max(Max.Y, (float)box.Max.Y);
            Max.Z = Math.Max(Max.Z, (float)box.Max.Z);
        }*/


        public AxisAlignedBox3f Intersect(AxisAlignedBox3f box)
        {
            AxisAlignedBox3f intersect = new AxisAlignedBox3f(
                Math.Max(Min.X, box.Min.X), Math.Max(Min.Y, box.Min.Y), Math.Max(Min.Z, box.Min.Z),
                Math.Min(Max.X, box.Max.X), Math.Min(Max.Y, box.Max.Y), Math.Min(Max.Z, box.Max.Z));
            if (intersect.Height <= 0 || intersect.Width <= 0 || intersect.Depth <= 0)
                return AxisAlignedBox3f.Empty;
            else
                return intersect;
        }



        public bool Contains(Vector3 v)
        {
            return (Min.X <= v.X) && (Min.Y <= v.Y) && (Min.Z <= v.Z)
                && (Max.X >= v.X) && (Max.Y >= v.Y) && (Max.Z >= v.Z);
        }
        public bool Intersects(AxisAlignedBox3f box)
        {
            return !((box.Max.X <= Min.X) || (box.Min.X >= Max.X)
                || (box.Max.Y <= Min.Y) || (box.Min.Y >= Max.Y)
                || (box.Max.Z <= Min.Z) || (box.Min.Z >= Max.Z));
        }


        public double DistanceSquared(Vector3 v)
        {
            float dx = (v.X < Min.X) ? Min.X - v.X : (v.X > Max.X ? v.X - Max.X : 0);
            float dy = (v.Y < Min.Y) ? Min.Y - v.Y : (v.Y > Max.Y ? v.Y - Max.Y : 0);
            float dz = (v.Z < Min.Z) ? Min.Z - v.Z : (v.Z > Max.Z ? v.Z - Max.Z : 0);
            return dx * dx + dy * dy + dz * dz;
        }
        public float Distance(Vector3 v)
        {
            return (float)Math.Sqrt(DistanceSquared(v));
        }


        public Vector3 NearestPoint(Vector3 v)
        {
            float x = (v.X < Min.X) ? Min.X : (v.X > Max.X ? Max.X : v.X);
            float y = (v.Y < Min.Y) ? Min.Y : (v.Y > Max.Y ? Max.Y : v.Y);
            float z = (v.Z < Min.Z) ? Min.Z : (v.Z > Max.Z ? Max.Z : v.Z);
            return new Vector3(x, y, z);
        }



        //! relative translation
        /*public void Translate(Vector3 vTranslate)
        {
            Min.Add(vTranslate);
            Max.Add(vTranslate);
        }

        public void MoveMin(Vector3 vNewMin)
        {
            Max.X = vNewMin.X + (Max.X - Min.X);
            Max.Y = vNewMin.Y + (Max.Y - Min.Y);
            Max.Z = vNewMin.Z + (Max.Z - Min.Z);
            Min.Set(vNewMin);
        }
        public void MoveMin(float fNewX, float fNewY, float fNewZ)
        {
            Max.X = fNewX + (Max.X - Min.X);
            Max.Y = fNewY + (Max.Y - Min.Y);
            Max.Z = fNewZ + (Max.Z - Min.Z);
            Min.Set(fNewX, fNewY, fNewZ);
        }*/



        public override string ToString()
        {
            return string.Format("x[{0:F8},{1:F8}] y[{2:F8},{3:F8}] z[{4:F8},{5:F8}]", Min.X, Max.X, Min.Y, Max.Y, Min.Z, Max.Z);
        }

        public int CompareTo(AxisAlignedBox3f other)
        {
            throw new NotImplementedException();
        }
    }
}
