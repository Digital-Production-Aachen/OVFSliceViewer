// Irrlicht Engine
// Modified file from https://sourceforge.net/p/irrlicht/code/HEAD/tree/trunk/include/SViewFrustum.h

using OpenTK;
using OpenTK.Mathematics;

namespace SliceViewerBusinessLayer.Classes
{
    public class ViewFrustum
    {
		//! Defines the view frustum. That's the space visible by the camera.
		/** The view frustum is enclosed by 6 planes. These six planes share
		eight points. A bounding box around these eight points is also stored in
		this structure.
		*/
		enum VFPLANES
		{
			//! Far plane of the frustum. That is the plane furthest away from the eye.
			VF_FAR_PLANE = 0,
			//! Near plane of the frustum. That is the plane nearest to the eye.
			VF_NEAR_PLANE,
			//! Left plane of the frustum.
			VF_LEFT_PLANE,
			//! Right plane of the frustum.
			VF_RIGHT_PLANE,
			//! Bottom plane of the frustum.
			VF_BOTTOM_PLANE,
			//! Top plane of the frustum.
			VF_TOP_PLANE,

			//! Amount of planes enclosing the view frustum. Should be 6.
			VF_PLANE_COUNT
		};

		//! the position of the camera
		public Vector3 cameraPosition;

		//! all planes enclosing the view frustum.
		Plane3[] planes = new Plane3[6]; //manually set to 6
		// plane3d<f32> planes[VF_PLANE_COUNT];

		//! bounding box around the view frustum
		//aabbox3d<f32> boundingBox;


		//! Hold a copy of important transform matrices
		private enum E_TRANSFORMATION_STATE_FRUSTUM
		{
			ETS_VIEW = 0,
			ETS_PROJECTION = 1,
			ETS_COUNT_FRUSTUM
		};

		//! Hold a copy of important transform matrices
		//Matrix4[] matrices = new Matrix4[(int)E_TRANSFORMATION_STATE_FRUSTUM.ETS_COUNT_FRUSTUM];

		//float BoundingRadius;
		//float FarNearDistance;
		//Vector3 BoundingCenter;

