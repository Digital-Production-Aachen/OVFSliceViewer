using OpenTK;
using System;
using OVFSliceViewerBusinessLayer.Model;
using SliceViewerBusinessLayer.Classes;
using System.Diagnostics;
using OpenTK.Mathematics;
using AutomatedBuildChain.Proto;

namespace OVFSliceViewerBusinessLayer.Classes
{
    // Todo: seperate Matrix for movement and rotation
    
    public class Camera : IZoomable, IRotateable, OVFSliceViewerBusinessLayer.Model.IModelViewProjection
    {
        Vector3 _position;
        Vector3 _cameraTarget;
        Vector3 _translation = new Vector3();

        protected SceneController _scene;
        protected float _fieldOfView;
        protected float _aspectRatio;
        protected float _canvasWidth;
        protected float _canvasHeight;
        protected float _zoomfactor = 1f;
        
        float _yaw = 0;
        float _pitch = 0;

        public float zNear { get; set; } = 0.1f;
        public float zFar { get; set; } = 100f;
        public float CanvasWidth => _canvasWidth;
        public float CanvasHeight => _canvasHeight;
        public float ObjectHeight { get; set; }
        //public Vector2 CameraPosition { get { return new Vector2(_position.X, _position.Y); } set { CameraPosition = value; } }
        public Vector3 CameraDirection => _cameraTarget - _position;
        public Vector3 CameraPosition => _position;

        public Vector3 Translation => _translation;

        public Matrix4 RotationMatrixYaw { get; protected set; } = Matrix4.Identity;
        public Matrix4 RotationMatrixPitch { get; protected set; } = Matrix4.Identity;
        public Matrix4 LookAtTransformationMatrix =>
            Matrix4.LookAt
            (
                (RotationMatrixYaw * RotationMatrixPitch * new Vector4(_position, 1)).Xyz,
                (RotationMatrixYaw * new Vector4(_cameraTarget, 1)).Xyz,
                (RotationMatrixYaw * RotationMatrixPitch * new Vector4(Vector3.UnitY, 1)).Xyz
                );

        public Matrix4 ProjectionMatrix => Matrix4.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, zNear, zFar);

        public Matrix4 TranslationMatrix { get; protected set; } = Matrix4.Identity;

        public Matrix4 ModelViewProjection => TranslationMatrix * LookAtTransformationMatrix * ProjectionMatrix;

        public ViewFrustum viewFrustum;


        public Camera(SceneController scene, float canvasWidth, float canvasHeight)
        {
            _scene = scene;
            _canvasWidth = canvasWidth;
            _canvasHeight = canvasHeight;
            _fieldOfView = ((float)Math.PI / 180) * 80;

            _aspectRatio = Convert.ToSingle(_canvasWidth) / Convert.ToSingle(_canvasHeight);
            _position = new Vector3(0, 0, 50f);
            _cameraTarget = Vector3.Zero;
            TranslationMatrix = Matrix4.CreateTranslation(new Vector3(0));
            viewFrustum = new ViewFrustum(ModelViewProjection, false);
        }
        public void Resize(int width, int height)
        {
            _canvasWidth = width;
            _canvasHeight = height;
            _aspectRatio = Convert.ToSingle(_canvasWidth) / Convert.ToSingle(_canvasHeight);
        }

        public void setCameraPosition(float x, float y, float z)
        {
            _position = new Vector3(x, y, z);
        }

        public void MoveToPosition2D(Vector2 position)
        {
            if (RotationMatrixYaw != Matrix4.Identity || RotationMatrixPitch != Matrix4.Identity)
            {
                RotationMatrixYaw = Matrix4.Identity;
                RotationMatrixPitch = Matrix4.Identity;
                _yaw = 0;
                _pitch = 0;
            }
            _translation = new Vector3(-position);
            TranslationMatrix = Matrix4.CreateTranslation(_translation);
            _scene.Render();
        }
        public void Move(Vector2 delta) // Basically it moves Camera and focus (target)
        {
            if (delta.Length < 0.00001)
            {
                return;
            }
            var delta3 = new Vector3(-delta);
            delta3 = ConvertToCanvasCoordinates(delta3);

            var temp = (RotationMatrixYaw * new Vector4(delta3));
            temp = temp * delta3.Xy.Length / temp.Length;

            _translation += temp.Xyz;

            var temp2 = Matrix4.CreateTranslation(_translation);//temp.Xyz);
            TranslationMatrix = temp2;
            UpdateFrustum();
            _scene.Render();
        }

        protected Vector3 ConvertToCanvasCoordinates(Vector3 delta)
        {
            delta.X = -Convert.ToSingle((2f * delta.X / _canvasWidth) * Math.Tan(_fieldOfView / 2f) * (_position.Z - ObjectHeight) * _aspectRatio);
            delta.Y = Convert.ToSingle((2f * delta.Y / _canvasHeight) * Math.Tan(_fieldOfView / 2f) * (_position.Z - ObjectHeight));
            return delta;
        }

        public void Zoom(bool makeBigger, bool fastZoom = false)
        {
            float zoom = _zoomfactor;
            if (fastZoom)
                zoom *= 5;

            if (makeBigger && _position.Z > -zFar)
                _position.Z -= zoom;
            else if (_position.Z < zFar)
                _position.Z += zoom;

            UpdateFrustum();
            _scene.Render();
        }

        public void ChangeHeight(float deltaZ)
        {
            ObjectHeight += deltaZ;
            _position.Z += deltaZ;
            _scene.Render();
        }

        public void Rotate(Vector2 delta)
        {
            _yaw += MathHelper.DegreesToRadians(delta.X);
            _pitch += MathHelper.DegreesToRadians(delta.Y);
            _yaw = Convert.ToSingle(_yaw % (2 * Math.PI));
            _pitch = Convert.ToSingle(_pitch % Math.PI);

            if (_pitch > Math.PI / 2)
            {
                _pitch = Convert.ToSingle(Math.PI / 2) - 0.01f;
            }
            else if (_pitch < -Math.PI / 2)
            {
                _pitch = -Convert.ToSingle(Math.PI / 2) + 0.01f;
            }

            RotationMatrixYaw = Matrix4.CreateFromAxisAngle(Vector3.UnitZ, _yaw);
            RotationMatrixPitch = Matrix4.CreateFromAxisAngle(Vector3.UnitX, _pitch);
            UpdateFrustum();
            _scene.Render();
        }

        public void UpdateFrustum()
        {
            viewFrustum.cameraPosition = _position;
            viewFrustum.SetFrom(ProjectionMatrix, false);
        }
    }
}
