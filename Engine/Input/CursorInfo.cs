using DevExpress.XtraLayout;
using SharpDX;
using System;
using Point = System.Drawing.Point;
namespace Engine
{
    public class CursorInfo
    {
        Point previousPosition;
        Point currentPosition;
        Point positionDelta;
        float sensitivity = 0.001f;
        bool isFree = true;
        public CursorInfo(Point startingPosition)
        {
            previousPosition = startingPosition; 
            currentPosition = startingPosition;
        }
        public void Update(Point newPosition)
        {
            currentPosition = newPosition;
            positionDelta = new Point(currentPosition.X - previousPosition.X, currentPosition.Y - previousPosition.Y);
            previousPosition = currentPosition;
        }
        public Vector2 GetDelta()
        {
            float x = positionDelta.X * sensitivity;
            float y = positionDelta.Y * sensitivity;
            return new Vector2(x, y);
        }
        public void Lock()
        {
            isFree = false;
        }
        public void Unlock() 
        {
            isFree = true;
        }
        public bool IsFree()
        {
            return isFree;
        }
    }
}