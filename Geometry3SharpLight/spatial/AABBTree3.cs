// Modified file from https://github.com/gradientspace/geometry3Sharp

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Diagnostics;

namespace Geometry3SharpLight
{
    public class AABBTree3
    {
        public AABBTree3(Triangle3f[] triangles)
        {
            TriangleCount = triangles.Length;
            Triangles = triangles;
            Build();
        }

        public void Build()
        {
            build_top_down(false);
            //mesh_timestamp = mesh.ShapeTimestamp;
        }

        // Top-down build strategies will put at most this many triangles into a box.
        // Larger value == shallower trees, but leaves cost more to test
        public int TopDownLeafMaxTriCount = 4;

        // storage for box nodes. 
        //   - box_to_index is a pointer into index_list
        //   - box_centers and box_extents are the centers/extents of the bounding boxes
        protected DVector<int> box_to_index;
        protected DVector<Vector3> box_centers;
        protected DVector<Vector3> box_extents;

        // list of indices for a given box. There is *no* marker/sentinel between
        // boxes, you have to get the starting index from box_to_index[]
        //
        // There are three kinds of records:
        //   - if i < triangles_end, then the list is a number of triangles,
        //       stored as [N t1 t2 t3 ... tN]
        //   - if i > triangles_end and index_list[i] < 0, this is a single-child
        //       internal box, with index (-index_list[i])-1     (shift-by-one in case actual value is 0!)
        //   - if i > triangles_end and index_list[i] > 0, this is a two-child
        //       internal box, with indices index_list[i]-1 and index_list[i+1]-1
        protected DVector<int> index_list;

        // index_list[i] for i < triangles_end is a triangle-index list, otherwise box-index pair/single
        protected int triangles_end = -1;

        // box_to_index[root_index] is the root node of the tree
        protected int root_index = -1;

        const float box_eps = (float)(50.0f * 1.19209E-07);

        public Func<int, bool> TriangleFilterF = null;

        //---------------------
        int TriangleCount;
        Triangle3f[] Triangles;

        public Vector3 GetTriCentroid(Triangle3f tri)
        {
            double f = (1.0 / 3.0);
            return new Vector3(
                (float)((tri.VertexA.X + tri.VertexB.X + tri.VertexC.X) * f),
                (float)((tri.VertexA.Y + tri.VertexB.Y + tri.VertexC.Y) * f),
                (float)((tri.VertexA.Z + tri.VertexB.Z + tri.VertexC.Z) * f));
        }

        public AxisAlignedBox3f GetTriBounds(Triangle3f tri)
        {
            float x = tri.VertexA.X, y = tri.VertexA.Y, z = tri.VertexA.Z;
            float minx = x, maxx = x, miny = y, maxy = y, minz = z, maxz = z;

            x = tri.VertexB.X; y = tri.VertexB.Y; z = tri.VertexB.Z;
            if (x < minx) minx = x; else if (x > maxx) maxx = x;
            if (y < miny) miny = y; else if (y > maxy) maxy = y;
            if (z < minz) minz = z; else if (z > maxz) maxz = z;

            x = tri.VertexC.X; y = tri.VertexC.Y; z = tri.VertexC.Z;
            if (x < minx) minx = x; else if (x > maxx) maxx = x;
            if (y < miny) miny = y; else if (y > maxy) maxy = y;
            if (z < minz) minz = z; else if (z > maxz) maxz = z;

            return new AxisAlignedBox3f(minx, miny, minz, maxx, maxy, maxz);
        }

