using EEA.General;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Ship
{
    public class Cannonball : MonoBehaviour, IPoolable
    {
        private string owner;
        private Vector3 direction;

        [SerializeField] private float speed;
        [SerializeField] private float gravity;
        [SerializeField] private float gravityEffectAfter;
        [SerializeField] private int damage;
        [SerializeField] private ParticleSystem explosionParticle;

        private float attackPowerIncreatePercent;
        private float t = 0;
        public void Set(string owner, Vector3 direction, float attackPowerIncreatePercent)
        {
            this.owner = owner;
            this.direction = direction;
            this.attackPowerIncreatePercent = attackPowerIncreatePercent;
        }

        private void FixedUpdate()
        {
            t += Time.deltaTime;

            if(t > gravityEffectAfter)
            {
                direction += new Vector3(0, -gravity, 0) * Time.deltaTime;
            }

            transform.position += direction * speed * Time.fixedDeltaTime;

            if(transform.position.y < 0)
            {
                LeanPool.Despawn(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out IDamageable _stats))
            {
                if(_stats.ID != owner)
                {
                    _stats.TakeDamage(-(damage + (int)(damage * attackPowerIncreatePercent)));

                    LeanPool.Despawn(gameObject);

                    var expl_particle = LeanPool.Spawn(explosionParticle);
                    expl_particle.transform.position = transform.position;

                    LeanPool.Despawn(expl_particle, 1.5f);
                }
            }
        }

        public void OnDespawn()
        {
        }

        public void OnSpawn()
        {
            direction = Vector3.zero;
            t = 0;
        }
    }
}