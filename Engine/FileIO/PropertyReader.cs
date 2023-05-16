using System;
using System.Globalization;
using System.IO;

namespace Engine
{
    public class PropertyReader
    {
        string[] _fileLines;
        int _numberOfProperties;

        public PropertyReader(string path, ref int numberOfProperties)
        {
            _fileLines = File.ReadAllLines(path);
            numberOfProperties = Convert.ToInt32(_fileLines[0]);
            _numberOfProperties = numberOfProperties;
        }

        public PropertyArray[] ReadProperties()
        {
            PropertyArray[] props = new PropertyArray[_numberOfProperties];

            // First line and names of properties
            int offset = 1; 

            int valuesRead = 0;

            for (int m = 0; m < _numberOfProperties; m++)
            {
                string PropertyLabel = "property";

                float minProp = 0;
                float maxProp = 0;

                float[,,] data = new float[Grid.Size.x, Grid.Size.y, Grid.Size.z];

                for (int i = 0; i < Grid.Size.x; i++)
                {
                    for (int j = 0; j < Grid.Size.y; j++)
                    {
                        for (int k = 0; k < Grid.Size.z; k++)
                        {
                            if (Char.IsLetter(_fileLines[valuesRead + offset][0]))
                            {
                                PropertyLabel = _fileLines[valuesRead + offset];
                                offset++;
                            }

                            data[i, j, k] = Convert.ToSingle(_fileLines[valuesRead + offset], CultureInfo.InvariantCulture);

                            if (valuesRead == 0)
                            {
                                minProp = data[i, j, k];
                                maxProp = data[i, j, k];
                            }
                            else
                            {
                                minProp = minProp < data[i, j, k] ? minProp : data[i, j, k];
                                maxProp = maxProp > data[i, j, k] ? maxProp : data[i, j, k];
                            }

                            valuesRead++;
                        }
                    }
                }

                float propScale = maxProp - minProp;
                if (propScale == 0) propScale = 1;

                float propOffset = minProp;

                props[m] = new PropertyArray(Grid.Size, data, propOffset, propScale, PropertyLabel);
            }
            return props;
        }
    }   
}