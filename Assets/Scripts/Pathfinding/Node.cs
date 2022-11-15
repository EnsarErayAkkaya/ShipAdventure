using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.Pathfinding
{
    public class Node : IHeapItem<Node>
    {
        public Vector3 worldPosition;
        public bool walkable;
        public int gridX, gridY;

        public int gCost;
        public int hCost;

        public Node parent;
        
        public int FCost => gCost + hCost;

        private int heapIndex;
        public int HeapIndex { get => heapIndex; set => heapIndex = value; }

        public Node(Vector3 worldPosition, bool walkable, int gridX, int gridY)
        {
            this.worldPosition = worldPosition;
            this.walkable = walkable;
            this.gridX = gridX;
            this.gridY = gridY;
        }

        public int CompareTo(Node other)
        {
            int compare = FCost.CompareTo(other.FCost);
            if(compare == 0)
            {
                compare = hCost.CompareTo(other.hCost);
            }
            return -compare;
        }
    }
}