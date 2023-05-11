using DevExpress.Utils.Html;
using DevExpress.XtraEditors;
using SharpDX;
using SharpDX.Direct3D11;
using System;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;
namespace Engine
{
    public class Cell
    {
        static Vector4[] _vertexLocalPositions;

        const int N_OF_ATTRIBS = 3;
        const int N_OF_VERTICES = 36;

        Vector4[] _vertData = new Vector4[N_OF_VERTICES * N_OF_ATTRIBS];

        Buffer _buffer;

        public Cell(Vector3[] pos, bool isActiveByDefault, Device device, float y, float property, float offset, float scaling)
        {
            if(_vertexLocalPositions == null)
            {
                _vertexLocalPositions = new Vector4[N_OF_VERTICES];
                VertexReader.LoadVertices(_vertexLocalPositions, "../../Resources/CellLocalCoordinates.txt");
            }
            property -= offset;
            property /= scaling;
            for(int i=0;i< N_OF_VERTICES; i++)
            {
                float blue = 0.8f -  property;
                float green = 0.8f - Math.Abs(property - 0.5f) * 2.0f;
                float red = 1 * property+0.1f;

                Vector4 vertCol = new Vector4(red, green, blue, 1.0f);
                _vertData[i * N_OF_ATTRIBS] = new Vector4(pos[i], 1.0f);
                _vertData[i * N_OF_ATTRIBS + 1] = vertCol;
                _vertData[i * N_OF_ATTRIBS + 2] = _vertexLocalPositions[i];
            }
        }

        public void OffsetAndScaleVertices(Vector3 offset, Vector3 scale)
        {
            for (int i = 0; i < N_OF_VERTICES; i++)
            {
                _vertData[i * N_OF_ATTRIBS] = new Vector4(_vertData[i * N_OF_ATTRIBS].X/scale.X, _vertData[i * N_OF_ATTRIBS].Y / scale.Y, _vertData[i * N_OF_ATTRIBS].Z / scale.Z,1.0f);
                _vertData[i * N_OF_ATTRIBS] += new Vector4(offset.X / scale.X, offset.Y / scale.Y, offset.Z / scale.Z, 0.0f); 
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