        void build_top_down(bool bSorted)
        {
            // build list of valid triangles & centers. We skip any
            // triangles that have infinite/garbage vertices...
            int[] triangles = new int[TriangleCount];
            Vector3[] centers = new Vector3[TriangleCount];
            for (int i = 0; i < TriangleCount; i++)
            {
                //Debug.WriteLine(Triangles[i].VertexA + " " + Triangles[i].VertexB + " " + Triangles[i].VertexC);
                Vector3 centroid = GetTriCentroid(Triangles[i]);
                double d2 = centroid.LengthSquared;
                bool bInvalid = double.IsNaN(d2) || double.IsInfinity(d2);
                //Debug.Assert(bInvalid == false);
                if (bInvalid == false)
                {
                    triangles[i] = i;
                    centers[i] = GetTriCentroid(Triangles[i]);
                } // otherwise skip this tri
            }

            boxes_set tris = new boxes_set();
            boxes_set nodes = new boxes_set();
            AxisAlignedBox3f rootBox;
            int rootnode = split_tri_set_midpoint(triangles, centers, 0, TriangleCount, 0, TopDownLeafMaxTriCount, tris, nodes, out rootBox);
            /*int rootnode = (bSorted) ?
                split_tri_set_sorted(triangles, centers, 0, mesh.TriangleCount, 0, TopDownLeafMaxTriCount, tris, nodes, out rootBox)
                : split_tri_set_midpoint(triangles, centers, 0, mesh.TriangleCount, 0, TopDownLeafMaxTriCount, tris, nodes, out rootBox);*/

            box_to_index = tris.box_to_index;
            box_centers = tris.box_centers;
            box_extents = tris.box_extents;
            index_list = tris.index_list;
            triangles_end = tris.iIndicesCur;
            int iIndexShift = triangles_end;
            int iBoxShift = tris.iBoxCur;

            // ok now append internal node boxes & index ptrs
            for (int i = 0; i < nodes.iBoxCur; ++i)
            {
                box_centers.insert(nodes.box_centers[i], iBoxShift + i);
                box_extents.insert(nodes.box_extents[i], iBoxShift + i);
                // internal node indices are shifted
                box_to_index.insert(iIndexShift + nodes.box_to_index[i], iBoxShift + i);
            }

            // now append index list
            for (int i = 0; i < nodes.iIndicesCur; ++i)
            {
                int child_box = nodes.index_list[i];
                if (child_box < 0)
                {        // this is a triangles box
                    child_box = (-child_box) - 1;
                }
                else
                {
                    child_box += iBoxShift;
                }
                child_box = child_box + 1;
                index_list.insert(child_box, iIndexShift + i);
            }

            root_index = rootnode + iBoxShift;
        }

        class boxes_set
        {
            public DVector<int> box_to_index = new DVector<int>();
            public DVector<Vector3> box_centers = new DVector<Vector3>();
            public DVector<Vector3> box_extents = new DVector<Vector3>();
            public DVector<int> index_list = new DVector<int>();
            public int iBoxCur = 0;
            public int iIndicesCur = 0;
        }

        int split_tri_set_midpoint(int[] triangles, Vector3[] centers, int iStart, int iCount, int depth, int minTriCount,
            boxes_set tris, boxes_set nodes, out AxisAlignedBox3f box)
        {
            box = AxisAlignedBox3f.Empty;
            int iBox = -1;

            if (iCount < minTriCount)
            {
                // append new triangles box
                iBox = tris.iBoxCur++;
                tris.box_to_index.insert(tris.iIndicesCur, iBox);

                tris.index_list.insert(iCount, tris.iIndicesCur++);
                for (int i = 0; i < iCount; ++i)
                {
                    tris.index_list.insert(triangles[iStart + i], tris.iIndicesCur++);
                    box.Contain(GetTriBounds(Triangles[triangles[iStart + i]])); // made changes here!
                }

                tris.box_centers.insert(box.Center, iBox);
                tris.box_extents.insert(box.Extents, iBox);

                return -(iBox + 1);
            }

            //compute interval along an axis and find midpoint
            int axis = depth % 3;
            Interval1d interval = new Interval1d(double.MaxValue, -double.MaxValue);
            for (int i = 0; i < iCount; ++i)
            {
                interval.Contain(centers[iStart + i][axis]);
            } 
            double midpoint = interval.Center;

            int n0, n1;
            if (Math.Abs(interval.a - interval.b) > 1E-08/*MathUtil.ZeroTolerance*/)
            {
                // we have to re-sort the centers & triangles lists so that centers < midpoint
                // are first, so that we can recurse on the two subsets. We walk in from each side,
                // until we find two out-of-order locations, then we swap them.
                int l = 0;
                int r = iCount - 1;
                while (l < r)
                {
                    // [RMS] is <= right here? if v.axis == midpoint, then this loop
                    //   can get stuck unless one of these has an equality test. But
                    //   I did not think enough about if this is the right thing to do...
                    while (centers[iStart + l][axis] <= midpoint)
                        l++;
                    while (centers[iStart + r][axis] > midpoint)
                        r--;
                    if (l >= r)
                        break;      //done!
                    //swap
                    Vector3 tmpc = centers[iStart + l]; centers[iStart + l] = centers[iStart + r]; centers[iStart + r] = tmpc;
                    int tmpt = triangles[iStart + l]; triangles[iStart + l] = triangles[iStart + r]; triangles[iStart + r] = tmpt;
                }

                n0 = l;
                n1 = iCount - n0;
                //Debug.Assert(n0 >= 1 && n1 >= 1);
            }
            else
            {
                // interval is near-empty, so no point trying to do sorting, just split half and half
                n0 = iCount / 2;
                n1 = iCount - n0;
            }

            // create child boxes
            AxisAlignedBox3f box1;
            int child0 = split_tri_set_midpoint(triangles, centers, iStart, n0, depth + 1, minTriCount, tris, nodes, out box);
            int child1 = split_tri_set_midpoint(triangles, centers, iStart + n0, n1, depth + 1, minTriCount, tris, nodes, out box1);
            box.Contain(box1);

            // append new box
            iBox = nodes.iBoxCur++;
            nodes.box_to_index.insert(nodes.iIndicesCur, iBox);

            nodes.index_list.insert(child0, nodes.iIndicesCur++);
            nodes.index_list.insert(child1, nodes.iIndicesCur++);

            nodes.box_centers.insert(box.Center, iBox);
            nodes.box_extents.insert(box.Extents, iBox);

            return iBox;
        }

