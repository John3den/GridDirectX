using DevExpress.Data;
using DevExpress.XtraEditors;
using GridRender;
using SharpDX;
using SharpDX.Direct3D11;
using System.Diagnostics;

namespace Engine
{
    public class Grid
    {
        public readonly PropertyArray[] Properties;
        public readonly Slider Slider;

        public static Vector3i Size;

        int _numberOfProperties;
        int _currentProperty = 0;

        Cell[,,] _cells;
        Device _device;
        GridGeometry _geometry;

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

        public Grid(Device dev, XtraForm1 form, string path)
        {
            _device = dev;
            _geometry = new GridGeometry(path);
            Slider = new Slider(form);

            PropertyReader propReader = new PropertyReader("../../Resources/grid.binprops.txt", ref _numberOfProperties);
            Properties = propReader.ReadProperties();

            _cells = new Cell[Size.x, Size.y, Size.z];

            GenerateCells();
        }

        public Cell GetCell(int x, int y, int z)
        {
            return _cells[x, y, z];
        }

        public void GenerateCells()
        {
            for (int k = 0; k < Size.z; k++)
            {
                for (int i = 0; i < Size.x; i++)
                {
                    for (int j = 0; j < Size.y; j++)
                    {
                        float normalizedPropertyValue = NormalizePropertyValue(Properties[_currentProperty].Values[i, j, k]);
                        _cells[i, j, k] = new Cell(_geometry.GetCellGeometry(i, j, k), normalizedPropertyValue);
                        _cells[i, j, k].CreateBuffer(_device);
                    }
                }
            }
        }

        public int GetNumberOfProperties()
        {
            return _numberOfProperties;
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

        public float NormalizePropertyValue(float propertyValue)
        {
            float propertyOffset = Properties[_currentProperty].GetOffset();
            float propertyScaling = Properties[_currentProperty].GetScaling();

            propertyValue -= propertyOffset;
            propertyValue /= propertyScaling;

            return propertyValue;
        }

        public void Update()
        {
            Slider.Update();
        }
    }
}