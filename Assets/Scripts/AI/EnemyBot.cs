using EEA.FSM;
using EEA.General;
using EEA.Ship;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.Enemy
{
    public enum ShipAIType
    {
        Aggressive, Defansive, Tactical
    }
    public class EnemyBot : BaseStateMachine, IPlayer
    {
        [SerializeField] protected ShipAIType shipAIType; 
        [SerializeField] protected ShipMovement shipMovement;
        [SerializeField] protected ShipCannonShoot shipCannonShoot;
        public GameObject GameObject => gameObject;

        public ShipMovement ShipMovement => shipMovement;

        public ShipCannonShoot ShipCannonShoot => shipCannonShoot;

        protected override void MachineOnEnable()
        {
            base.MachineOnEnable();

            shipMovement.ShipStats.onHealthChange += OnHealthChange;
        }

        public void SetAIType(ShipAIType type)
        {
            shipAIType = type;
        }

        protected override void MachineOnDisable()
        {
            base.MachineOnDisable();
        }

        private void OnHealthChange(float healthPercent)
        {
            if(healthPercent <= 0)
            {
                StopMachine();
            }
        }
    }
}