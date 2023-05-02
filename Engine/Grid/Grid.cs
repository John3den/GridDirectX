using DevExpress.XtraPrinting.BarCode;
using SharpDX;
using SharpDX.Direct3D11;
using System;

namespace Engine
{
    public class Grid
    {
        public Vector3i size;
        public Vector3 position = new Vector3();
        int cellCount = 0;
        public Cell[,,] cells;
        public Property[] props;
        float propertyoffset = 0;
        float propertyscaling = 1;
        Vector3 leastCorner = new Vector3();
        Vector3 mostCorner = new Vector3();
        Vector3 scale = new Vector3();
        public Grid(string path, Device device)
        {
            GridReader reader = new GridReader(path);
            size = reader.GetGridSize();
            cellCount = size.x * size.y * size.z;
            cells = new Cell[size.x,size.y,size.z];
            new PropertyReader("../../Resources/grid.binprops.txt", size, ref props,ref propertyscaling, ref propertyoffset);
            int count = -1;
            for (int k = 0; k < size.z; k++) 
            {
                for (int i = 0; i < size.x; i++) 
                {
                    for (int j = 0; j < size.y; j++)
                    {
                        count++;

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

                        Cell cell = new Cell(posData,act, device, (float)k / (float)size.z, props[0].values[i,j,k],propertyoffset,propertyscaling);
                        cells[i, j,k] = cell;
                    }
                }
            }
            scale = (reader.mostCorner - reader.leastCorner);
            position = ((reader.mostCorner + reader.leastCorner) / 2);
            for (int k = 0; k < size.z; k++)
            {
                for (int i = 0; i < size.x; i++)
                {
                    for (int j = 0; j < size.y; j++)
                    {
                        cells[i, j, k].OffsetAndScaleVertices(-position,scale);
                        cells[i, j, k].CreateBuffer(device);
                    }
                }
            }
        }
    }
}