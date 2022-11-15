using EEA.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Pathfinding
{
    [Serializable]
    public class Grid : MonoBehaviour
    {
        [SerializeField] private Sea sea;

        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private Vector2 gridWorldSize;
        [SerializeField] private float nodeRadius;
        [SerializeField] private bool displayGridGizmos;
        Node[,] grid;

        private float nodeDiameter;

        private int gridSizeX, gridSizeY;

        public int MaxSize => gridSizeX * gridSizeY;

        private void Awake()
        {
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            CreateGrid();
        }

        private void CreateGrid()
        {
            grid = new Node[gridSizeX, gridSizeY];
            Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, obstacleMask) /*|| sea.IsPositionInObstacle(worldPoint)*/);
                    grid[x, y] = new Node(worldPoint, walkable, x, y);
                }
            }
        }

        public Node NodeFromWorldPoint(Vector3 worldPosition)
        {
            int x = Mathf.RoundToInt((Mathf.Clamp(worldPosition.x, -gridWorldSize.x, gridWorldSize.x) + gridWorldSize.x / 2 - nodeRadius) / nodeDiameter);
            int y = Mathf.RoundToInt((Mathf.Clamp(worldPosition.z, -gridWorldSize.y, gridWorldSize.y) + gridWorldSize.y / 2 - nodeRadius) / nodeDiameter);

            return grid[x, y];
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if(x == 0 && y == 0)
                    {
                        continue;
                    }

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }

        public int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            if(dstX > dstY)
            {
                return 14 * dstY + (10 * (dstX - dstY));
            }
            return 14 * dstX+ (10 * (dstY- dstX));
        }

        public void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

            if(grid != null && displayGridGizmos)
            {
                foreach (Node n in grid)
                {
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;

                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                }
            }          
        }
    }
}