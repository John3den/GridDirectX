namespace Engine
{
    public class Property
    {
        public float[,,] values;
        public Vector3i size;
        public Property(Vector3i s, float[,,] data)
        {
            values = data;
            size = s;
        }
    }
}