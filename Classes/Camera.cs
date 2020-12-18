using OpenTK;
using System;
using System.Windows.Media.Media3D;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace OVFSliceViewer
{
    // Todo: seperate Matrix for movement and rotation

    public class Camera : IZoomable, IRotateable
    {
        Vector3 _position;    
        Vector3 _cameraTarget;
        Vector3 _translation = new Vector3();

        protected float _fieldOfView;
        protected float _aspectRatio;
        protected float _canvasWidth;
        protected float _canvasHeight;
        protected float _zoomfactor = 1f;
        float _yaw = 0;
        float _pitch = 0;
        public bool Is2D => RotationMatrixYaw == Matrix4.Identity && RotationMatrixPitch == Matrix4.Identity;
        public float ObjectHeight { get; set; }
        public Matrix4 RotationMatrixYaw { get; protected set; } = Matrix4.Identity;
        public Matrix4 RotationMatrixPitch { get; protected set; } = Matrix4.Identity;
        public Matrix4 LookAtTransformationMatrix =>
            Matrix4.LookAt
            (
                (RotationMatrixYaw * RotationMatrixPitch * new Vector4(_position,1)).Xyz,
                (RotationMatrixYaw * new Vector4(_cameraTarget,1)).Xyz,
                (RotationMatrixYaw * RotationMatrixPitch * new Vector4(Vector3.UnitY, 1)).Xyz
                );

        public Matrix4 ProjectionMatrix => Matrix4.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, 0.1f, 200f);

        public Matrix4 TranslationMatrix { get; protected set; } = Matrix4.Identity;
        public Camera(float canvasWidth, float canvasHeight)
        {
            _canvasWidth = canvasWidth;
            _canvasHeight = canvasHeight;
            _fieldOfView = ((float)Math.PI / 180) * 80;
            
            _aspectRatio = Convert.ToSingle(_canvasWidth) / Convert.ToSingle(_canvasHeight);
            _position = new Vector3(0, 0, 50f);
            _cameraTarget = Vector3.Zero;
        }
        
        public void MoveToPosition2D(Vector2 position)
        {
            var deltaX = position.X - _position.X;
            var deltaY = position.Y - _position.Y;

            if (RotationMatrixYaw != Matrix4.Identity || RotationMatrixPitch != Matrix4.Identity)
            {
                RotationMatrixYaw = Matrix4.Identity;
                RotationMatrixPitch = Matrix4.Identity;

                _yaw = 0;
                _pitch = 0;
            }

            _position.X = position.X;
            _position.Y = position.Y;
            
            _cameraTarget.X += deltaX;
            _cameraTarget.Y += deltaY;
        }
        public void Move(Vector2 delta) // Basically it moves Camera and focus (target)
        {
            var delta3 = new Vector3(-delta);
            delta3 = ConvertToCanvasCoordinates(delta3);

            _translation += delta3;

            TranslationMatrix = Matrix4.CreateTranslation((RotationMatrixYaw * new Vector4(_translation)).Xyz);
        }

        protected Vector3 ConvertToCanvasCoordinates(Vector3 delta)
        {
            delta.X = -Convert.ToSingle((2f * delta.X / _canvasWidth) * Math.Tan(_fieldOfView / 2f) * (_position.Z - ObjectHeight) * _aspectRatio);
            delta.Y = Convert.ToSingle((2f * delta.Y / _canvasHeight) * Math.Tan(_fieldOfView / 2f) * (_position.Z - ObjectHeight));
            return delta;
        }

        public void Zoom(bool makeBigger)
        {
            Vector3 newPosition;
            var delta = (_position - _cameraTarget).Length;
            
            if (makeBigger)
            {
                newPosition = _cameraTarget + (delta - _zoomfactor) * ((_position - _cameraTarget).Normalized());
            }
            else
            {
                newPosition = _cameraTarget + (delta + _zoomfactor) * ((_position - _cameraTarget).Normalized());
            }

            if ((newPosition - _cameraTarget).Length < 0.1f)
            {
                newPosition = _cameraTarget + (_position - _cameraTarget).Normalized() * 0.11f;
            }
            else if ((newPosition - _cameraTarget).Length > 100f)
            {
                newPosition = _cameraTarget + (_position - _cameraTarget).Normalized() * 99f;
            }

            _position = newPosition;
        }

        public void ChangeHeight(float deltaZ)
        {
            ObjectHeight += deltaZ;
            _position.Z += deltaZ;
        }

        public void Rotate(Vector2 delta)
        {
            _yaw += MathHelper.DegreesToRadians(delta.X);
            _pitch += MathHelper.DegreesToRadians(delta.Y);
            _yaw = Convert.ToSingle(_yaw % (2*Math.PI));
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
        }
    }
}
