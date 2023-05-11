using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting.BarCode;
using GridRender;
using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.IO;
using System.Windows.Forms;

namespace Engine
{
    public class Grid
    {
        public readonly Slider Slider;
        public Property[] Properties;

        public int CurrentProperty
        {
            set
            {
                if (value < _numberOfProperties && value >= 0)
                {
                    _currentProperty = value;
                }
            }
            get { return _currentProperty; }
        }

        Device _device;
        Vector3i _size;
        Cell[,,] _cells;

        int _numberOfProperties; 
        int _currentProperty = 0;
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

            _size = reader.GetGridSize();
            _cells = new Cell[_size.x, _size.y, _size.z];

            for (int k = 0; k < _size.z; k++)
            {
                for (int i = 0; i < _size.x; i++)
                {
                    for (int j = 0; j < _size.y; j++)
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

                        // Calculate normalized( in the range [0,1] ) property value
                        float normalizedPropertyValue = NormalizePropertyValue(Properties[_currentProperty].Values[i, j, k]);

                        _cells[i, j, k] = new Cell(posData, act, _device, normalizedPropertyValue);
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

        public Grid(Device dev, XtraForm1 form, string path)
        {
            GridReader gridReader = new GridReader(path);
            _size = gridReader.GetGridSize();

            PropertyReader propReader = new PropertyReader("../../Resources/grid.binprops.txt", _size, ref _numberOfProperties);
            Properties = propReader.ReadProperties();

            _pathToCells = path;
            _device = dev;

            Slider = new Slider(form, this);

            gridReader.Close();
        }

        public float NormalizePropertyValue(float propertyValue)
        {
            float propertyOffset = Properties[_currentProperty].GetOffset();
            float propertyScaling = Properties[_currentProperty].GetScaling();

            propertyValue -= propertyOffset;
            propertyValue /= propertyScaling;

            return propertyValue;
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

        public void Update()
        {
            Slider.Update();
        }

        public void ChangeProperty(LabelControl labelControl, bool next)
        {
            if (next)
            {
                CurrentProperty++;
                GenerateCells();
            }
            else
            {
                CurrentProperty--;
                GenerateCells();
            }
            labelControl.Text = Properties[CurrentProperty].Label;
        }
    }
}