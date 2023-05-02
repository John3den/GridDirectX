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
namespace Engine
{
    public class Cell
    {
        private const int N_OF_ATTRIBS = 3;
        private const int N_OF_VERTICES = 36;

        public Buffer buffer;
        public Vector4[] vertData = new Vector4[N_OF_VERTICES * N_OF_ATTRIBS];
        public bool active;
        private Vector4[] localPositions = {
        new Vector4(-1,-1,1,1.0f),
        new Vector4(-1,1,1,1.0f),
        new Vector4(1,-1,1,1.0f),
        new Vector4(1,1,1,1.0f),
        new Vector4(1,-1,1,1.0f),
        new Vector4(-1,1,1,1.0f),

        new Vector4(-1,-1,-1,1.0f),
        new Vector4(1,-1,-1,1.0f),
        new Vector4(-1,1,-1,1.0f),
        new Vector4(1,1,-1,1.0f),
        new Vector4(-1,1,-1,1.0f),
        new Vector4(1,-1,-1,1.0f),

        new Vector4(-1,1,1,1.0f),
        new Vector4(-1,-1,1,1.0f),
        new Vector4(-1,1,-1,1.0f),
        new Vector4(-1,-1,-1,1.0f),
        new Vector4(-1,1,-1,1.0f),
        new Vector4(-1,-1,1,1.0f),

        new Vector4(1,-1,1,1.0f),
        new Vector4(1,1,1,1.0f),
        new Vector4(1,1,-1,1.0f),
        new Vector4(1,-1,-1,1.0f),
        new Vector4(1,-1,1,1.0f),
        new Vector4(1,1,-1,1.0f),

        new Vector4(-1,-1,1,1.0f),
        new Vector4(1,-1,1,1.0f),
        new Vector4(1,-1,-1,1.0f),
        new Vector4(1,-1,-1,1.0f),
        new Vector4(-1,-1,-1,1.0f),
        new Vector4(-1,-1,1,1.0f),

        new Vector4(1,1,1,1.0f),
        new Vector4(-1,1,1,1.0f),
        new Vector4(1,1,-1,1.0f),
        new Vector4(-1,1,-1,1.0f),
        new Vector4(1,1,-1,1.0f),
        new Vector4(-1,1,1,1.0f),
    };
        public Cell(Vector3[] pos, bool isActiveByDefault,Device device,float y,float property,float offset, float scaling)
        {
            active = isActiveByDefault;
            property -= offset;
            property /= scaling;
            for(int i=0;i< N_OF_VERTICES; i++)
            {

                Vector4 vertCol = new Vector4((y + (float)Math.Log(property*(Math.E-1) + 1)) /2, (float)Math.Log(property * (Math.E - 1) + 1) / 2, ((1-y) + (float)Math.Log(property * (Math.E - 1) + 1)) / 2, 1.0f);
                vertData[i * N_OF_ATTRIBS] = new Vector4(pos[i], 1.0f);
                vertData[i * N_OF_ATTRIBS + 1] = vertCol;
                vertData[i * N_OF_ATTRIBS + 2] = localPositions[i];
                //Console.WriteLine(vertData[i * N_OF_ATTRIBS]);
            }
        }
        public void OffsetAndScaleVertices(Vector3 offset, Vector3 scale)
        {
            for (int i = 0; i < N_OF_VERTICES; i++)
            {
                vertData[i * N_OF_ATTRIBS] = new Vector4(vertData[i * N_OF_ATTRIBS].X/scale.X, vertData[i * N_OF_ATTRIBS].Y / scale.Y, vertData[i * N_OF_ATTRIBS].Z / scale.Z,1.0f);
                vertData[i * N_OF_ATTRIBS] += new Vector4(offset.X / scale.X, offset.Y / scale.Y, offset.Z / scale.Z, 0.0f); 
            }
        }
        public void CreateBuffer(Device device)
        {
            buffer = Buffer.Create(device, BindFlags.VertexBuffer, vertData);
        }
        public Buffer getVert()
        {
            return buffer;
        }
    }
}