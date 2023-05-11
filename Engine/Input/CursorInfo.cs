using SharpDX;
using Point = System.Drawing.Point;
namespace Engine
{
    public class CursorInfo
    {
        Point _previousPosition;
        Point _currentPosition;
        Point _positionDelta;
        float _sensitivity = 0.001f;
        bool _isFree = true;

        public CursorInfo(Point startingPosition)
        {
            _previousPosition = startingPosition; 
            _currentPosition = startingPosition;
        }

        public void Update(Point newPosition)
        {
            _currentPosition = newPosition;
            _positionDelta = new Point(_currentPosition.X - _previousPosition.X, _currentPosition.Y - _previousPosition.Y);
            _previousPosition = _currentPosition;
        }

        public Vector2 GetDelta()
        {
            float x = _positionDelta.X * _sensitivity;
            float y = _positionDelta.Y * _sensitivity;
            return new Vector2(x, y);
        }

        public void Lock()
        {
            _isFree = false;
        }

        public void Unlock() 
        {
            _isFree = true;
        }

        public bool IsFree()
        {
            return _isFree;
        }
    }
}