		public ViewFrustum( Matrix4 mat, bool zClipFromZero)
		{
            for (int i = 0; i < (int)VFPLANES.VF_PLANE_COUNT; i++)
            {
				planes[i] = new Plane3(new Vector3(0,0,0), new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            }
			SetFrom(mat, zClipFromZero);
		}


		/*public void transform( Matrix4 mat)
		{
			for (u32 i = 0; i < VFPLANES.VF_PLANE_COUNT; ++i)
				mat.transformPlane(planes[i]);

			mat.transformVect(cameraPosition);
			recalculateBoundingBox();
		}*/


		public Vector3 GetFarLeftUp() 
		{
			Vector3 p = Vector3.Zero;
			planes[(int) VFPLANES.VF_FAR_PLANE].GetIntersectionWithPlanes(
				planes[(int)VFPLANES.VF_TOP_PLANE],
				planes[(int)VFPLANES.VF_LEFT_PLANE], ref p);

			return p;
		}

		public Vector3 GetFarLeftDown() 
		{
			Vector3 p = Vector3.Zero;
			planes[(int)VFPLANES.VF_FAR_PLANE].GetIntersectionWithPlanes(
				planes[(int)VFPLANES.VF_BOTTOM_PLANE],
				planes[(int)VFPLANES.VF_LEFT_PLANE], ref p);

			return p;
		}

		public Vector3 GetFarRightUp() 
		{
			Vector3 p = Vector3.Zero;
			planes[(int)VFPLANES.VF_FAR_PLANE].GetIntersectionWithPlanes(
				planes[(int)VFPLANES.VF_TOP_PLANE],
				planes[(int)VFPLANES.VF_RIGHT_PLANE], ref p);

			return p;
		}

		public Vector3 GetFarRightDown() 
		{
			Vector3 p = Vector3.Zero;
			planes[(int)VFPLANES.VF_FAR_PLANE].GetIntersectionWithPlanes(
				planes[(int)VFPLANES.VF_BOTTOM_PLANE],
				planes[(int)VFPLANES.VF_RIGHT_PLANE], ref p);

			return p;
		}

		public Vector3 GetNearLeftUp() 
		{
			Vector3 p = Vector3.Zero;
			planes[(int)VFPLANES.VF_NEAR_PLANE].GetIntersectionWithPlanes(
				planes[(int)VFPLANES.VF_TOP_PLANE],
				planes[(int)VFPLANES.VF_LEFT_PLANE], ref p);

			return p;
		}

		public Vector3 GetNearLeftDown() 
		{
			Vector3 p = Vector3.Zero;
			planes[(int)VFPLANES.VF_NEAR_PLANE].GetIntersectionWithPlanes(
				planes[(int)VFPLANES.VF_BOTTOM_PLANE],
				planes[(int)VFPLANES.VF_LEFT_PLANE], ref p);

			return p;
		}

		public Vector3 GetNearRightUp() 
		{
			Vector3 p = Vector3.Zero;
			planes[(int)VFPLANES.VF_NEAR_PLANE].GetIntersectionWithPlanes(
				planes[(int)VFPLANES.VF_TOP_PLANE],
				planes[(int)VFPLANES.VF_RIGHT_PLANE], ref p);

			return p;
		}

		public Vector3 GetNearRightDown() 
		{
			Vector3 p = Vector3.Zero;
			planes[(int)VFPLANES.VF_NEAR_PLANE].GetIntersectionWithPlanes(
				planes[(int)VFPLANES.VF_BOTTOM_PLANE],
				planes[(int)VFPLANES.VF_RIGHT_PLANE], ref p);

			return p;
		}

		/*public  aabbox3d<f32> getBoundingBox() 
		{
			return boundingBox;
		}*/

		/*public void recalculateBoundingBox()
		{
			boundingBox.reset(getNearLeftUp());
			boundingBox.addInternalPoint(getNearRightUp());
			boundingBox.addInternalPoint(getNearLeftDown());
			boundingBox.addInternalPoint(getNearRightDown());
			boundingBox.addInternalPoint(getFarRightUp());
			boundingBox.addInternalPoint(getFarLeftDown());
			boundingBox.addInternalPoint(getFarRightDown());
			boundingBox.addInternalPoint(getFarLeftUp());

			// Also recalculate the bounding sphere when the bbox changes
			recalculateBoundingSphere();
		}*/

		/*public float getBoundingRadius() 
		{
			return BoundingRadius;
		}

		public Vector3 getBoundingCenter() 
		{
			return BoundingCenter;
		}

		public void setFarNearDistance(float distance)
		{
			FarNearDistance = distance;
		}*/

		//! This constructor creates a view frustum based on a projection
		//! and/or view matrix.
		public void SetFrom( Matrix4 mat, bool zClipFromZero)
		{
			// left clipping plane
			planes[(int)VFPLANES.VF_LEFT_PLANE].normal.X = mat[0, 3] + mat[0, 0];
			planes[(int)VFPLANES.VF_LEFT_PLANE].normal.Y = mat[1, 3] + mat[1, 0];
			planes[(int)VFPLANES.VF_LEFT_PLANE].normal.Z = mat[2, 3] + mat[2, 0];
			planes[(int)VFPLANES.VF_LEFT_PLANE].d = mat[3, 3] + mat[3, 0];

			// right clipping plane
			planes[(int)VFPLANES.VF_RIGHT_PLANE].normal.X = mat[0, 3] - mat[0, 0];
			planes[(int)VFPLANES.VF_RIGHT_PLANE].normal.Y = mat[1, 3] - mat[1, 0];
			planes[(int)VFPLANES.VF_RIGHT_PLANE].normal.Z = mat[2, 3] - mat[2, 0];
			planes[(int)VFPLANES.VF_RIGHT_PLANE].d = mat[3, 3] - mat[3, 0];

			// top clipping plane
			planes[(int)VFPLANES.VF_TOP_PLANE].normal.X = mat[0, 3] - mat[0, 1];
			planes[(int)VFPLANES.VF_TOP_PLANE].normal.Y = mat[1, 3] - mat[1, 1];
			planes[(int)VFPLANES.VF_TOP_PLANE].normal.Z = mat[2, 3] - mat[2, 1];
			planes[(int)VFPLANES.VF_TOP_PLANE].d = mat[3, 3] - mat[3, 1];

			// bottom clipping plane
			planes[(int)VFPLANES.VF_BOTTOM_PLANE].normal.X = mat[0, 3] + mat[0, 1];
			planes[(int)VFPLANES.VF_BOTTOM_PLANE].normal.Y = mat[1, 3] + mat[1, 1];
			planes[(int)VFPLANES.VF_BOTTOM_PLANE].normal.Z = mat[2, 3] + mat[2, 1];
			planes[(int)VFPLANES.VF_BOTTOM_PLANE].d = mat[3, 3] + mat[3, 1];

			// far clipping plane
			planes[(int)VFPLANES.VF_FAR_PLANE].normal.X = mat[0, 3] - mat[0, 2];
			planes[(int)VFPLANES.VF_FAR_PLANE].normal.Y = mat[1, 3] - mat[1, 2];
			planes[(int)VFPLANES.VF_FAR_PLANE].normal.Z = mat[2, 3] - mat[2, 2];
			planes[(int)VFPLANES.VF_FAR_PLANE].d =		  mat[3, 3] - mat[3, 2];

			// near clipping plane
			if (zClipFromZero)
			{
				planes[(int)VFPLANES.VF_NEAR_PLANE].normal.X = mat[0, 2];
				planes[(int)VFPLANES.VF_NEAR_PLANE].normal.Y = mat[1, 2];
				planes[(int)VFPLANES.VF_NEAR_PLANE].normal.Z = mat[2, 2];
				planes[(int)VFPLANES.VF_NEAR_PLANE].d =		   mat[3, 2];
			}
			else
			{
				// near clipping plane
				planes[(int)VFPLANES.VF_NEAR_PLANE].normal.X = mat[0, 3] + mat[0, 2];
				planes[(int)VFPLANES.VF_NEAR_PLANE].normal.Y = mat[1, 3] + mat[1, 2];
				planes[(int)VFPLANES.VF_NEAR_PLANE].normal.Z = mat[2, 3] + mat[2, 2];
				planes[(int)VFPLANES.VF_NEAR_PLANE].d =		   mat[3, 3] + mat[3, 2];
			}

            // normalize normals
            uint i;
			for (i = 0; i != (int)VFPLANES.VF_PLANE_COUNT; ++i)
			{
				planes[i].normal.Normalize();
			}

			// make bounding box
			//recalculateBoundingBox();
		}

		/*!
			View Frustum depends on Projection  View Matrix
		*/
		/*public Matrix4 getTransform(E_TRANSFORMATION_STATE state)
			{
			u32 index = 0;
			switch (state)
			{
				case video::ETS_PROJECTION:
					index = ETS_PROJECTION; break;
				case video::ETS_VIEW:
					index = ETS_VIEW; break;
				default:
					break;
			}
			return Matrices[index];
		}*/

		/*!
			View Frustum depends on Projection  View Matrix
		*/
		/*public  Matrix4 getTransform(E_TRANSFORMATION_STATE state) 
		{
			UInt32 index = 0;
			switch (state)
			{
				case video::ETS_PROJECTION:
					index = ETS_PROJECTION; break;
				case video::ETS_VIEW:
					index = ETS_VIEW; break;
				default:
					break;
			}
			return matrices[index];
		}*/

		//! Clips a line to the frustum
		/*public bool clipLine(line3d<f32> line) 
		{
			bool wasClipped = false;
			for (u32 i = 0; i < VF_PLANE_COUNT; ++i)
			{
				if (planes[i].classifyPointRelation(line.start) == ISREL3D_FRONT)
				{
					line.start = line.start.getInterpolated(line.end,
							1.f - planes[i].getKnownIntersectionWithLine(line.start, line.end));
					wasClipped = true;
				}
				if (planes[i].classifyPointRelation(line.end) == ISREL3D_FRONT)
				{
					line.end = line.start.getInterpolated(line.end,
							1.f - planes[i].getKnownIntersectionWithLine(line.start, line.end));
					wasClipped = true;
				}
			}
			return wasClipped;
		}*/

		/*public void recalculateBoundingSphere()
		{
			// Find the center
			float shortlen = (getNearLeftUp() - getNearRightUp()).getLength();
			float longlen = (getFarLeftUp() - getFarRightUp()).getLength();

			float farlen = FarNearDistance;
			float fartocenter = (farlen + (shortlen - longlen) * (shortlen + longlen) / (4 * farlen)) / 2;
			float neartocenter = farlen - fartocenter;

			BoundingCenter = cameraPosition + -planes[VF_NEAR_PLANE].Normal * neartocenter;

			// Find the radius
			Vector3 dir[8];
			dir[0] = getFarLeftUp() - BoundingCenter;
			dir[1] = getFarRightUp() - BoundingCenter;
			dir[2] = getFarLeftDown() - BoundingCenter;
			dir[3] = getFarRightDown() - BoundingCenter;
			dir[4] = getNearRightDown() - BoundingCenter;
			dir[5] = getNearLeftDown() - BoundingCenter;
			dir[6] = getNearRightUp() - BoundingCenter;
			dir[7] = getNearLeftUp() - BoundingCenter;

			u32 i = 0;
			float diam[8] = { 0.f };

			for (i = 0; i < 8; ++i)
				diam[i] = dir[i].getLengthSQ();

			float longest = 0;

			for (i = 0; i < 8; ++i)
			{
				if (diam[i] > longest)
					longest = diam[i];
			}

			BoundingRadius = sqrtf(longest);
		}*/
    }
}
