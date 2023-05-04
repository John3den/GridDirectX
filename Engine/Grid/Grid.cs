using DevExpress.XtraPrinting.BarCode;
using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.IO;

namespace Engine
{
    public class Grid
    {
        Vector3i size;
        Cell[,,] cells;
        int numberOfProperties; 
        int currentProperty = 0;
        public int CurrentProperty
        {
            set
            {
                if(value < numberOfProperties && value >=0)
                {
                    currentProperty = value;
                }
            }
            get { return currentProperty; }
        }
        public Property[] props;
        string pathToCells;
        Device device;
        public void GenerateCells()
        {
            GridReader reader = new GridReader(pathToCells);
            size = reader.GetGridSize();
            cells = new Cell[size.x, size.y, size.z];
            for (int k = 0; k < size.z; k++)
            {
                for (int i = 0; i < size.x; i++)
                {
                    for (int j = 0; j < size.y; j++)
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

                        Cell cell = new Cell(posData, act, device, (float)k / (float)size.z, props[currentProperty].values[i, j, k], props[currentProperty].GetOffset(), props[currentProperty].GetScaling());
                        cells[i, j, k] = cell;
                    }
                }
            }

            for (int k = 0; k < size.z; k++)
            {
                for (int i = 0; i < size.x; i++)
                {
                    for (int j = 0; j < size.y; j++)
                    {
                        cells[i, j, k].OffsetAndScaleVertices(-reader.GetGridPosition(), reader.GetGridScale());
                        cells[i, j, k].CreateBuffer(device);
                    }
                }
            }
            reader.Close();
        }
        public Grid(Device dev, string path)
        {
            pathToCells = path;
            GridReader reader = new GridReader(pathToCells);
            size = reader.GetGridSize();
            new PropertyReader("../../Resources/grid.binprops.txt", size, ref props, ref numberOfProperties);
            device = dev;

            reader.Close();
        }
        public Cell GetCell(int x, int y, int z)
        {
            return cells[x, y, z];
        }
        public Vector3i GetSize()
        {
            return size;
        }
        public int GetNumberOfProperties()
        {
            return numberOfProperties; 
        }
    }
}