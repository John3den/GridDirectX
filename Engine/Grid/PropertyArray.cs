using SharpDX;
using System;

namespace Engine
{
    public class PropertyArray
    {
        public float[,,] Values;
        public Vector3i Size;
        public string Label;

        float _scaling;
        float _offset;

        public PropertyArray(Vector3i size, float[,,] data, float propertyOffset, float propertyScale, string propertyLabel)
        {
            _scaling = propertyScale;
            _offset = propertyOffset;

            Values = data;
            Size = size;
            Label = propertyLabel; 
        }

        public static Vector4 GetColor(float value)
        {
            float blue = 0.8f - value;
            float green = 0.8f - Math.Abs(value - 0.5f) * 2.0f;
            float red = 1 * value + 0.1f;

            return new Vector4(red, green, blue, 1.0f);
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