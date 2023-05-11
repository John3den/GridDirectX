using DevExpress.Utils.Extensions;
using SharpDX;
using SharpDX.MediaFoundation;
using System;
using System.Globalization;
using System.IO;
namespace Engine
{
    public class PropertyReader
    {
        string[] _fileLines;
        int _numberOfProperties;
        Vector3i _gridDimensions;
        public PropertyReader(string path, Vector3i dimensions, ref int numberOfProperties)
        {
            _gridDimensions = dimensions; 
            _fileLines = File.ReadAllLines(path);
            numberOfProperties = Convert.ToInt32(_fileLines[0]);
            _numberOfProperties = numberOfProperties;
        }

        public Property[] ReadProperties()
        {
            Property[] props = new Property[_numberOfProperties];
            int offset = 1;// first two lines
            int valuesRead = 0;
            for (int m = 0; m < _numberOfProperties; m++)
            {
                string PropertyLabel = "property";
                float minProp = 0;
                float maxProp = 0;
                float[,,] data;
                data = new float[_gridDimensions.x, _gridDimensions.y, _gridDimensions.z];
                for (int i = 0; i < _gridDimensions.x; i++)
                {
                    for (int j = 0; j < _gridDimensions.y; j++)
                    {
                        for (int k = 0; k < _gridDimensions.z; k++)
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
                float propscale = maxProp - minProp;
                if (propscale == 0) propscale = 1;
                float propoffset = minProp;
                props[m] = new Property(_gridDimensions, data, propoffset, propscale, PropertyLabel);
            }
            return props;
        }
    }   
}