using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EEA.Player;
using System.Linq;

namespace EEA.Managers
{
    public class IslandManager : MonoBehaviour
    {
       /* [SerializeField] private Vector2Int noiseSize;
        [SerializeField] private int interval = 1;
        [SerializeField] private Island.Island island;

        [SerializeField] private Player.Player player;

        private Vector2Int[] coordinates = { new Vector2Int(11,7) };

        private List<Island.Island> islands = new List<Island.Island>();

        private Vector2Int lastControlledIndex = new Vector2Int(-1000,-1000);

        private void Start()
        {
            player = FindObjectOfType<Player.Player>();
        }

        private void Update()
        {
            List<Vector2Int> calculatedIslands = CalculateCurrentIslandCoordinates();
            
            if(calculatedIslands != null)
            {
                CreateIslands(calculatedIslands);
            }

        }

        private List<Vector2Int> CalculateCurrentIslandCoordinates()
        {
            Vector2Int playerPos = new Vector2Int((int)player.ShipMovement.transform.position.x, (int)player.ShipMovement.transform.position.z);
            Vector2Int origin = playerPos - (noiseSize / 2) + Vector2Int.one;

            if(origin.x % interval != 0)
            {
                int diff = origin.x % interval;
                if(interval - diff > diff) // round to top
                {
                    origin.x += interval - diff;

                }
                else
                {
                    origin.x += diff;
                }
            }

            if (origin.y % interval != 0)
            {
                int diff = origin.y % interval;
                if (interval - diff > diff) // round to top
                {
                    origin.y += interval - diff;

                }
                else
                {
                    origin.y += diff;
                }
            }

            if (lastControlledIndex == origin)
            {
                return null;
            }

            lastControlledIndex = origin;

            List<Vector2Int> calculatedIslands = new List<Vector2Int>();

            for (int i = 0; i < noiseSize.x; i+= interval)
            {
                for (int j = 0; j < noiseSize.y; j+= interval)
                {
                    float xCoord = origin.x + ((float)i * 1.01f) + 0.01f;
                    float yCoord = origin.y + ((float)j * 1.01f) + 0.01f;

                    float value = Mathf.PerlinNoise(xCoord, yCoord);

                    //Debug.Log("(" + (origin.x + i) + ", " + (origin.y + j) + ") -" + "("+ xCoord +", "+ yCoord+") = " + value);

                    if (value > .6f)
                    {
                        //Debug.Log("coords: (" + origin.x + i + ","+ origin.y + j + ")");

                        calculatedIslands.Add(origin + new Vector2Int(i, j));
                    }   
                }
            }
            return calculatedIslands;
        }

        private bool IsIslandExists(Vector2Int _coordinate)
        {
            return islands.Any(s => s.Coordinate == _coordinate);
        }

        private void CreateIsland(Vector2Int _coordinate)
        {
            Island.Island _island = Instantiate(island, new Vector3(_coordinate.x, 0, _coordinate.y), Quaternion.identity);
            _island.transform.SetParent(transform);
            _island.Init(_coordinate);
            islands.Add(_island);
        }

        private void CreateIslands(List<Vector2Int> islandCoordinates)
        {
            List<int> deletingIslands = new List<int>();
            for(int i = islands.Count - 1; i > 0 ; i--)
            {
                if(!islandCoordinates.Any(s => s == islands[i].Coordinate))
                {
                    deletingIslands.Add(i);
                }
            }

            foreach (var index in deletingIslands)
            {
                Destroy(islands[index].gameObject);
                islands.RemoveAt(index);
            }

            foreach (var coordinate in islandCoordinates)
            {
                if(!IsIslandExists(coordinate))
                {
                    CreateIsland(coordinate);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Vector3 playerPos = player.ShipMovement.transform.position;
            Vector2Int origin = new Vector2Int((int)playerPos.x, (int)playerPos.z) - (noiseSize / 2);

            if (origin.x % interval != 0)
            {
                int diff = origin.x % interval;
                if (interval - diff > diff) // round to top
                {
                    origin.x += diff;

                }
                else
                {
                    origin.x += interval - diff;
                }
            }

            if (origin.y % interval != 0)
            {
                int diff = origin.y % interval;
                if (interval - diff > diff) // round to top
                {

                    origin.y += diff;
                }
                else
                {
                    origin.y += interval - diff;

                }
            }

            origin += Vector2Int.one;

            for (int i = 0; i < noiseSize.x; i += interval)
            {
                for (int j = 0; j < noiseSize.y; j += interval)
                {
                    float xCoord = origin.x + ((float)i * 0.01f) + 0.05f;
                    float yCoord = origin.y + ((float)j * 0.01f) + 0.05f;

                    float value = Mathf.PerlinNoise(xCoord, yCoord);

                    if (value > .6f)
                    {
                        Gizmos.color = Color.red;
                    }
                    else
                    {
                        Gizmos.color = Color.blue;
                    }
                    Gizmos.DrawWireCube(new Vector3(origin.x + i, 0, origin.y + j), Vector3.one * .95f);
                }
            }
        }*/
    }
}