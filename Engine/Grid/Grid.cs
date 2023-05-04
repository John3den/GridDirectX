using DevExpress.XtraPrinting.BarCode;
using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.IO;

namespace Engine
{
    public class Grid
    {
        Vector3i _size;
        Cell[,,] _cells;
        int _numberOfProperties; 
        int _currentProperty = 0;
        public int CurrentProperty
        {
            set
            {
                if(value < _numberOfProperties && value >=0)
                {
                    _currentProperty = value;
                }
            }
            get { return _currentProperty; }
        }
        public Property[] _props;
        string _pathToCells;
        Device _device;
        public void GenerateCells()
        {
            GridReader reader = new GridReader(_pathToCells);
            _size = reader.GetGridSize();
            _cells = new Cell[_size.x, _size.y, _size.z];
            for (int k = 0; k < _size.z; k++)
            {
                for (int i = 0; i < _size.x; i++)
                {
                    for (int j = 0; j < _size.y; j++)
                    {
                        bool act = reader.GetCellStatus();
                        Vector3 v0 = reader.GetCellVertex();
                        Vector3 v4 = reader.GetCellVertex();
                        Vector3 v1 = reader.GetCellVertex();
                        Vector3 v5 = reader.GetCellVertex();
                        Vector3 v2 = reader.GetCellVertex();
                        Vector3 v6 = reader.GetCellVertex();
                        Vector3 v3 = reader.GetCellVertex();
                        Vector3 v7 = reader.GetCellVertex();
                        Vector3[] posData = new Vector3[36];

                        //TOP
                        posData[0] = v0;
                        posData[1] = v1;
                        posData[2] = v3;
                        posData[3] = v2;
                        posData[4] = v3;
                        posData[5] = v1;
                        //BOTTOM
                        posData[6] = v4;
                        posData[7] = v7;
                        posData[8] = v5;
                        posData[9] = v6;
                        posData[10] = v5;
                        posData[11] = v7;
                        //FRONT
                        posData[12] = v1;
                        posData[13] = v0;
                        posData[14] = v5;
                        posData[15] = v4;
                        posData[16] = v5;
                        posData[17] = v0;
                        //BACK
                        posData[18] = v3;
                        posData[19] = v2;
                        posData[20] = v6;
                        posData[21] = v7;
                        posData[22] = v3;
                        posData[23] = v6;
                        //LEFT
                        posData[24] = v0;
                        posData[25] = v3;
                        posData[26] = v7;
                        posData[27] = v7;
                        posData[28] = v4;
                        posData[29] = v0;
                        //RIGHT
                        posData[30] = v2;
                        posData[31] = v1;
                        posData[32] = v6;
                        posData[33] = v5;
                        posData[34] = v6;
                        posData[35] = v1;

                        Cell cell = new Cell(posData, act, _device, (float)k / (float)_size.z, _props[_currentProperty]._values[i, j, k], _props[_currentProperty].GetOffset(), _props[_currentProperty].GetScaling());
                        _cells[i, j, k] = cell;
                    }
                }
            }

            for (int k = 0; k < _size.z; k++)
            {
                for (int i = 0; i < _size.x; i++)
                {
                    for (int j = 0; j < _size.y; j++)
                    {
                        _cells[i, j, k].OffsetAndScaleVertices(-reader.GetGridPosition(), reader.GetGridScale());
                        _cells[i, j, k].CreateBuffer(_device);
                    }
                }
            }
            reader.Close();
        }
        public Grid(Device dev, string path)
        {
            _pathToCells = path;
            GridReader reader = new GridReader(_pathToCells);
            _size = reader.GetGridSize();
            new PropertyReader("../../Resources/grid.binprops.txt", _size, ref _props, ref _numberOfProperties);
            _device = dev;

            reader.Close();
        }
        public Cell GetCell(int x, int y, int z)
        {
            return _cells[x, y, z];
        }
        public Vector3i GetSize()
        {
            return _size;
        }
        public int GetNumberOfProperties()
        {
            return _numberOfProperties; 
        }
    }
}