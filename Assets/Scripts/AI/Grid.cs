using System.Collections;
using UnityEngine;

namespace MMS.AI
{
    public class Grid
    {
        private int width;
        private int height;
        private float cellSize;
        private Vector3 originPosition;
        private PathNode[,] gridArray;

        public Grid(int width, int height, float cellSize, Vector3 origin)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            originPosition = origin;

            gridArray = new PathNode[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int z = 0; z < gridArray.GetLength(1); z++)
                {
                    gridArray[x, z] = new PathNode(this, x, z);
                }
            }
        }

        public Vector3 GetWorldPosition(int x, int z)
        {
            return new Vector3(x, 0f, z) * cellSize + originPosition;
        }

        public void GetXZ(Vector3 worldPosition, out int x, out int z)
        {
            x = Mathf.RoundToInt((worldPosition - originPosition).x / cellSize);
            z = Mathf.RoundToInt((worldPosition - originPosition).z / cellSize);
        }

        public void SetPathNode(int x, int z, PathNode value)
        {
            if (x >= 0 && z >= 0 && x < width && z < height)
            {
                gridArray[x, z] = value;
            }
        }

        public void SetPathNode(Vector3 worldPosition, PathNode value)
        {
            int x, z;
            GetXZ(worldPosition, out x, out z);
            SetPathNode(x, z, value);
        }

        public PathNode GetPathNode(int x, int z)
        {
            if (x >= 0 && z >= 0 && x < width && z < height)
            {
                return gridArray[x, z];
            }
            else
            {
                return null;
            }
        }

        public PathNode GetPathNode(Vector3 worldPosition)
        {
            int x, z;
            GetXZ(worldPosition, out x, out z);
            return GetPathNode(x, z);
        }

        public int GetWidth => width;
        public int GetHeight => height;

        public IEnumerable GetPathNodeList => gridArray;
    }
}

