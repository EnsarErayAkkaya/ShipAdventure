using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.Ship
{
    public class ShipAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private ShipStats shipStats;
        private void Start()
        {
            shipStats.onDeath += OnDeath;
        }

        public void Death()
        {
            animator.SetTrigger("Death");
        }

        private void OnDeath(string id)
        {
            Death();     
        }
    }
}