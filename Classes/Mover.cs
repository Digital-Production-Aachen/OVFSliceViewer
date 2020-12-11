using System;
using System.Numerics;

namespace LayerViewer
{
    public class Mover
    {
        Vector2 _startPosition;
        Vector2 _stopPosition;
        Vector2 _delta => _stopPosition - _startPosition;
        bool _mouseIsDown = false;
        IMoveable _moveableObject;

        public Mover(IMoveable moveableObject)
        {
            _moveableObject = moveableObject;
        }

        public void StartMovement(Vector2 position)
        {
            _startPosition = position;
            _stopPosition = position;
            _mouseIsDown = true;
            _moveableObject.StartMove();
        }
        
        public void Move(Vector2 position)
        {
            if (_mouseIsDown)
            {
                _stopPosition = position;
                _moveableObject.Move(_delta);
            }
        }
        
        public void StopMovement(Vector2 position)
        {
            _stopPosition = position;
            _mouseIsDown = false;
            _moveableObject.StopMove();
        }
    }

    public interface IMoveable
    {
        void StartMove();
        void StopMove();
        void Move(Vector2 delta);
    }
}
