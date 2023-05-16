using SharpDX;
using SharpDX.Direct3D11;
using System;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;


namespace Engine
{
    public class CellGeometry
    {
        Vector4[] _vertexPositions = new Vector4[Cell.N_OF_VERTICES];
        public static Vector4[] VertexLocalPositions;

        public CellGeometry(Vector3[] positions)
        {
            if (VertexLocalPositions == null)
            {
                VertexLocalPositions = new Vector4[Cell.N_OF_VERTICES];
                VertexReader.LoadVertices(VertexLocalPositions, "../../Resources/CellLocalCoordinates.txt");
            }

            for (int i=0; i < Cell.N_OF_VERTICES; i++)
            {
                _vertexPositions[i] = new Vector4(positions[i], 1.0f);
            }
        }

        public void OffsetAndScaleVertices(Vector3 offset, Vector3 scale)
        {
            for (int i = 0; i < Cell.N_OF_VERTICES; i++)
            {
                float x = (_vertexPositions[i].X + offset.X) / scale.X;
                float y = (_vertexPositions[i].Y + offset.Y) / scale.Y;
                float z = (_vertexPositions[i].Z + offset.Z) / scale.Z;

                _vertexPositions[i] = new Vector4(x, y, z, 1.0f);
            }
        }

        public Vector4[] GetVertexPositions()
        {
            return _vertexPositions;
        }
    }
}