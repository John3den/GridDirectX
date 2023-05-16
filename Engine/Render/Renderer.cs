using SharpDX;
using SharpDX.Direct3D11;
using System;
using Buffer = SharpDX.Direct3D11.Buffer;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using System.Threading.Tasks;
using GridRender;
using SharpDX.Direct3D;
using SharpDX.D3DCompiler;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;
using DevExpress.Utils.StructuredStorage.Internal.Writer;
using System.Windows.Forms;

namespace Engine
{
    public class Renderer
    {

        public const int THREAD_COUNT = 4;

        Camera _camera;
        XtraForm1 _targetForm;
        DeviceContext[] _contextPerThread;
        DeviceContext[] _deferredContexts;
        Buffer _dynamicConstantBuffer;

        Matrix _view;
        Matrix _proj;
        Matrix _world = Matrix.Scaling(1.0f / 3f);
        Matrix _worldViewProj;

        VertexShader _vertexShader;
        PixelShader _pixelShader;
        RenderTargetView _renderView;
        DepthStencilView _depthView;
        InputLayout _layout;

        public readonly Device Device;
        public readonly SwapChain SwapChain;

        public DeviceContext Immediate;

        public Renderer(XtraForm1 form, Camera cam)
        {
            _camera = cam;
            _targetForm = form;

            var desc = CreateSwapChainDescription(form);

            // Create Device and SwapChain 
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, desc, out Device, out SwapChain);
            Immediate = Device.ImmediateContext;

            GenerateContexts();

            //create backbubuffer and renderview
            var backBuffer = Texture2D.FromSwapChain<Texture2D>(SwapChain, 0);
            _renderView = new RenderTargetView(Device, backBuffer);

            LoadShaders();

            _proj = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, form.ClientSize.Width / (float)form.ClientSize.Height, 0.1f, 100.0f)
                    * Matrix.Translation(-0.2f, 0, 0)
                    * Matrix.RotationZ((float)Math.PI);

            SetupBuffers(form);
        }

        private SwapChainDescription CreateSwapChainDescription(XtraForm1 form)
        {
            return new SwapChainDescription()
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
    }

        private void GenerateContexts() 
        {
            _deferredContexts = new DeviceContext[THREAD_COUNT];
            for (int i = 0; i < _deferredContexts.Length; i++)
                _deferredContexts[i] = new DeviceContext(Device);

            _contextPerThread = new DeviceContext[Renderer.THREAD_COUNT];
            _contextPerThread[0] = Immediate;
        }

        private void CreateDevice()
        {

        }

        public void SetupBuffers(XtraForm1 form)
        {
            _dynamicConstantBuffer = new Buffer(Device, Utilities.SizeOf<Matrix>(), ResourceUsage.Dynamic, BindFlags.ConstantBuffer, CpuAccessFlags.Write, ResourceOptionFlags.None, 0);

            var depthBuffer = new Texture2D(Device, new Texture2DDescription()
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

            _depthView = new DepthStencilView(Device, depthBuffer);
        }

        public void LoadShaders()
        {
            //Create vertex shader
            var bytecode = ShaderBytecode.CompileFromFile("../../Resources/MultiCube.fx", "VS", "vs_4_0");
            _vertexShader = new VertexShader(Device, bytecode);

            _layout = new InputLayout(Device, ShaderSignature.GetInputSignature(bytecode), new[]
{
                        new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                        new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 16, 0),
                        new InputElement("LOCALPOS", 0, Format.R32G32B32A32_Float, 32, 0)
                    });

            //Create pixel shader
            bytecode.Dispose();
            bytecode = ShaderBytecode.CompileFromFile("../../Resources/MultiCube.fx", "PS", "ps_4_0");
            _pixelShader = new PixelShader(Device, bytecode);
            bytecode.Dispose();
        }

        public void SetupPipeline()
        {
            Array.Copy(_deferredContexts, _contextPerThread, _contextPerThread.Length);
            for (int i = 0; i < THREAD_COUNT; i++)
            {
                var renderingContext = _contextPerThread[i];
                renderingContext.InputAssembler.InputLayout = _layout;
                renderingContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
                renderingContext.VertexShader.SetConstantBuffer(0, _dynamicConstantBuffer);
                renderingContext.VertexShader.Set(_vertexShader);
                renderingContext.Rasterizer.SetViewport(0, 0, _targetForm.ClientSize.Width, _targetForm.ClientSize.Height);
                renderingContext.PixelShader.Set(_pixelShader);
                renderingContext.OutputMerger.SetTargets(_depthView, _renderView);
            }
        }

        private void UpdateWorldViewProj()
        {
            _view = _camera.GetView();
            _worldViewProj = _world * _view * _proj;
            _worldViewProj.Transpose();
        }

        public void RenderDeferred(int threadCount,CommandList[] commandLists, Grid grid)
        {
            UpdateWorldViewProj();

            int delta = Grid.Size.y / threadCount;
            if (delta == 0) delta = 1;

            int nextStartingRow = 0;
            var tasks = new Task[threadCount];

            // Assign threads to layers for rendering
            for (int i = 0; i < threadCount; i++)
            {
                var threadIndex = i;
                int fromRow = nextStartingRow;
                int toRow = (i + 1) == threadCount ? Grid.Size.y : fromRow + delta;
                if (toRow > Grid.Size.y)
                    toRow = Grid.Size.y;
                nextStartingRow = toRow;

                tasks[i] = new Task(() => RenderLayer(threadIndex, fromRow, toRow, commandLists, grid));
                tasks[i].Start();
            }

            Task.WaitAll(tasks);
        }

        void DrawCell(DeviceContext renderingContext, Buffer dynamicConstantBuffer,Matrix worldViewProj, Buffer dataBuffer)
        {
            renderingContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(dataBuffer, Utilities.SizeOf<Vector4>() * 3, 0));
            var dataBox = renderingContext.MapSubresource(dynamicConstantBuffer, 0, MapMode.WriteDiscard, MapFlags.None);
            Utilities.Write(dataBox.DataPointer, ref worldViewProj);
            renderingContext.UnmapSubresource(dynamicConstantBuffer, 0);
            renderingContext.Draw(36, 0);
        }

        void RenderLayer(int contextIndex, int fromY, int toY, CommandList[] commandLists, Grid grid)
        {
            var renderingContext = _contextPerThread[contextIndex];
            if (contextIndex == 0)
            {
                _contextPerThread[0].ClearDepthStencilView(_depthView, DepthStencilClearFlags.Depth, 1.0f, 0);
                _contextPerThread[0].ClearRenderTargetView(_renderView, Color.White);
            }

            for (int y = fromY; y < toY; y++)
            {
                for (int x = 0; x < Grid.Size.x; x++)
                {
                    for (int z = 0; z < Grid.Size.z; z++)
                    {
                        if (grid.Slider.IncludesCell(x, y, z))
                        {
                            DrawCell(renderingContext, _dynamicConstantBuffer, _worldViewProj, grid.GetCell(x, y, z).getVert());
                        }
                    }
                }
            }
            commandLists[contextIndex] = renderingContext.FinishCommandList(false);
        }
    }

}