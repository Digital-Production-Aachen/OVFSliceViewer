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

        protected float _fieldOfView;
        protected float _aspectRatio;
        protected float _canvasWidth;
        protected float _canvasHeight;
        protected float _zoomfactor = 1f;
        float _yaw = 0;
        float _pitch = 0;
        public float ObjectHeight { get; set; }
        public Matrix3 RotationMatrixYaw { get; protected set; } = Matrix3.Identity;
        public Matrix3 RotationMatrixPitch { get; protected set; } = Matrix3.Identity;
        public Matrix4 TransformationMatrix =>
            Matrix4.LookAt
            (
                (RotationMatrixYaw * RotationMatrixPitch * _position),
                (RotationMatrixYaw * _cameraTarget),
                RotationMatrixYaw * RotationMatrixPitch * Vector3.UnitY
                );
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

            if (RotationMatrixYaw != Matrix3.Identity || RotationMatrixPitch != Matrix3.Identity)
            {
                RotationMatrixYaw = Matrix3.Identity;
                RotationMatrixPitch = Matrix3.Identity;
            }

            _position.X = position.X;
            _position.Y = position.Y;
            
            _cameraTarget.X += deltaX;
            _cameraTarget.Y += deltaY;
        }
        public void Move(Vector2 delta) // Basically it moves Camera and focus (target)
        {
            var delta3 = new Vector3(delta);
            delta3 = ConvertToCanvasCoordinates(delta3);

            _position = _position + delta3;
            _cameraTarget = _cameraTarget + delta3;
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
            //var deltaZ = newZ - ObjectHeight;

            ObjectHeight += deltaZ;
            _position.Z += deltaZ;
        }

        public void Rotate(Vector2 delta)
        {
            _yaw += MathHelper.DegreesToRadians(delta.X);
            _pitch += MathHelper.DegreesToRadians(delta.Y);
            _yaw = Convert.ToSingle(_yaw % Math.PI);
            _pitch = Convert.ToSingle(_pitch % Math.PI);

            if (_pitch > Math.PI / 2)
            {
                _pitch = Convert.ToSingle(Math.PI / 2) - 0.01f;
            }
            else if (_pitch < -Math.PI / 2)
            {
                _pitch = -Convert.ToSingle(Math.PI / 2) + 0.01f;
            }

            RotationMatrixYaw = Matrix3.CreateFromAxisAngle(Vector3.UnitZ, _yaw);
            RotationMatrixPitch = Matrix3.CreateFromAxisAngle(Vector3.UnitX, _pitch);
        }
    }
}
