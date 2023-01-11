using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
namespace EEA.Pathfinding
{
    public class Pathfinding : MonoBehaviour
    {
        PathRequestManager pathRequestManager;
        [SerializeField] private Grid grid;

        private void Start()
        {
            pathRequestManager = PathRequestManager.instance;
        }

        internal void StartFindPath(Vector3 pathStart, Vector3 pathEnd)
        {
            StartCoroutine(FindPath(pathStart, pathEnd));
        }

        IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Vector3[] waypoints = new Vector3[0];
            bool pathSuccess = false;

            Node startNode = grid.NodeFromWorldPoint(startPos);
            Node targetNode = grid.NodeFromWorldPoint(targetPos);

            if (startNode.walkable && targetNode.walkable)
            {
                Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
                HashSet<Node> closedSet = new HashSet<Node>();
                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    Node currentNode = openSet.RemoveFirst();
                    closedSet.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        sw.Stop();
                        UnityEngine.Debug.Log("Path found " + sw.ElapsedMilliseconds + " ms");
                        pathSuccess = true;
                        break;
                    }

                    foreach (Node neighbour in grid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.walkable || closedSet.Contains(neighbour))
                        {
                            continue;
                        }

                        int newMovementCostToNeighbour = currentNode.gCost + grid.GetDistance(currentNode, neighbour);

                        if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newMovementCostToNeighbour;
                            neighbour.hCost = grid.GetDistance(neighbour, targetNode);
                            neighbour.parent = currentNode;

                            if (!openSet.Contains(neighbour))
                            {
                                openSet.Add(neighbour);
                            }
                        }
                    }
                }
            }
            yield return null;
            if(pathSuccess)
            {
                waypoints = RetracePath(startNode, targetNode, targetPos);
            }
            pathRequestManager.FinishedProcessingPath(waypoints, pathSuccess);
        }

        Vector3[] RetracePath(Node startNode, Node endNode, Vector3 targetPos)
        {
            List<Node> path = new List<Node>();

            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            Vector3[] waypoints = SimplifyPath(path, targetPos);
            Array.Reverse(waypoints);
            return waypoints;
        }

        Vector3[] SimplifyPath(List<Node> path, Vector3 targetPos)
        {
            List<Vector3> waypoints = new List<Vector3>();

            waypoints.Add(targetPos);

            Vector2 lastDir = Vector2.zero;

            for (int i = 1; i < path.Count; i++)
            {
                Vector2 newDir = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);

                if(lastDir != newDir)
                {
                    waypoints.Add(path[i].worldPosition);
                }
                lastDir = newDir;
            }
            return waypoints.ToArray();
        }

    }
}