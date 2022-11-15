using EEA.General;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EEA.Loot;
using DG.Tweening;
using EEA.UI;

namespace EEA.Ship
{
    public class ShipStats : BaseStat
    {
        [SerializeField] private Loot.Loot loot;
        [SerializeField] private HealthUI healthUI;
        
        private Color color = Color.black;
        private bool isPlayer = false;

        public bool IsPlayer => isPlayer;
        public Color Color => color;
        public Action<string> onDeath;
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

        private void OnDeath(float percent)
        {
            if(percent == 0)
            {
                DOVirtual.DelayedCall(4, () =>
                {
                    Instantiate(loot, transform.position, Quaternion.identity).Set(ID);
                    Destroy(gameObject);
                });
                onDeath(ID);
            }
        }
    }
}