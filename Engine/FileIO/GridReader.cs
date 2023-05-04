using DevExpress.XtraReports.Design;
using SharpDX;
using System.IO;
using System.Text;
namespace Engine
{
    public class GridReader
    {
        Stream _stream;
        BinaryReader _reader;
        Vector3 _mostCorner;
        Vector3 _leastCorner;
        bool _readFirstVertex = false;
        public Vector3i GetGridSize()
        {
            int x = _reader.ReadInt32();
            int y = _reader.ReadInt32();
            int z = _reader.ReadInt32();
            return new Vector3i(x, y, z);
        }
        public void Close()
        {
            _reader.Close();
        }
        public bool GetCellStatus()
        {
            return _reader.ReadBoolean();
        }
        public Vector3 GetCellVertex()
        {
            float x = _reader.ReadSingle();
            float y = _reader.ReadSingle();
            float z = _reader.ReadSingle();
            Vector3 cellVertexCoordinates = new Vector3(x, y, z);
            if (!_readFirstVertex)
            {
                _mostCorner = cellVertexCoordinates;
                _leastCorner = cellVertexCoordinates;
                _readFirstVertex = true;
            }
            else
            {
                if(x > _mostCorner.X)
                {
                    _mostCorner.X = x;
                }
                if(x < _leastCorner.X)
                {
                    _leastCorner.X = x;
                }
                if(y > _mostCorner.Y)
                {
                    _mostCorner.Y = y;
                }
                if(y < _leastCorner.Y)
                {
                    _leastCorner.Y = y;
                }
                if(z > _mostCorner.Z)
                {
                    _mostCorner.Z = z;
                }
                if(z < _leastCorner.Z)
                {
                    _leastCorner.Z = z;
                }

            }
            return cellVertexCoordinates;
        }
        public GridReader(string path)
        {
            _stream = File.Open(path, FileMode.Open);
            _reader = new BinaryReader(_stream, Encoding.UTF8, false);
        }
        ~GridReader()
        {
            _stream.Close();
            _reader.Close();
        }
        public Vector3 GetGridPosition()
        {
            return ((_mostCorner + _leastCorner) / 2);
        }
        public Vector3 GetGridScale()
        {
            return (_mostCorner - _leastCorner);
        }
    }
}