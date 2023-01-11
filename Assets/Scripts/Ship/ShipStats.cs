using EEA.General;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EEA.Loot;
using DG.Tweening;
using EEA.UI;
using EEA.Attributes;

namespace EEA.Ship
{
    public class ShipStats : BaseStat
    {
        [SerializeField] private Loot.Loot loot;
        [SerializeField] private HealthUI healthUI;
        
        private Color color = Color.black;
        private bool isPlayer = false;
        private Dictionary<AttributeType, float> attributes = new Dictionary<AttributeType, float>();

        public Action<string> onDeath;
        public Action<Dictionary<AttributeType, float>> onAttributesChange;

        public bool IsPlayer => isPlayer;
        public Color Color => color;
        protected override void Init()
        {
            base.Init();
            onHealthChange += OnDeath;
        }

        public void SetColor(Color c)
        {
            color = c;
            healthUI.ProgressBar.SetColor(color);
        }

        public void SetIsPlayer(bool isPlayer)
        {
            this.isPlayer = isPlayer;
        }

        public void AddAttribute(AttributeType type, float value)
        {
            if (!attributes.ContainsKey(type))
            {
                attributes[type] = 0;
            }
            attributes[type] += value;

            onAttributesChange(attributes);
        }

        public void RemoveAttribute(AttributeType type, float value)
        {
            if (attributes.ContainsKey(type))
            {
                attributes[type] -= value;
            }
            onAttributesChange(attributes);
        }

        private void OnDeath(float percent)
        {
            if(percent == 0)
            {
                DOVirtual.DelayedCall(4, () =>
                {
                    Instantiate(loot, transform.position, Quaternion.identity).Set(ID);
                    Destroy(gameObject, 3);
                });
                onDeath(ID);
            }
        }
    }
}