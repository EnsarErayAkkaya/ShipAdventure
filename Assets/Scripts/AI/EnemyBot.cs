using EEA.FSM;
using EEA.General;
using EEA.Ship;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.Enemy
{
    public class EnemyBot : BaseStateMachine
    {
        [SerializeField] protected ShipMovement shipMovement;
        [SerializeField] private BaseStat baseStat;

        protected override void MachineOnEnable()
        {
            base.MachineOnEnable();

            baseStat.onHealthChange += OnHealthChange;
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