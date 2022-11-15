using DG.Tweening;
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

        private string parentId;

        private void Start()
        {
            transform.DOMoveY(0, 2).From(transform.position.y).OnComplete(() =>
            {
                transform.DOMoveY(.2f, 1)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.Linear);
                transform.DORotate(Vector3.right * 30, 2)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.Linear);
            });
        }

        public void Set(string parentId)
        {
            this.parentId = parentId;
        }  


        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.TryGetComponent(out ShipStats shipStats))
            {
                if (shipStats.ID != parentId)
                {
                    shipStats.GainHealth(healthAmount);
                    Destroy(Instantiate(collectParticle, transform.position, Quaternion.identity).gameObject, 2);
                    Destroy(gameObject);
                }
            }
        }
    }
}