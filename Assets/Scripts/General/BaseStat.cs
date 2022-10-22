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

        public string ID => id;

        public Action<float> onHealthChange;
        private void Start()
        {
            id = Guid.NewGuid().ToString();
            ModifyHealth(0);
        }

        public bool ModifyHealth(int value)
        {
            health += value;

            if (health < 0)
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
            return ModifyHealth(value);
        }
    }
}