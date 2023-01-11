using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.General
{
    public class BaseStat : MonoBehaviour, IDamageable
    {
        [SerializeField] private string id;

        [SerializeField] private int health = 100;
        [SerializeField] private int maxHealth = 100;

        public Action<float> onHealthChange;

        public string ID => id;

        public bool isDead => health <= 0;

        private void Start()
        {
            Init();
        }
        protected virtual void Init()
        {
            ModifyHealth(0);
        }

        public void SetID(string id)
        {
            this.id = id;
        }

        private bool ModifyHealth(int value)
        {
            health += value;

            if (health <= 0)
            {
                health = 0;
                Debug.Log("Dead");
                onHealthChange?.Invoke(0);
                return false;
            }
            else if (health > maxHealth)
            {
                health = maxHealth;
            }
            
            onHealthChange?.Invoke((float)health / 100.0f);

            return true;
        }

        public bool TakeDamage(int value)
        {
            if(health > 0)
            {
                return ModifyHealth(value);
            }
            return false;
        }

        public bool GainHealth(int value)
        {
            if (health > 0)
            {
                return ModifyHealth(value);
            }
            return false;
        }
    }
}