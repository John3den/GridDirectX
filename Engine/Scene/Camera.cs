using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Windows.Forms;
namespace Engine
{
    public class Camera
    {
        Vector3 _lookDirection;
        Vector3 _leftDirection;
        Vector3 _position;
        Vector3 _upDirection;
        Matrix _view;
        float _pitch = 0;
        float _yaw = 0;
        bool _focused = true;
        public Camera()
        {
            _leftDirection = new Vector3((float)Math.Sin(_yaw + Math.PI / 2) * (float)Math.Sin(_pitch), (float)Math.Cos(_pitch), (float)Math.Cos(_yaw + Math.PI / 2) * (float)Math.Sin(_pitch));
            _lookDirection = new Vector3((float)Math.Sin(_yaw) * (float)Math.Sin(_pitch), (float)Math.Cos(_pitch), (float)Math.Cos(_yaw) * (float)Math.Sin(_pitch));
            _upDirection = new Vector3((float)Math.Sin(_yaw) * (float)Math.Sin(_pitch + Math.PI / 2), (float)Math.Cos(_pitch + Math.PI / 2), (float)Math.Cos(_yaw) * (float)Math.Sin(_pitch + Math.PI / 2));
            _view = Matrix.LookAtLH(_position, _position + _lookDirection, _upDirection);
            _position = new Vector3(0, 0, -3);
        }
        public void Update()
        {
            float speed = 1f;
            if (KeyboardState.IsPressed(Keys.W))
                _position += _lookDirection * speed * RenderLoopControl._deltaTime;
            if (KeyboardState.IsPressed(Keys.S))
                _position -= _lookDirection * speed * RenderLoopControl._deltaTime;
            if (KeyboardState.IsPressed(Keys.Q))
                _position.Y += speed * RenderLoopControl._deltaTime;
            if (KeyboardState.IsPressed(Keys.E))
                _position.Y -= speed * RenderLoopControl._deltaTime;
            if (KeyboardState.IsPressed(Keys.A))
            {
                    _position += _leftDirection * speed * RenderLoopControl._deltaTime;
            }
            if (KeyboardState.IsPressed(Keys.D))
            {
                    _position -= _leftDirection * speed * RenderLoopControl._deltaTime;
            }
            if (_focused)
                _lookDirection = -_position;
            else
                _lookDirection = new Vector3((float)Math.Sin(_yaw) * (float)Math.Sin(_pitch), (float)Math.Cos(_pitch), (float)Math.Cos(_yaw) * (float)Math.Sin(_pitch )); 
            _leftDirection = Vector3.Cross(_lookDirection, Vector3.Up);
            _view = Matrix.LookAtLH(_position, _position + _lookDirection, Vector3.UnitY);
        }
        public void UpdView()
        {
            _view = Matrix.LookAtLH(_position, _position + _lookDirection, Vector3.UnitY);
        }
        public void Unfocus()
        {
            Vector3 normal = Vector3.Normalize(-_position);
            _pitch = (float) (Math.Acos(normal.Y)  );
            if(normal.Z>0)
                _yaw =  (float)Math.Atan(normal.X/ normal.Z);
            else
                _yaw = (float)Math.Atan(normal.X / normal.Z) + (float)Math.PI;
            Console.WriteLine(_pitch + " " + _yaw);

        }
        public void ChangeFocus()
        {
            _focused = !_focused;
            if (!_focused)
            {
                Unfocus();
            }
        }
        public void Rotate(Vector2 rotationVector)
        {
            float pitchShift = rotationVector.X;
            float yawShift = rotationVector.Y;
            _pitch += pitchShift;
            _yaw += yawShift;
            if (_pitch < 0)
            {
                _pitch = 0.0001f;
            }
            if (_pitch > Math.PI)
            {
                _pitch = (float)Math.PI - 0.0001f;
            }
            Update();
        }
        public void Move(Vector3 movementVector)
        {
            _position += movementVector;
            Update();
        }
        public Matrix GetView()
        {
            return _view;
        }
    }
}