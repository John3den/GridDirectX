using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace Engine
{
    public class Property
    {
        public float[,,] values;
        public Vector3i size;
        float scaling;
        float offset;
        public string label;
        public Property(Vector3i s, float[,,] data,float propertyOffset, float propertyScale, string propertyLabel)
        {
            scaling = propertyScale;
            offset = propertyOffset;
            values = data;
            size = s;
            label = propertyLabel; 
        }
        public float GetOffset()
        {
            return offset;
        }
        public float GetScaling()
        {   
            return scaling;
        }
    }
}