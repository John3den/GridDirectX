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

namespace Engine
{
    public class Renderer
    {

        public const int THREAD_COUNT = 4;
        Camera _camera;
        XtraForm1 targetForm;
        DeviceContext[] contextPerThread;
        DeviceContext[] deferredContexts;
        Buffer dynamicConstantBuffer;
        Buffer staticContantBuffer;
        Matrix view;
        Matrix proj;
        VertexShader vertexShader;
        PixelShader pixelShader;
        RenderTargetView renderView;
        DepthStencilView depthView;
        InputLayout layout;
        public readonly Device device;
        public readonly SwapChain swapChain;
        public DeviceContext imm;
        public Renderer(XtraForm1 form, Camera cam)
        {
            _camera = cam;
            targetForm = form;

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
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, desc, out device, out swapChain);
            imm = device.ImmediateContext;

            //generate contexts
            GenerateContexts();

            //create backbubuffer and renderview
            var factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAll);

            var backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
            renderView = new RenderTargetView(device, backBuffer);

            LoadShaders();

            proj = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, form.ClientSize.Width / (float)form.ClientSize.Height, 0.1f, 100.0f);

            SetupBuffers(form);
        }

        public void GenerateContexts() 
        {
            deferredContexts = new DeviceContext[THREAD_COUNT];
            for (int i = 0; i < deferredContexts.Length; i++)
                deferredContexts[i] = new DeviceContext(device);

            contextPerThread = new DeviceContext[Renderer.THREAD_COUNT];
            contextPerThread[0] = imm;
        }

        public void SetupBuffers(XtraForm1 form)
        {
            staticContantBuffer = new Buffer(device, Utilities.SizeOf<Matrix>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            dynamicConstantBuffer = new Buffer(device, Utilities.SizeOf<Matrix>(), ResourceUsage.Dynamic, BindFlags.ConstantBuffer, CpuAccessFlags.Write, ResourceOptionFlags.None, 0);

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

            depthView = new DepthStencilView(device, depthBuffer);
        }

        public void LoadShaders()
        {
            //Create vertex shader
            var bytecode = ShaderBytecode.CompileFromFile("../../Resources/MultiCube.fx", "VS", "vs_4_0");
            vertexShader = new VertexShader(device, bytecode);

            layout = new InputLayout(device, ShaderSignature.GetInputSignature(bytecode), new[]
{
                        new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                        new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 16, 0),
                        new InputElement("LOCALPOS", 0, Format.R32G32B32A32_Float, 32, 0)
                    });

            //Create pixel shader
            bytecode.Dispose();
            bytecode = ShaderBytecode.CompileFromFile("../../Resources/MultiCube.fx", "PS", "ps_4_0");
            pixelShader = new PixelShader(device, bytecode);
            bytecode.Dispose();
        }

        public void SetupPipeline()
        {
            Array.Copy(deferredContexts, contextPerThread, contextPerThread.Length);
            for (int i = 0; i < THREAD_COUNT; i++)
            {
                var renderingContext = contextPerThread[i];
                renderingContext.InputAssembler.InputLayout = layout;
                renderingContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
                renderingContext.VertexShader.SetConstantBuffer(0, dynamicConstantBuffer);
                renderingContext.VertexShader.Set(vertexShader);
                renderingContext.Rasterizer.SetViewport(0, 0, targetForm.ClientSize.Width, targetForm.ClientSize.Height);
                renderingContext.PixelShader.Set(pixelShader);
                renderingContext.OutputMerger.SetTargets(depthView, renderView);
            }
        }

        public void RenderDeferred(int threadCount,CommandList[] commandLists, Grid grid)
        {
            int deltaCube = grid.GetSize().y / threadCount;
            if (deltaCube == 0) deltaCube = 1;
            int nextStartingRow = 0;
            var tasks = new Task[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                var threadIndex = i;
                int fromRow = nextStartingRow;
                int toRow = (i + 1) == threadCount ? grid.GetSize().y : fromRow + deltaCube;
                if (toRow > grid.GetSize().y)
                    toRow = grid.GetSize().y;
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
            var renderingContext = contextPerThread[contextIndex];
            if (contextIndex == 0)
            {
                contextPerThread[0].ClearDepthStencilView(depthView, DepthStencilClearFlags.Depth, 1.0f, 0);
                contextPerThread[0].ClearRenderTargetView(renderView, Color.White);
            }

            var rotateMatrix = Matrix.Scaling(1.0f / 3f);
            Matrix worldViewProj;
            view = _camera.GetView();

            var viewProj = Matrix.Multiply(view, proj);
            worldViewProj = rotateMatrix * viewProj;
            worldViewProj *= Matrix.Translation(-0.2f, 0, 0);
            worldViewProj *= Matrix.RotationZ((float)Math.PI);
            worldViewProj.Transpose();

            Vector3i gridSize = grid.GetSize();

            for (int y = fromY; y < toY; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    for (int z = 0; z < gridSize.z; z++)
                    {
                        if (grid.Slider.IncludesCell(x, y, z))
                        {
                            DrawCell(renderingContext, dynamicConstantBuffer, worldViewProj, grid.GetCell(x, y, z).getVert());
                        }
                    }
                }
            }
            commandLists[contextIndex] = renderingContext.FinishCommandList(false);
        }
    }

}