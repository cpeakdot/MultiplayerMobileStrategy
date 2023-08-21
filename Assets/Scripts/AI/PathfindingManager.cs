using System.Collections.Generic;
using UnityEngine;

namespace MMS.AI
{
    public class PathfindingManager : MonoBehaviour
    {
        public static PathfindingManager Instance { get; private set; }
        
        private Pathfinding pathfinding;
        [SerializeField] private GameObject nodePrefab;
    
        [Header("Grid")] 
        [SerializeField] private int gridWidth;
        [SerializeField] private int gridHeight;
        [SerializeField] private float gridCellSize = 1f;
        [SerializeField] private Vector3 gridOrigin;
        [SerializeField] private LayerMask unWalkableAreaLayer;
        [SerializeField] private float maxHeightOfUnwalkableArea;

        private bool debugging = true;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            pathfinding = new Pathfinding(gridWidth, gridHeight, gridCellSize, gridOrigin);

#if UNITY_EDITOR
            if(debugging)
            {
                foreach (PathNode node in pathfinding.GetGrid.GetPathNodeList)
                {
                    Instantiate(nodePrefab, pathfinding.GetGrid.GetWorldPosition(node.GetX, node.GetZ),
                        Quaternion.identity);
                }
            }
#endif

            CheckWalkableGrids();
        }

        public List<PathNode> GetPathToPosition(Vector3 startPosition, Vector3 endposition)
        {
            
            pathfinding.GetGrid.GetXZ(startPosition, out int startX, out int startZ);
            pathfinding.GetGrid.GetXZ(endposition, out int endX, out int endZ);
            
            List<PathNode> path = pathfinding.FindPath(startX, startZ, endX, endZ);
            
            
            if (!debugging)
            {
                if (path != null)
                {
                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        Debug.DrawLine(new Vector3(path[i].GetX,0f, path[i].GetZ), new Vector3(path[i+1].GetX,0f, path[i+1].GetZ), Color.red, 5f);
                    }
                }
            }
            
            return path;
        }

        private void CheckWalkableGrids()
        {
            foreach (PathNode pathNode in pathfinding.GetGrid.GetPathNodeList)
            {
                Vector3 gridWorldPosition = pathfinding.GetGrid.GetWorldPosition(pathNode.GetX, pathNode.GetZ);
                Vector3 p1 = gridWorldPosition + (Vector3.up * maxHeightOfUnwalkableArea);
                
#if UNITY_EDITOR
                Vector3 p2 = gridWorldPosition + (Vector3.up * 3f);
                Debug.DrawLine(p1, p2, Color.green, 10f);
#endif
                
                if(!Physics.BoxCast(p1, gridCellSize * Vector3.one, Vector3.down, Quaternion.identity, 20f, unWalkableAreaLayer))
                {
                    continue;
                }
#if UNITY_EDITOR
                Debug.DrawLine(p1, p2, Color.red, 10f);
#endif
                pathNode.walkable = false;
            }
        }

        public Grid GetGrid => pathfinding.GetGrid;
    }
}

