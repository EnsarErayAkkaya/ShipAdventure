using UnityEngine;

namespace EEA.General
{
    public class Obstacle : MonoBehaviour
    {
        private Collider collider;

        private void Start()
        {
            collider = GetComponent<Collider>();
        }

        public Collider Collider => collider;
    }
}