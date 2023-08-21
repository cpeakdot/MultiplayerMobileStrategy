using System.Collections.Generic;
using UnityEngine;

namespace MMS.AI
{
    public class Pathfinding
    {
        private Grid grid;
        private List<PathNode> openList;
        private List<PathNode> closedList;

        private const int STRAIGHT_COST = 10;
        private const int DIAGONAL_COST = 14;

        public Grid GetGrid => grid;
        
        public Pathfinding(int width, int height, float cellSize = 1f, Vector3 origin = default)
        {
            grid = new Grid(width, height, cellSize, origin);
        }

        public List<PathNode> FindPath(int startX, int startZ, int endX, int endZ)
        {
            PathNode startNode = grid.GetPathNode(startX, startZ);
            PathNode endNode = grid.GetPathNode(endX, endZ);

            if (startNode == null || endNode == null)
            {
                return null;
            }

            openList = new List<PathNode> { startNode };
            closedList = new List<PathNode>();

            for (int x = 0; x < grid.GetWidth; x++)
            {
                for (int z = 0; z < grid.GetHeight; z++)
                {
                    PathNode pathNode = grid.GetPathNode(x, z);
                    pathNode.gCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.cameFromNode = null;
                }
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistance(startNode, endNode);
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(openList);
                if (currentNode == endNode)
                {
                    // return the path.
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
                {
                    if (closedList.Contains(neighbourNode) || !neighbourNode.walkable) continue;

                    int tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, neighbourNode);
                    
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateDistance(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();

                        if (!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);
                        }
                    }
                    
                }
            }
            
            // No path found.
            return null;
        }

        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            List<PathNode> neighbourList = new();
            
            if (currentNode.GetX - 1 >= 0)
            {
                neighbourList.Add(GetNode(currentNode.GetX - 1, currentNode.GetZ));
                if (currentNode.GetZ > 0)
                {
                    neighbourList.Add(GetNode(currentNode.GetX - 1, currentNode.GetZ - 1));
                }

                if (currentNode.GetZ + 1 < grid.GetHeight)
                {
                    neighbourList.Add(GetNode(currentNode.GetX - 1, currentNode.GetZ + 1));
                }
            }

            if (currentNode.GetX + 1 < grid.GetWidth)
            {
                neighbourList.Add(GetNode(currentNode.GetX + 1, currentNode.GetZ));
                if (currentNode.GetZ - 1 >= 0)
                {
                    neighbourList.Add(GetNode(currentNode.GetX + 1, currentNode.GetZ - 1));
                }

                if (currentNode.GetZ + 1 < grid.GetHeight)
                {
                    neighbourList.Add(GetNode(currentNode.GetX + 1, currentNode.GetZ + 1));
                }
            }

            if (currentNode.GetZ - 1 >= 0)
            { 
                neighbourList.Add(GetNode(currentNode.GetX, currentNode.GetZ - 1));
            }

            if (currentNode.GetZ + 1 < grid.GetHeight)
            {
                neighbourList.Add(GetNode(currentNode.GetX, currentNode.GetZ + 1));
            }

            return neighbourList;
        }

        private PathNode GetNode(int x, int z)
        {
            return grid.GetPathNode(x, z);
        }

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new();
            path.Add(endNode);
            PathNode currentNode = endNode;
            while (currentNode.cameFromNode != null)
            {
                path.Add(currentNode.cameFromNode);
                currentNode = currentNode.cameFromNode;
            }
            path.Reverse();
            return path;
        }

        private int CalculateDistance(PathNode a, PathNode b)
        {
            int xDistance = Mathf.Abs(a.GetX - b.GetX);
            int zDistance = Mathf.Abs(a.GetZ - b.GetZ);

            int remaining = Mathf.Abs(xDistance - zDistance);

            return DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + STRAIGHT_COST * remaining;
        }

        private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
        {
            PathNode lowestFCostNode = pathNodeList[0];
            for (int i = 0; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].fCost < lowestFCostNode.fCost)
                {
                    lowestFCostNode = pathNodeList[i];
                }
            }

            return lowestFCostNode;
        }
    }
}

