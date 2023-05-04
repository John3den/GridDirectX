using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Windows.Forms;
namespace Engine
{
    public class Camera
    {
        Vector3 lookDirection;
        Vector3 leftDirection;
        Vector3 position;
        Vector3 upDirection;
        Matrix view;
        float pitch = 0;
        float yaw = 0;
        bool focused = true;
        public Camera()
        {
            leftDirection = new Vector3((float)Math.Sin(yaw + Math.PI / 2) * (float)Math.Sin(pitch), (float)Math.Cos(pitch), (float)Math.Cos(yaw + Math.PI / 2) * (float)Math.Sin(pitch));
            lookDirection = new Vector3((float)Math.Sin(yaw) * (float)Math.Sin(pitch), (float)Math.Cos(pitch), (float)Math.Cos(yaw) * (float)Math.Sin(pitch));
            upDirection = new Vector3((float)Math.Sin(yaw) * (float)Math.Sin(pitch + Math.PI / 2), (float)Math.Cos(pitch + Math.PI / 2), (float)Math.Cos(yaw) * (float)Math.Sin(pitch + Math.PI / 2));
            view = Matrix.LookAtLH(position, position + lookDirection, upDirection);
            position = new Vector3(0, 0, -3);
        }
        public void Update()
        {
            float speed = 1f;
            if (KeyboardState.IsPressed(Keys.W))
                position += lookDirection * speed * RenderLoopControl.deltaTime;
            if (KeyboardState.IsPressed(Keys.S))
                position -= lookDirection * speed * RenderLoopControl.deltaTime;
            if (KeyboardState.IsPressed(Keys.Q))
                position.Y += speed * RenderLoopControl.deltaTime;
            if (KeyboardState.IsPressed(Keys.E))
                position.Y -= speed * RenderLoopControl.deltaTime;
            if (KeyboardState.IsPressed(Keys.A))
            {
                    position += leftDirection * speed * RenderLoopControl.deltaTime;
            }
            if (KeyboardState.IsPressed(Keys.D))
            {
                    position -= leftDirection * speed * RenderLoopControl.deltaTime;
            }
            if (focused)
                lookDirection = -position;
            else
                lookDirection = new Vector3((float)Math.Sin(yaw) * (float)Math.Sin(pitch), (float)Math.Cos(pitch), (float)Math.Cos(yaw) * (float)Math.Sin(pitch )); 
            leftDirection = Vector3.Cross(lookDirection, Vector3.Up);
            view = Matrix.LookAtLH(position, position + lookDirection, Vector3.UnitY);
        }
        public void UpdView()
        {
            view = Matrix.LookAtLH(position, position + lookDirection, Vector3.UnitY);
        }
        public void Unfocus()
        {
            Vector3 normal = Vector3.Normalize(-position);
            pitch = (float) (Math.Acos(normal.Y)  );
            if(normal.Z>0)
                yaw =  (float)Math.Atan(normal.X/ normal.Z);
            else
                yaw = (float)Math.Atan(normal.X / normal.Z) + (float)Math.PI;
            Console.WriteLine(pitch + " " + yaw);

        }
        public void ChangeFocus()
        {
            focused = !focused;
            if (!focused)
            {
                Unfocus();
            }
        }
        public void Rotate(Vector2 rotationVector)
        {
            float pitchShift = rotationVector.X;
            float yawShift = rotationVector.Y;
            pitch += pitchShift;
            yaw += yawShift;
            if (pitch < 0)
            {
                pitch = 0.0001f;
            }
            if (pitch > Math.PI)
            {
                pitch = (float)Math.PI - 0.0001f;
            }
            Update();
        }
        public void Move(Vector3 movementVector)
        {
            position += movementVector;
            Update();
        }
        public Matrix GetView()
        {
            return view;
        }
    }
}