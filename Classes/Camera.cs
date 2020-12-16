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
        Vector3 _rotationStartYawAndPitch = new Vector3(0, 0, 0);
        
        Vector3 _cameraTarget;
        Vector3 _upAxis;

        protected bool _isMoving = false;
        protected float _fieldOfView;
        protected float _aspectRatio;
        protected float _canvasWidth;
        protected float _canvasHeight;
        protected float _zoomfactor = 1f;
        float _yaw = 0;
        float _pitch = 0;
        Vector2 _sensitivity = new Vector2(1, 1);
        readonly float _baseSensitivity = 0.01f;

        public float ObjectHeight { get; set; }
        public Matrix4 TransformationMatrix =>
            Matrix4.LookAt
            (
                (RotationMatrix * RotationMatrix2 * _position),
                (RotationMatrix * _cameraTarget),
                RotationMatrix * RotationMatrix2 * _upAxis
                );
        public Matrix3 RotationMatrix { get; protected set; } = Matrix3.Identity;
        public Matrix3 RotationMatrix2 { get; protected set; } = Matrix3.Identity;

        public Matrix3 TranslationMatrix { get; protected set; } = Matrix3.Identity;


        public Camera(float canvasWidth, float canvasHeight)
        {
            _canvasWidth = canvasWidth;
            _canvasHeight = canvasHeight;

            _fieldOfView = ((float)Math.PI / 180) * 80;
            _aspectRatio = Convert.ToSingle(_canvasWidth) / Convert.ToSingle(_canvasHeight);

            InitCamera();

            
            //_focus = _position;
            //_focus.Z = 0;

            var rotationMatrix = Matrix3.CreateFromAxisAngle(Vector3.UnitZ, 0);   // Create rotation matrix
            RotationMatrix = rotationMatrix;
        }

        protected void InitCamera()
        {
            _position = new Vector3(0, 0, 50f);
            _cameraTarget = Vector3.Zero;
            _upAxis = Vector3.UnitY;
        }

        public void MoveToPosition2D(Vector2 position)
        {
            var deltaX = position.X - _position.X;
            var deltaY = position.Y - _position.Y;

            if (RotationMatrix != Matrix3.Identity || RotationMatrix2 != Matrix3.Identity)
            {
                RotationMatrix = Matrix3.Identity;
                RotationMatrix2 = Matrix3.Identity;
                //_position.Z = 25f;
                //_cameraTarget.Z = 0f;
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
            var delta = (_position - _cameraTarget).Length;
            Vector3 newPosition = new Vector3();
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

        public void ChangeHeight(float newZ)
        {
            ObjectHeight = newZ;
        }

        public void Rotate(Vector2 delta)
        {
            var sensitivity = _sensitivity * _baseSensitivity;

            var delta3 = new Vector3(delta.X * sensitivity.X, delta.Y * sensitivity.Y, 0);
            delta3 = (_rotationStartYawAndPitch - delta3);

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

            RotationMatrix = Matrix3.CreateFromAxisAngle(Vector3.UnitZ, _yaw);
            RotationMatrix2 = Matrix3.CreateFromAxisAngle(Vector3.UnitX, _pitch);
        }
    }
}
