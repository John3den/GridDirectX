using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace Engine
{
    public class Property
    {
        public float[,,] Values;
        public Vector3i Size;
        public string Label;

        float _scaling;
        float _offset;

        public Property(Vector3i s, float[,,] data, float propertyOffset, float propertyScale, string propertyLabel)
        {
            _scaling = propertyScale;
            _offset = propertyOffset;
            Values = data;
            Size = s;
            Label = propertyLabel; 
        }

        public float GetOffset()
        {
            return _offset;
        }

        public float GetScaling()
        {   
            return _scaling;
        }
    }
}