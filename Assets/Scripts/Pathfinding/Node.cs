using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.Pathfinding
{
    public class Node
    {
        public Vector3 worldPosition;
        public bool walkable;

        public Node(Vector3 worldPosition, bool walkable)
        {
            this.worldPosition = worldPosition;
            this.walkable = walkable;
        }
    }
}