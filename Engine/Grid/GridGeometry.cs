using DevExpress.Data;
using DevExpress.XtraEditors;
using GridRender;
using SharpDX;
using SharpDX.Direct3D11;

namespace Engine
{
    public class GridGeometry
    {
        CellGeometry[,,] _cellGeometries;

        string _pathToCells;

        // top -> bottom -> front -> back -> left -> right
        int[] vertexOrder = { 0, 1, 3, 2, 3, 1,  4, 7, 5, 6, 5, 7,
                              1, 0, 5, 4, 5, 0,  3, 2, 6, 7, 3, 6,
                              0, 3, 7, 7, 4, 0,  2, 1, 6, 5, 6, 1 };

        // corner 0 -> corner 1 -> corner 2 -> corner 3
        int[] vertexReadOrder = { 0, 4, 1, 5, 2, 6, 3, 7 };

        public void GenerateCells()
        {
            GridReader reader = new GridReader(_pathToCells);

            Grid.Size = reader.GetGridSize();
            _cellGeometries = new CellGeometry[Grid.Size.x, Grid.Size.y, Grid.Size.z];

            for (int k = 0; k < Grid.Size.z; k++)
            {
                for (int i = 0; i < Grid.Size.x; i++)
                {
                    for (int j = 0; j < Grid.Size.y; j++)
                    {
                        bool act = reader.GetCellStatus();
                        Vector3[] currentCellVertices = new Vector3[8];

                        // Read vertices of current cell
                        for (int h = 0; h < 8; h++) 
                        {
                            currentCellVertices[vertexReadOrder[h]] = reader.GetCellVertex();
                        }

                        Vector3[] posData = new Vector3[36];

                        // Assign vertex position data to create the cell
                        for (int h = 0; h < 36; h++) 
                        {
                            posData[h] = currentCellVertices[vertexOrder[h]];
                        }

                        _cellGeometries[i, j, k] = new CellGeometry(posData);
                    }
                }
            }

            for (int k = 0; k < Grid.Size.z; k++)
            {
                for (int i = 0; i < Grid.Size.x; i++)
                {
                    for (int j = 0; j < Grid.Size.y; j++)
                    {   
                        _cellGeometries[i, j, k].OffsetAndScaleVertices(-reader.GetGridPosition(), reader.GetGridScale());
                    }
                }
            }
            reader.Close();
        }

        public CellGeometry GetCellGeometry(int i, int j, int k)
        {
            return _cellGeometries[i, j, k];
        }

        public GridGeometry(string path)
        {
            _pathToCells = path;
            GenerateCells();
        }


    }
}