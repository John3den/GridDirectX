using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace Engine
{
    public class Property
    {
        public float[,,] _values;
        public Vector3i _size;
        float _scaling;
        float _offset;
        public string _label;
        public Property(Vector3i s, float[,,] data,float propertyOffset, float propertyScale, string propertyLabel)
        {
            _scaling = propertyScale;
            _offset = propertyOffset;
            _values = data;
            _size = s;
            _label = propertyLabel; 
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