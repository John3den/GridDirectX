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
        public Buffer buffer;
        public Vector4 vertCol = new Vector4(1,0,0,1);
        public Vector4[] vertData = new Vector4[72];
        public bool active;
        public Cell(Vector3[] pos, bool isActiveByDefault,Device device,float y,float property)
        {
            active = isActiveByDefault; 
            for(int i=0;i<36;i++)
            {
                vertCol = new Vector4(y/2 + 0.2f,property/500,(float)Math.Log((1-y)*Math.E)/2 + 0.2f,1.0f);
                vertData[i*2+1] = vertCol;

            }
            for (int i = 0; i < 36; i ++)
            {
                vertData[i*2] = new Vector4(pos[i],1.0f);

            }

            buffer = Buffer.Create(device, BindFlags.VertexBuffer, vertData);
        }
        public Buffer getVert()
        {
            return buffer;
        }
    }
}