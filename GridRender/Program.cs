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
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using Engine;

namespace MultiCube
{
    internal class Program
    {
        const int THREAD_COUNT = 4;

        Camera camera;

        CursorInfo cursor;
        public void Run()
        {
            var form = new XtraForm1();

            var desc = new SwapChainDescription()
            {
                BufferCount = 2,
                ModeDescription =
                new ModeDescription(form.ClientSize.Width, form.ClientSize.Height,
                    new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = form.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };
            // Create Device and SwapChain 
            Device device;
            SwapChain swapChain;
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, desc, out device, out swapChain);
            var immediateContext = device.ImmediateContext;

            // PreCreate deferred contexts 
            var deferredContexts = new DeviceContext[THREAD_COUNT];
            for (int i = 0; i < deferredContexts.Length; i++)
                deferredContexts[i] = new DeviceContext(device);

            // Allocate rendering context array 
            var contextPerThread = new DeviceContext[THREAD_COUNT];
            contextPerThread[0] = immediateContext;
            var commandLists = new CommandList[THREAD_COUNT];
            CommandList[] frozenCommandLists = null;

            // Check if driver is supporting natively CommandList
            bool supportConcurentResources;
            bool supportCommandList;
            device.CheckThreadingSupport(out supportConcurentResources, out supportCommandList);

            // Ignore all windows events 
            var factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAll);

            // New RenderTargetView from the backbuffer 
            var backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
            var renderView = new RenderTargetView(device, backBuffer);

            // Compile Vertex and Pixel shaders 
            var bytecode = ShaderBytecode.CompileFromFile("../../Resources/MultiCube.fx", "VS", "vs_4_0");
            var vertexShader = new VertexShader(device, bytecode);

            // Layout from VertexShader input signature 
            var layout = new InputLayout(device, ShaderSignature.GetInputSignature(bytecode), new[]
                    {
                        new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                        new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 16, 0),
                        new InputElement("LOCALPOS", 0, Format.R32G32B32A32_Float, 32, 0)
                    });
            bytecode.Dispose();

            bytecode = ShaderBytecode.CompileFromFile("../../Resources/MultiCube.fx", "PS", "ps_4_0");
            var pixelShader = new PixelShader(device, bytecode);
            bytecode.Dispose();

            //read grid data here
            Grid grid = new Grid("../../Resources/grid.bin",device);
            Slider slider = new Slider(form, grid);
            camera = new Camera();

            form.StartPosition = FormStartPosition.CenterScreen;
            cursor = new CursorInfo(Cursor.Position);
            // Create Constant Buffer 
            var staticContantBuffer = new Buffer(device, Utilities.SizeOf<Matrix>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            var dynamicConstantBuffer = new Buffer(device, Utilities.SizeOf<Matrix>(), ResourceUsage.Dynamic, BindFlags.ConstantBuffer, CpuAccessFlags.Write, ResourceOptionFlags.None, 0);

            // Create Depth Buffer & View 
            var depthBuffer = new Texture2D(device, new Texture2DDescription()
            {
                Format = Format.D32_Float_S8X24_UInt,
                ArraySize = 1,
                MipLevels = 1,
                Width = form.ClientSize.Width,
                Height = form.ClientSize.Height,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            });

            var depthView = new DepthStencilView(device, depthBuffer);



            // Prepare matrices     
            const float viewZ = 5.0f;
            var view = Matrix.LookAtLH(new Vector3(0, 0, -viewZ), new Vector3(0, 0, 0), Vector3.UnitY);
            var proj = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, form.ClientSize.Width / (float)form.ClientSize.Height, 0.1f, 100.0f);

            // Use clock 
            var clock = new Stopwatch();
            clock.Start();

            var fpsTimer = new Stopwatch();
            fpsTimer.Start();
            int fpsCounter = 0;

