using OpenTK;
using System;

namespace OVFSliceViewerBusinessLayer.Classes
{
    public class MotionTracker
    {
        bool _isMoving = false;
        Vector2 _start = Vector2.Zero;
        public void Start(Vector2 position)
        {
            _start = position;
            _isMoving = true;
        }

        public void Move(Vector2 position, Action<Vector2> action)
        {
            if (!_isMoving)
            {
                _start = position;
                _isMoving = true;
            }
            else
            {
                var delta = position - _start;
                _start = position;
                action(delta);
            }
        }

        public void Stop()
        {
            _isMoving = false;
        }
    }
}
