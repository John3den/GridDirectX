using GridRender;
using System.Windows.Forms;

namespace Engine
{
    public class InputHandler
    {
        Camera _camera;
        CursorInfo _cursor;
        public InputHandler(XtraForm1 form,Camera cam, CursorInfo cursr)
        {
            _camera = cam;
            _cursor = cursr;
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
        public void ProcessMouseDown(XtraForm1 form, MouseEventArgs arg)
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
        public void ProcessKeyUp(KeyEventArgs arg)
        {
            if (arg.KeyCode == Keys.W)
                KeyboardState.KeyUp(Keys.W);
            if (arg.KeyCode == Keys.S)
                KeyboardState.KeyUp(Keys.S);
            if (arg.KeyCode == Keys.Q)
                KeyboardState.KeyUp(Keys.Q);
            if (arg.KeyCode == Keys.E)
                KeyboardState.KeyUp(Keys.E);
            if (arg.KeyCode == Keys.A)
                KeyboardState.KeyUp(Keys.A);
            if (arg.KeyCode == Keys.D)
                KeyboardState.KeyUp(Keys.D);
        }
        public void ProcessKeyDown(KeyEventArgs arg)
        {
            if (arg.KeyCode == Keys.F)
            {
                _camera.ChangeFocus();
            }
            if (arg.KeyCode == Keys.W)
                KeyboardState.KeyDown(Keys.W);
            if (arg.KeyCode == Keys.S)
                KeyboardState.KeyDown(Keys.S);
            if (arg.KeyCode == Keys.Q)
                KeyboardState.KeyDown(Keys.Q);
            if (arg.KeyCode == Keys.E)
                KeyboardState.KeyDown(Keys.E);
            if (arg.KeyCode == Keys.A)
                KeyboardState.KeyDown(Keys.A);
            if (arg.KeyCode == Keys.D)
                KeyboardState.KeyDown(Keys.D);
        }
    }
}