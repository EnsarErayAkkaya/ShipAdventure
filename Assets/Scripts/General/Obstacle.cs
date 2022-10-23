using UnityEngine;

namespace EEA.General
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private Collider collider;

        public Collider Collider => collider;
    }
}