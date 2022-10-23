using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.Pathfinding
{
    public class Pathfinding : MonoBehaviour
    {
        public Transform seeker, target;

        [SerializeField] private Grid grid;

        private void Update()
        {
            FindPath(seeker.position, target.position);
        }

        void FindPath(Vector3 startPos, Vector3 targetPos)
        {
            Node startNode = grid.NodeFromWorldPoint(startPos);
            Node targetNode = grid.NodeFromWorldPoint(targetPos);

            List<Node> openSet = new List<Node>(); 
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                 
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if(currentNode == targetNode)
                {
                    RetracePath(startNode, targetNode);
                    return;
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if(!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + grid.GetDistance(currentNode, neighbour);

                    if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = grid.GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if(!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
        }

        void RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();

            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            path.Reverse();

            grid.path = path;
        }
    }
}