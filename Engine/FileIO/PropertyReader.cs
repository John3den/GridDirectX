using DevExpress.Utils.Extensions;
using SharpDX;
using System;
using System.Globalization;
using System.IO;
namespace Engine
{
    public class PropertyReader
    {
        public PropertyReader(string path, Vector3i dimensions, ref Property[] props, ref int numberOfProperties)
        {
            
            string[] lines = File.ReadAllLines(path);
            int offset = 1;// first two lines
            int valuesRead = 0;
            numberOfProperties = Convert.ToInt32(lines[0]);
            props = new Property[numberOfProperties];
            for (int m = 0; m < numberOfProperties; m++)
            {
                string PropertyLabel = "property";
                float minProp = 0;
                float maxProp = 0;
                float[,,] data;
                data = new float[dimensions.x, dimensions.y, dimensions.z];
                for (int i = 0; i < dimensions.x; i++)
                {
                    for (int j = 0; j < dimensions.y; j++)
                    {
                        for (int k = 0; k < dimensions.z; k++)
                        {
                            if (Char.IsLetter(lines[valuesRead + offset][0]))
                            {
                                PropertyLabel = lines[valuesRead + offset];
                                offset++;
                            }
                            data[i, j, k] = Convert.ToSingle(lines[valuesRead + offset], CultureInfo.InvariantCulture);

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
                props[m] = new Property(dimensions, data, propoffset, propscale,PropertyLabel);
            }
            

        }
    }
        
}