using GridRender;
using System.Windows.Forms;

namespace Engine
{
    public static class InputHandler
    {
        static Camera _camera;
        static CursorInfo _cursor;

        public static void Initialize(XtraForm1 form, Camera camera, CursorInfo cursor)
        {
            _camera = camera;
            _cursor = cursor;

            form.MouseDown += (target, arg) =>
            {
                ProcessMouseDown(form, arg);
            };
            form.KeyUp += (target, arg) =>
            {
                ProcessKeyUp(arg);
            };
            form.KeyDown += (target, arg) =>
            {
                ProcessKeyDown(arg);
            };
        }

        public static void ProcessMouseDown(XtraForm1 form, MouseEventArgs arg)
        {
            if (arg.Button == MouseButtons.Right)
            {
                if (_cursor.IsFree())
                {
                    _cursor.Lock();
                    Cursor.Hide();
                }
                else
                {
                    _cursor.Unlock();
                    Cursor.Show();
                    Cursor.Position = new System.Drawing.Point(form.Size.Width / 2, form.Size.Height / 2);
                    _cursor.Update(Cursor.Position);
                }
            }
        }

        public static void ProcessKeyUp(KeyEventArgs arg)
        {
            if (arg.KeyCode == Keys.W)
                KeyboardState.KeyUp(Keys.W);

            if (arg.KeyCode == Keys.S)
                KeyboardState.KeyUp(Keys.S);

            if (arg.KeyCode == Keys.ShiftKey)
                KeyboardState.KeyUp(Keys.ShiftKey);

            if (arg.KeyCode == Keys.Space)
                KeyboardState.KeyUp(Keys.Space);

            if (arg.KeyCode == Keys.A)
                KeyboardState.KeyUp(Keys.A);

            if (arg.KeyCode == Keys.D)
                KeyboardState.KeyUp(Keys.D);
        }

        public static void ProcessKeyDown(KeyEventArgs arg)
        {
            if (arg.KeyCode == Keys.F)
                _camera.ChangeFocus();

            if (arg.KeyCode == Keys.W)
                KeyboardState.KeyDown(Keys.W);

            if (arg.KeyCode == Keys.S)
                KeyboardState.KeyDown(Keys.S);

            if (arg.KeyCode == Keys.ShiftKey)
                KeyboardState.KeyDown(Keys.ShiftKey);

            if (arg.KeyCode == Keys.Space)
                KeyboardState.KeyDown(Keys.Space);

            if (arg.KeyCode == Keys.A)
                KeyboardState.KeyDown(Keys.A);

            if (arg.KeyCode == Keys.D)
                KeyboardState.KeyDown(Keys.D);
        }
    }
}