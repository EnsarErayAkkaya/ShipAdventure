using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.Pathfinding
{
    public class Node
    {
        public Vector3 worldPosition;
        public bool walkable;
        public int gridX, gridY;

        public int gCost;
        public int hCost;

        public Node parent;
        
        public int FCost => gCost + hCost;

        public Node(Vector3 worldPosition, bool walkable, int gridX, int gridY)
        {
            this.worldPosition = worldPosition;
            this.walkable = walkable;
            this.gridX = gridX;
            this.gridY = gridY;
        }
    }
}