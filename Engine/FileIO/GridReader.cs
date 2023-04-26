using SharpDX;
using System.IO;
using System.Text;
namespace Engine
{
    public class GridReader
    {
        Stream stream;
        BinaryReader reader;
        public Vector3i GetGridSize()
        {
            int x = reader.ReadInt32();
            int y = reader.ReadInt32();
            int z = reader.ReadInt32();
            return new Vector3i(x, y, z);
        }
        public bool GetCellStatus()
        {
            return reader.ReadBoolean();
        }
        public Vector3 GetCellVertex()
        {
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();
            float z = reader.ReadSingle();
            return new Vector3(x, y, z);
        }
        public GridReader(string path)
        {
            stream = File.Open(path, FileMode.Open);
            reader = new BinaryReader(stream, Encoding.UTF8, false);
        }
        ~GridReader()
        {
            stream.Close();
            reader.Close();
        }
    }
}