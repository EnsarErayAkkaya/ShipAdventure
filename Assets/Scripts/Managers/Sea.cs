using EEA.General;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EEA.Pathfinding;

namespace EEA.Managers
{
    public class Sea : MonoBehaviour
    {
        [SerializeField] private Pathfinding.Grid grid;
        [SerializeField] private Obstacle[] obstacles;
        [SerializeField] private Collider seaCollider;
        [SerializeField] private float safeRadius;
        
        private static Sea instance;

        public float SafeRadius => safeRadius;

        public struct SeaPos
        {
            public Vector3 pos;
            public bool success;
            public SeaPos(Vector3 _pos, bool _success)
            {
                pos = _pos;
                success = _success;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public SeaPos GetRandomAreaOnSea()
        {
            Vector2 unitSphere = Random.insideUnitCircle;
            int tryCount = 0;
            while (tryCount < 5)
            {
                tryCount++;
                Vector3 possiblePos = new Vector3(unitSphere.x, 0, unitSphere.y) * safeRadius;

                if(!IsPositionInObstacle(possiblePos) && grid.NodeFromWorldPoint(possiblePos).walkable)
                {
                    Debug.Log("Sea pos found at " + tryCount + " try");
                    return new SeaPos(possiblePos, true);
                }
            }

            Debug.Log("Couldn't found sea pos");
            return new SeaPos(Vector3.zero, false);
        }

        public Vector3 GetBounds()
        {
            return new Vector3(seaCollider.bounds.max.x - seaCollider.bounds.min.x, 0, seaCollider.bounds.max.z - seaCollider.bounds.min.z);
        }

        public bool IsPositionInObstacle(Vector3 pos)
        {
            return obstacles.Any(obs => obs.Collider.bounds.Contains(pos));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(gameObject.transform.position, safeRadius);
        }
    }
}