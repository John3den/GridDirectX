using DevExpress.Utils.Extensions;
using System;
using System.Globalization;
using System.IO;
namespace Engine
{
    public class PropertyReader
    {
        public PropertyReader(string path, Vector3i dimensions, ref Property[] props, ref float propscale, ref float propoffset)
        {
            int propsCount = 0;
            int size = dimensions.x * dimensions.y * dimensions.z;
            float[,,] data = new float[dimensions.x, dimensions.y, dimensions.z];
            string[] lines = File.ReadAllLines(path);
            int c = 0;
            int pc = 0;
            float minProp = 0;
            float maxProp = 0;
            int offset = 2;// first two lines
            props = new Property[Convert.ToInt32(lines[0])];
            for (int i = 0; i < dimensions.x; i++)
            {
                for (int j = 0; j < dimensions.y; j++)
                {
                    for (int k = 0; k < dimensions.z; k++)
                    { 
                        data[i, j, k] = Convert.ToSingle(lines[c + offset], CultureInfo.InvariantCulture);

                        if(c == 0)
                        {
                            minProp = data[i, j, k];
                            maxProp = data[i, j, k];
                        }
                        else
                        {
                            minProp = minProp < data[i, j, k] ? minProp : data[i, j, k];
                            maxProp = maxProp > data[i, j, k] ? maxProp : data[i, j, k];
                        }

                        c++;

                    }
                }
            }
            propscale = maxProp - minProp;
            if (propscale == 0) propscale = 1;
            propoffset = minProp;
            props[0] = new Property(dimensions,data);
        }
    }
        
}