        public virtual int FindNearestHitTriangle(Ray3f ray, double fMaxDist = double.MaxValue)
        {
            /*if (mesh_timestamp != mesh.ShapeTimestamp)
                throw new Exception("DMeshAABBTree3.FindNearestHitTriangle: mesh has been modified since tree construction");
            if (ray.Direction.IsNormalized == false)
                throw new Exception("DMeshAABBTree3.FindNearestHitTriangle: ray direction is not normalized");*/

            // [RMS] note: using float.MaxValue here because we need to use <= to compare box hit
            //   to fNearestT, and box hit returns double.MaxValue on no-hit. So, if we set
            //   nearestT to double.MaxValue, then we will test all boxes (!)
            double fNearestT = (fMaxDist < double.MaxValue) ? fMaxDist : float.MaxValue;
            int tNearID = -1;
            find_hit_triangle(root_index, ref ray, ref fNearestT, ref tNearID);
            return tNearID;
        }

        protected void find_hit_triangle(int iBox, ref Ray3f ray, ref double fNearestT, ref int tID)
        {
            int idx = box_to_index[iBox];
            if (idx < triangles_end)
            {            // triange-list case, array is [N t1 t2 ... tN]
                Triangle3f tri = new Triangle3f();
                int num_tris = index_list[idx];
                for (int i = 1; i <= num_tris; ++i)
                {
                    int ti = index_list[idx + i];
                    if (TriangleFilterF != null && TriangleFilterF(ti) == false)
                        continue;

                    tri = Triangles[ti];
                    //mesh.GetTriVertices(ti, ref tri.V0, ref tri.V1, ref tri.V2);
                    double rayt;
                    if (IntrRay3Triangle3.Intersects(ref ray, ref tri.VertexA, ref tri.VertexB, ref tri.VertexC, out rayt))
                    {
                        if (rayt < fNearestT)
                        {
                            fNearestT = rayt;
                            tID = ti;
                        }
                    }
                    //IntrRay3Triangle3 ray_tri_hit = new IntrRay3Triangle3(ray, tri);
                    //if ( ray_tri_hit.Find() ) {
                    //    if ( ray_tri_hit.RayParameter < fNearestT ) {
                    //        fNearestT = ray_tri_hit.RayParameter;
                    //        tID = ti;
                    //    }
                    //}
                }

            }
            else
            {                                // internal node, either 1 or 2 child boxes
                double e = 1E-06;

                int iChild1 = index_list[idx];
                if (iChild1 < 0)
                {                 // 1 child, descend if nearer than cur min-dist
                    iChild1 = (-iChild1) - 1;
                    double fChild1T = box_ray_intersect_t(iChild1, ray);
                    if (fChild1T <= fNearestT + e)
                    {
                        find_hit_triangle(iChild1, ref ray, ref fNearestT, ref tID);
                    }

                }
                else
                {                            // 2 children, descend closest first
                    iChild1 = iChild1 - 1;
                    int iChild2 = index_list[idx + 1] - 1;

                    double fChild1T = box_ray_intersect_t(iChild1, ray);
                    double fChild2T = box_ray_intersect_t(iChild2, ray);
                    if (fChild1T < fChild2T)
                    {
                        if (fChild1T <= fNearestT + e)
                        {
                            find_hit_triangle(iChild1, ref ray, ref fNearestT, ref tID);
                            if (fChild2T <= fNearestT + e)
                            {
                                find_hit_triangle(iChild2, ref ray, ref fNearestT, ref tID);
                            }
                        }
                    }
                    else
                    {
                        if (fChild2T <= fNearestT + e)
                        {
                            find_hit_triangle(iChild2, ref ray, ref fNearestT, ref tID);
                            if (fChild1T <= fNearestT + e)
                            {
                                find_hit_triangle(iChild1, ref ray, ref fNearestT, ref tID);
                            }
                        }
                    }

                }
            }
        }

        double box_ray_intersect_t(int iBox, Ray3f ray)
        {
            Vector3 c = box_centers[iBox];
            Vector3 e = box_extents[iBox];
            AxisAlignedBox3f box = new AxisAlignedBox3f(ref c, e.X + box_eps, e.Y + box_eps, e.Z + box_eps);

            double ray_t = double.MaxValue;
            if (IntrRay3AxisAlignedBox3.FindRayIntersectT(ref ray, ref box, out ray_t))
            {
                return ray_t;
            }
            else
            {
                return double.MaxValue;
            }
        }
    }
}
