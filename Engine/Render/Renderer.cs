using SharpDX;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;
using MapFlags = SharpDX.Direct3D11.MapFlags;

namespace Engine
{
    public class Renderer
    {
        public void DrawCell(DeviceContext renderingContext, Buffer dynamicConstantBuffer,Matrix worldViewProj, Buffer dataBuffer)
        {
            renderingContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(dataBuffer, Utilities.SizeOf<Vector4>() * 3, 0));
            var dataBox = renderingContext.MapSubresource(dynamicConstantBuffer, 0, MapMode.WriteDiscard, MapFlags.None);
            Utilities.Write(dataBox.DataPointer, ref worldViewProj);
            renderingContext.UnmapSubresource(dynamicConstantBuffer, 0);
            renderingContext.Draw(36, 0);
        }
    }
}