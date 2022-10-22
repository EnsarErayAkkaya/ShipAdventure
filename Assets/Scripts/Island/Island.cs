using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Island
{
    public class Island : MonoBehaviour
    {
        [SerializeField] private float lootRadius;
        [SerializeField] private float lootDuration;

        private Vector2Int coordinate;

        public Vector2Int Coordinate => coordinate;
        public void Init(Vector2Int _coordinate)
        {
            coordinate = _coordinate;
        }

        private void FixedUpdate()
        {
            Physics.OverlapSphere(transform.position, lootRadius);
        }
    }
}