using DevExpress.XtraReports.Design;
using SharpDX;
using System.IO;
using System.Text;
namespace Engine
{
    public class GridReader
    {
        Stream stream;
        BinaryReader reader;
        public Vector3 mostCorner;
        public Vector3 leastCorner;
        bool readFirstVertex = false;
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
            Vector3 cellVertexCoordinates = new Vector3(x, y, z);
            if (!readFirstVertex)
            {
                mostCorner = cellVertexCoordinates;
                leastCorner = cellVertexCoordinates;
                readFirstVertex = true;
            }
            else
            {
                if(x > mostCorner.X)
                {
                    mostCorner.X = x;
                }
                if(x < leastCorner.X)
                {
                    leastCorner.X = x;
                }
                if(y > mostCorner.Y)
                {
                    mostCorner.Y = y;
                }
                if(y < leastCorner.Y)
                {
                    leastCorner.Y = y;
                }
                if(z > mostCorner.Z)
                {
                    mostCorner.Z = z;
                }
                if(z < leastCorner.Z)
                {
                    leastCorner.Z = z;
                }

            }
            return cellVertexCoordinates;
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