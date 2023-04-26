using DevExpress.Utils.Extensions;
using System;
using System.Globalization;
using System.IO;
namespace Engine
{
    public class PropertyReader
    {
        public PropertyReader(string path, Vector3i dimensions, ref Property[] props)
        {
            int propsCount = 0;
            int size = dimensions.x * dimensions.y * dimensions.z;
            float[,,] data = new float[dimensions.x, dimensions.y, dimensions.z];
            string[] lines = File.ReadAllLines(path);
            int c = 0;
            int pc = 0;
            int offset = 2;// first two lines
            props = new Property[Convert.ToInt32(lines[0])];
            for (int i = 0; i < dimensions.x; i++)
            {
                for (int j = 0; j < dimensions.y; j++)
                {
                    for (int k = 0; k < dimensions.z; k++)
                    {
                        //Console.WriteLine(lines[c + offset]);
                        data[i, j, k] = Convert.ToSingle(lines[c + offset], CultureInfo.InvariantCulture);

                        c++;
                    }
                }
            }
            props[0] = new Property(dimensions,data);
        }
    }
        
}