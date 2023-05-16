using SharpDX;
using SharpDX.Direct3D11;
using System;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;

namespace Engine
{
    public class Cell
    {
        const int N_OF_ATTRIBS = 3;
        public const int N_OF_VERTICES = 36;

        Vector4[] _vertData = new Vector4[N_OF_VERTICES * N_OF_ATTRIBS];

        Buffer _buffer;

        public Cell(CellGeometry geometry, float property)
        {
            Vector4 color = PropertyArray.GetColor(property);

            for (int i = 0; i < N_OF_VERTICES; i++) 
            {
                _vertData[i * N_OF_ATTRIBS + 0] = geometry.GetVertexPositions()[i];
                _vertData[i * N_OF_ATTRIBS + 1] = color;
                _vertData[i * N_OF_ATTRIBS + 2] = CellGeometry.VertexLocalPositions[i];
            }

        }

        public void CreateBuffer(Device device)
        {
            _buffer = Buffer.Create(device, BindFlags.VertexBuffer, _vertData);
        }

        public Buffer getVert()
        {
            return _buffer;
        }
    }
}