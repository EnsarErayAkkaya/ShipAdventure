using UnityEngine;

namespace EEA.General
{
    public class Obstacle : MonoBehaviour, IDamageable
    {
        [SerializeField] private Collider collider;

        public Collider Collider => collider;

        public string ID => "-1";

        public bool TakeDamage(int value)
        {
            return false;
        }
    }
}