using EEA.Ship;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.Loot
{
    public class LootObject : MonoBehaviour
    {
        [SerializeField] private ParticleSystem collectParticle;

        [SerializeField] private int healthAmount;

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.TryGetComponent(out ShipStats shipStats))
            {
                shipStats.ModifyHealth(healthAmount);
                Destroy(Instantiate(collectParticle, transform.position, Quaternion.identity).gameObject, 2);
                Destroy(gameObject);
            }
        }
    }
}