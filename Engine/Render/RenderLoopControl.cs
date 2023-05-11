using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using GridRender;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Buffer = SharpDX.Direct3D11.Buffer;
using Color = SharpDX.Color;

using MapFlags = SharpDX.Direct3D11.MapFlags;
using Engine;
using CheckBox = DevExpress.XtraEditors.CheckEdit;
using DevExpress.XtraEditors;
using SharpDX.MediaFoundation;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;


namespace Engine
{
    public class RenderLoopControl
    {
        public static float DeltaTime;
        public readonly Grid Grid;

        CommandList[] _commandLists;
        Stopwatch _clock;
        Stopwatch _fpsTimer;
        Stopwatch _frameTimer = new Stopwatch();
        Camera _camera;
        CursorInfo _cursor;
        Renderer _renderer;
        XtraForm1 _form;

        public RenderLoopControl()
        {   
            //Create the form
            _form = new XtraForm1();
            _form.loopControl = this;

            //Initialize command list
            _commandLists = new CommandList[Renderer.THREAD_COUNT];
            CommandList[] frozenCommandLists = null;

            _cursor = new CursorInfo(Cursor.Position);
            _camera = new Camera();

            InputHandler.Init(_form, _camera, _cursor);
            _renderer = new Renderer(_form, _camera);

            //Read grid data and initialize grid and its properties
            Grid = new Grid(_renderer.device, _form, "../../Resources/grid.bin");
            Grid.GenerateCells();

            _form.StartPosition = FormStartPosition.CenterScreen;

            //Setup timers
            _clock = new Stopwatch();
            _clock.Start();
            int fpsCounter = 0;
            _fpsTimer = new Stopwatch();
            _fpsTimer.Start();

            RenderLoop.Run(_form, () =>
            {
                DeltaTime = (float)_frameTimer.ElapsedMilliseconds/1000;
                _frameTimer.Restart();

                //Reset cursor position to the center of the form
                if (!_cursor.IsFree())
                {
                    _cursor.Update(Cursor.Position);
                    _camera.Rotate(-new Vector2(_cursor.GetDelta().Y, _cursor.GetDelta().X));
                    Cursor.Position = new System.Drawing.Point(_form.Left + _form.Size.Width / 2, _form.Top + _form.Size.Height / 2);
                    _cursor.Update(Cursor.Position);
                }

                _camera.Update();
                Grid.Update();

                //Display text
                fpsCounter++;
                if (_fpsTimer.ElapsedMilliseconds > 1000)
                {
                    _form.Text = string.Format("SharpDX - FPS: {0:F2} ({1:F2}ms)", 1000.0 * fpsCounter / _fpsTimer.ElapsedMilliseconds, (float)_fpsTimer.ElapsedMilliseconds / fpsCounter);
                    _fpsTimer.Reset();
                    _fpsTimer.Stop();
                    _fpsTimer.Start();
                    fpsCounter = 0;
                }

                _renderer.SetupPipeline();
                _renderer.RenderDeferred(Renderer.THREAD_COUNT, _commandLists, Grid);

                //Execute command list and dispose
                for (int i = 0; i < Renderer.THREAD_COUNT; i++)
                {
                    var commandList = _commandLists[i];
                    _renderer.imm.ExecuteCommandList(commandList, false);
                    commandList.Dispose();
                    _commandLists[i] = null;
                }
                _renderer.swapChain.Present(0, PresentFlags.None);
            });
        }
    }
}