            // Register KeyDown event handler on the form
            bool switchToNextState = false;
            // Install keys handlers 
            form.MouseDown += (target, arg) =>
            {
                if (arg.Button == MouseButtons.Right)
                {
                    if(cursor.IsFree())
                    {
                        cursor.Lock();
                        Cursor.Hide();
                    }
                    else
                    {
                        cursor.Unlock();
                        Cursor.Show();
                        Cursor.Position = new System.Drawing.Point(form.Size.Width / 2, form.Size.Height / 2);
                        cursor.Update(Cursor.Position);
                    }
                }
            };
            form.KeyUp += (target, arg) =>
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
            };
            form.KeyDown += (target, arg) =>
            {
                if (arg.KeyCode == Keys.F)
                {
                    camera.focused = !camera.focused;
                    if (!camera.focused) camera.Unfocus();
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
                switchToNextState = true;
            };
            Action SetupPipeline = () =>
            {
                Array.Copy(deferredContexts, contextPerThread, contextPerThread.Length);
                for (int i = 0; i < THREAD_COUNT; i++)
                {
                    var renderingContext = contextPerThread[i];
                    renderingContext.InputAssembler.InputLayout = layout;
                    renderingContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
                    renderingContext.VertexShader.SetConstantBuffer(0, dynamicConstantBuffer);
                    renderingContext.VertexShader.Set(vertexShader);
                    renderingContext.Rasterizer.SetViewport(0, 0, form.ClientSize.Width, form.ClientSize.Height);
                    renderingContext.PixelShader.Set(pixelShader);
                    renderingContext.OutputMerger.SetTargets(depthView, renderView);
                }
            };
            Action<int, int, int> RenderLayer = (int contextIndex, int fromY, int toY) =>
            {
                var renderingContext = contextPerThread[contextIndex];
                var time = clock.ElapsedMilliseconds / 1000.0f;
                if (contextIndex == 0)
                {
                    contextPerThread[0].ClearDepthStencilView(depthView, DepthStencilClearFlags.Depth, 1.0f, 0);
                    contextPerThread[0].ClearRenderTargetView(renderView, Color.Black);
                }
                
                var rotateMatrix = Matrix.Scaling(1.0f/3f);
                for (int y = fromY; y < toY; y++)
                {
                    for (int x = 0; x < grid.size.x; x++)
                    {
                        for(int z = 0; z < grid.size.z; z++)    
                        {
                            if (slider.Includes(x,y,z))
                            {
                                renderingContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(grid.cells[x,y,z].buffer, Utilities.SizeOf<Vector4>() * 3, 0));
                                Matrix worldViewProj;
                                view = camera.GetView();
                                //view *=Matrix.RotationY(time);
                                var viewProj = Matrix.Multiply(view, proj);
                                worldViewProj = rotateMatrix * viewProj;
                                worldViewProj *= Matrix.Translation(0.2f,0,0);
                                worldViewProj.Transpose();
                                var dataBox = renderingContext.MapSubresource(dynamicConstantBuffer, 0, MapMode.WriteDiscard, MapFlags.None);
                                Utilities.Write(dataBox.DataPointer, ref worldViewProj);
                                renderingContext.UnmapSubresource(dynamicConstantBuffer, 0);
                                renderingContext.Draw(36, 0);
                            }
                        }
                    }
                }
                commandLists[contextIndex] = renderingContext.FinishCommandList(false);
            };

            Action<int> RenderDeferred = (int threadCount) =>
            {
                int deltaCube = grid.size.y/threadCount;
                if (deltaCube == 0) deltaCube = 1;
                int nextStartingRow = 0;
                var tasks = new Task[threadCount];
                for (int i = 0; i < threadCount; i++)
                {
                    var threadIndex = i;
                    int fromRow = nextStartingRow;
                    int toRow = (i + 1) == threadCount ? grid.size.y : fromRow + deltaCube;
                    if (toRow > grid.size.y)
                        toRow = grid.size.y;
                    nextStartingRow = toRow;

                    tasks[i] = new Task(() => RenderLayer(threadIndex, fromRow, toRow));
                    tasks[i].Start();
                }
                Task.WaitAll(tasks);
            };

            RenderLoop.Run(form, () =>
            {

                if(!cursor.IsFree())
                {
                    cursor.Update(Cursor.Position);
                    camera.Rotate(new Vector2(cursor.GetDelta().Y, cursor.GetDelta().X));
                    Cursor.Position = new System.Drawing.Point(form.Size.Width / 2, form.Size.Height / 2);
                    cursor.Update(Cursor.Position);
                }
                camera.Update();

                slider.Update();
                fpsCounter++;
                if (fpsTimer.ElapsedMilliseconds > 1000)
                {
                    form.Text = string.Format("SharpDX - FPS: {0:F2} ({1:F2}ms)", 1000.0 * fpsCounter / fpsTimer.ElapsedMilliseconds, (float)fpsTimer.ElapsedMilliseconds / fpsCounter);
                    fpsTimer.Reset();
                    fpsTimer.Stop();
                    fpsTimer.Start();
                    fpsCounter = 0;
                }

                SetupPipeline();

                RenderDeferred(THREAD_COUNT);

                for (int i = 0; i < THREAD_COUNT; i++)
                {
                    var commandList = commandLists[i];
                    immediateContext.ExecuteCommandList(commandList, false);
                    commandList.Dispose();
                    commandLists[i] = null; 
                }
                swapChain.Present(0, PresentFlags.None);
            });
        }
        
        [STAThread]
        private static void Main()
        {
            var program = new Program();
            program.Run();
        }
    }
}