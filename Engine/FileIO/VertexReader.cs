using System.Globalization;
using System.IO;
using System;
using SharpDX;

namespace Engine
{
    public class VertexReader
    {
        public static void LoadVertices(Vector4[] array, string path)
        {
            string[] lines = File.ReadAllLines(path);
            int i = 0;
            foreach (string line in lines)
            {
                string currentFloat = "";
                for (int counter = 0; counter < line.Length; counter++)
                {

                    if (line[counter] == 'f')
                    {
                        continue;
                    }
                    else
                    if (line[counter] == ',')
                    {
                        float number = Convert.ToSingle(currentFloat, CultureInfo.InvariantCulture);
                        currentFloat = "";
                        array[i / 4][i % 4] = number;
                        i++;
                    }
                    else if (line[counter] != ' ')
                    {
                        currentFloat += line[counter];
                    }

                }
            }
        }
    }

}
