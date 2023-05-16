using System.Diagnostics;
using System.Windows.Forms;
using GridRender;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;

namespace Engine
{
    public class RenderLoopControl
    {
        public static float DeltaTime;
        public readonly Grid GridObject;

        CommandList[] _commandLists;
        Stopwatch _clock;
        Stopwatch _fpsTimer;
        Stopwatch _frameTimer = new Stopwatch();
        Camera _camera = new Camera();
        CursorInfo _cursor;
        Renderer _renderer;
        XtraForm1 _form;
        int _fpsCounter = 0;

        public RenderLoopControl()
        {   
            //Create the form
            _form = new XtraForm1();
            _form.loopControl = this;

            //Initialize command list
            _commandLists = new CommandList[Renderer.THREAD_COUNT];

            _cursor = new CursorInfo(Cursor.Position);

            InputHandler.Initialize(_form, _camera, _cursor);
            _renderer = new Renderer(_form, _camera);

            //Read grid data and initialize grid and its properties
            GridObject = new Grid(_renderer.Device, _form, "../../Resources/grid.bin");

            SetupTimers();

            RenderLoop.Run(_form, () => { Frame(); });
        }

        private void Frame()
        {
            UpdateFrameTimer();

            //Reset cursor position to the center of the form
            if (!_cursor.IsFree())
            {
                ResetCursorPosition();
            }

            _camera.Update();
            GridObject.Update();

            //Display text on top
            UpdateFPSDisplay();

            _renderer.SetupPipeline();
            _renderer.RenderDeferred(Renderer.THREAD_COUNT, _commandLists, GridObject);

            //Execute command list and dispose
            for (int i = 0; i < Renderer.THREAD_COUNT; i++)
            {
                ExecuteCommand(i);
            }

            _renderer.SwapChain.Present(0, PresentFlags.None);
        }

        private void UpdateFPSDisplay()
        {
            _fpsCounter++;
            if (_fpsTimer.ElapsedMilliseconds > 1000)
            {
                _form.Text = string.Format("SharpDX - FPS: {0:F2} ({1:F2}ms)", 1000.0 * _fpsCounter / _fpsTimer.ElapsedMilliseconds, (float)_fpsTimer.ElapsedMilliseconds / _fpsCounter);
                ResetFPSTimer();
                _fpsCounter = 0;
            }
        }

        private void SetupTimers()
        {
            _clock = new Stopwatch();
            _clock.Start();
            _fpsTimer = new Stopwatch();
            _fpsTimer.Start();
        }

        private void ResetCursorPosition()
        {
            _cursor.Update(Cursor.Position);
            _camera.Rotate(-new Vector2(_cursor.GetDelta().Y, _cursor.GetDelta().X));
            Cursor.Position = new System.Drawing.Point(_form.Left + _form.Size.Width / 2, _form.Top + _form.Size.Height / 2);
            _cursor.Update(Cursor.Position);
        }

        private void ExecuteCommand(int i)
        {
            var commandList = _commandLists[i];
            _renderer.Immediate.ExecuteCommandList(commandList, false);
            commandList.Dispose();
            _commandLists[i] = null;
        }

        private void UpdateFrameTimer()
        {
            DeltaTime = (float)_frameTimer.ElapsedMilliseconds / 1000;
            _frameTimer.Restart();
        }

        private void ResetFPSTimer()
        {
            _fpsTimer.Reset();
            _fpsTimer.Stop();
            _fpsTimer.Start();
        }
    }
}