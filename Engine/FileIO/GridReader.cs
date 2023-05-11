using DevExpress.XtraReports.Design;
using SharpDX;
using System;
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
                _mostCorner.X = Math.Max(_mostCorner.X, x);
                _leastCorner.X = Math.Min(_leastCorner.X, x);

                _mostCorner.Y = Math.Max(_mostCorner.Y, y);
                _leastCorner.Y = Math.Min(_leastCorner.Y, y);

                _mostCorner.Z = Math.Max(_mostCorner.Z, z);
                _leastCorner.Z = Math.Min(_leastCorner.Z, z);
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