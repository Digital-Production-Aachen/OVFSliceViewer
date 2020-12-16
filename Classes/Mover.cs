//using System;
//using OpenTK;

//namespace OVFSliceViewer
//{
    //public class AbstrMover
    //{
    //    protected Vector2 _size;
    //    protected Vector2 _startPosition;
    //    protected Vector2 _stopPosition;
    //    protected Vector3 _delta => new Vector3(_stopPosition - _startPosition);
    //    protected bool _mouseIsDown = false;

//    public AbstrMover(Vector2 size)
//    {
//        _size = size;
//    }

//    public virtual void StartMovement(Vector2 position)
//    {
//        _startPosition = position;
//        _stopPosition = position;
//        _mouseIsDown = true;
//    }

//    public virtual void Move(Vector2 position)
//    {
//        _stopPosition = position;
//    }

//    public virtual void StopMovement(Vector2 position)
//    {
//        _stopPosition = position;
//        _mouseIsDown = false;
//    }
//}
//public class Mover : AbstrMover
//{
//    IMotion _moveableObject;

//    public Mover(IMotion moveableObject, Vector2 size): base(size)
//    {
//        _moveableObject = moveableObject;
//    }

//    public override void StartMovement(Vector2 position)
//    {
//        base.StartMovement(position);
//        _moveableObject.StartMove();
//    }

//    public override void Move(Vector2 position)
//    {
//        if (_mouseIsDown)
//        {
//            base.Move(position);
//            _moveableObject.Move(_delta);
//        }
//    }

//    public override void StopMovement(Vector2 position)
//    {
//        base.StopMovement(position);
//        _moveableObject.StopMove();
//    }
//}

//    public class Rotater : AbstrMover
//    {
//        IRotateable _rotatableObject;

//        public Rotater(IRotateable rotatableObject, Vector2 size) : base(size)
//        {
//            _rotatableObject = rotatableObject;
//        }

//        public override void StartMovement(Vector2 position)
//        {
//            var sensitivity = (position - _size/2);
//            // s(0) = 10;
//            // s(size/2) = 1
//            // s(x) = m * x + 10
//            // m = -9/1
//            sensitivity.X = (sensitivity.X / (_size.X / 2));
//            sensitivity.Y = (sensitivity.Y / (_size.Y / 2));

//            sensitivity.X = 10 - 9 * Math.Abs(sensitivity.X);
//            sensitivity.Y = 10 - 9 * Math.Abs(sensitivity.Y);

//            base.StartMovement(position);

//            _rotatableObject.StartRotation(sensitivity);
//        }

//        public override void Move(Vector2 position)
//        {
//            if (_mouseIsDown)
//            {
//                base.Move(position);
//                _rotatableObject.Rotate(_delta);
//            }
//        }

//        public override void StopMovement(Vector2 position)
//        {
//            base.StopMovement(position);
//            _rotatableObject.StopRotation();
//        }
//    }
//}
