using EEA.FSM;
using EEA.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Enemy.Kraken
{
    public class KrakenAI : BaseStateMachine
    {
        private PlayerManager playerManager;
        [SerializeField] private float attackAreaRadius;

        protected override void Init()
        {
            playerManager = FindObjectOfType<PlayerManager>();
            base.Init();

        }
        public bool CheckAnyShipInsideAttackArea()
        {
            for (int i = 0; i < playerManager.Players.Count; i++)
            {
                Vector3 shipPos = playerManager.Players[i].ShipMovement.transform.position;
                shipPos.y = transform.position.y;

                if ((shipPos - transform.position).sqrMagnitude < (attackAreaRadius * attackAreaRadius))
                {
                    return true;
                }
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, attackAreaRadius);
        }
    }
}