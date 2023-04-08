using System;
using OpenTK;
using OpenTK.Mathematics;

namespace OVFSliceViewerBusinessLayer.Classes
{
    public class Grid
    {
        Vertex[] _grid;
        public int Length { get; protected set; }
        //public IntPtr Size { get; protected set; }
        public Grid()
        {
            CreateGrid(100, 10);
            Length = _grid.Length;
        }

        private void CreateGrid(int gridSize, int gridDistance)
        {
            //rwth blue new Vector4(0f, 84f / 255f, 159f / 255f, 0f),
            var numberOfLines = gridSize / gridDistance;
            var grid = new Vertex[4 * numberOfLines + 4];

            for (int i = 0; i < 2 * numberOfLines + 2; i += 2)
            {
                grid[i] = new Vertex
                {
                    ColorIndex = 0,
                    Position = new Vector3((-gridSize / 2) + i / 2 * gridDistance, -gridSize / 2, 0)
                };
                grid[i + 1] = new Vertex
                {
                    ColorIndex = 0,
                    Position = new Vector3((-gridSize / 2) + i / 2 * gridDistance, gridSize / 2, 0)
                };

                grid[i + 2 * numberOfLines + 2] = new Vertex
                {
                    ColorIndex = 0,
                    Position = new Vector3((-gridSize / 2), (-gridSize / 2) + i / 2 * gridDistance, 0)
                };
                grid[i + 2 * numberOfLines + +2 + 1] = new Vertex
                {
                    ColorIndex = 0,
                    Position = new Vector3((gridSize / 2), (-gridSize / 2) + i / 2 * gridDistance, 0)
                };
            }

            _grid = grid;
        }

        public Vertex[] GetGrid()
        {
            return _grid;
        }
    }
}
