// Irrlicht Engine
// Modified file from https://sourceforge.net/p/irrlicht/code/HEAD/tree/trunk/source/Irrlicht/CSceneCollisionManager.cpp

using System;
using OpenTK;
using OpenTK.Mathematics;

namespace OVFSliceViewerCore.Model.Scene.IrrlichtEngine
{
    public class SceneCollisionManager
    {
        public static Vector3[] GetRayFromScreenCoordinates(Vector2 pos, Camera camera, int radius)
        {
            Vector3[] ray = new Vector3[2 + radius * 8];

            ViewFrustum f = camera.viewFrustum;

            Vector3 farLeftUp = f.GetFarLeftUp();
            Vector3 lefttoright = f.GetFarRightUp() - farLeftUp;
            Vector3 uptodown = f.GetFarLeftDown() - farLeftUp;

            var nx = pos.X / camera.CanvasWidth;
            var ny = pos.Y / camera.CanvasHeight;

            if (/*camera.isOrthogonal()*/ false)
                ray[0] = f.cameraPosition + lefttoright * (nx - 0.5f) + uptodown * (ny - 0.5f);
            else
                ray[0] = f.cameraPosition;

            // Apply ModelView-Matrix to start of ray
            ray[0] = (camera.LookAtTransformationMatrix * new Vector4(ray[0], 1)).Xyz;
            ray[0] = (Matrix4.Invert(Matrix4.Transpose(camera.TranslationMatrix)) * new Vector4(ray[0], 1)).Xyz;

            // Get Ray from Frustum and normalized screen coordinates and then apply View-Matrix
            ray[1] = farLeftUp + lefttoright * nx + uptodown * ny;
            ray[1] = (camera.LookAtTransformationMatrix * new Vector4(ray[1], 1)).Xyz;


            // temporary solution for coloring mutliple triangles
            // --------------------------------------------------
            float stepPercentage = 200f;

            // Create 8 Rays with offsets for every radius step
            for (int i = 1; i <= radius; i++)
            {
                int index = (i - 1) * 8 + 2;

                float t = (float)Math.Sqrt(Math.Pow(i, 2) / 2);
                float offset = 1 + t / stepPercentage;
                float offsetMin = 1 - t / stepPercentage;

                ray[index] = farLeftUp + lefttoright * (nx * offset) + uptodown * (ny * offset);
                ray[index] = (camera.LookAtTransformationMatrix * new Vector4(ray[index], 1)).Xyz;

                ray[index + 1] = farLeftUp + lefttoright * (nx * offsetMin) + uptodown * (ny * offsetMin);
                ray[index + 1] = (camera.LookAtTransformationMatrix * new Vector4(ray[index + 1], 1)).Xyz;

                ray[index + 2] = farLeftUp + lefttoright * (nx * offset) + uptodown * (ny * offsetMin);
                ray[index + 2] = (camera.LookAtTransformationMatrix * new Vector4(ray[index + 2], 1)).Xyz;

                ray[index + 3] = farLeftUp + lefttoright * (nx * offsetMin) + uptodown * (ny * offset);
                ray[index + 3] = (camera.LookAtTransformationMatrix * new Vector4(ray[index + 3], 1)).Xyz;

                offset = 1 + i / stepPercentage;
                offsetMin = 1 - i / stepPercentage;

                ray[index + 4] = farLeftUp + lefttoright * nx + uptodown * (ny * offset);
                ray[index + 4] = (camera.LookAtTransformationMatrix * new Vector4(ray[index + 4], 1)).Xyz;

                ray[index + 5] = farLeftUp + lefttoright * nx + uptodown * (ny * offsetMin);
                ray[index + 5] = (camera.LookAtTransformationMatrix * new Vector4(ray[index + 5], 1)).Xyz;

                ray[index + 6] = farLeftUp + lefttoright * (nx * offset) + uptodown * ny;
                ray[index + 6] = (camera.LookAtTransformationMatrix * new Vector4(ray[index + 6], 1)).Xyz;

                ray[index + 7] = farLeftUp + lefttoright * (nx * offsetMin) + uptodown * ny;
                ray[index + 7] = (camera.LookAtTransformationMatrix * new Vector4(ray[index + 7], 1)).Xyz;
            }

            // ray[0] is the starting point
            // all next vectors are ray directions
            return ray;
        }
    }
}
