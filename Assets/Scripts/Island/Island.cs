using EEA.Attributes;
using EEA.General;
using EEA.Managers;
using EEA.Ship;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EEA.Island
{
    public class Island : MonoBehaviour, IAITarget
    {
        [SerializeField] private IslandData islandData;
        [SerializeField] private MeshRenderer ringMeshRenderer;
        [SerializeField] private AttributeData attribute;
        [SerializeField] private Image attributeIcon;

        private ShipMovement owner = null;
        private float spendTime;

        private PlayerManager playerManager;
        private ShipMovement ship;

        private Material shaderRingMaterial;

        public Transform GetAITargetTranform => transform;
        public AITargetType TargetType => AITargetType.Island;

        private void Start()
        {
            playerManager = FindObjectOfType<PlayerManager>();
            shaderRingMaterial = ringMeshRenderer.material;           
        }

        public void SetAttribute(AttributeData attribute)
        {
            this.attribute = attribute;
            attributeIcon.sprite = attribute.icon;
        }

        private void FixedUpdate()
        {
            int closeShipCount = 0;
            for (int i = 0; i < playerManager.Players.Count; i++)
            {
                if(Vector3.Distance(playerManager.Players[i].ShipMovement.transform.position, transform.position) <= islandData.claimRadius)
                {
                    closeShipCount++;
                    ship = playerManager.Players[i].ShipMovement;
                    if(closeShipCount > 1)
                    {
                        spendTime = 0;
                        ship = null;
                        return;
                    }
                }
            }

            if(closeShipCount == 0)
            {
                spendTime = 0;
                ship = null;
                return;
            }

            ProcessIsland();
        }

        private void ProcessIsland()
        {
            if ((ship == null) || (owner != null && owner.ShipStats.ID == ship.ShipStats.ID)) // if close ship is the owner do nothing
            {
                return;
            }

            spendTime += Time.fixedDeltaTime;


            if (owner == null)
            {
                if (spendTime >= islandData.claimDuration)
                {
                    owner = ship;
                    owner.ShipStats.AddAttribute(attribute.AttributeType, attribute.value);
                    shaderRingMaterial.SetColor("color_", owner.ShipStats.Color);
                    spendTime = 0;
                }         
            }
            else
            {
                if (spendTime >= islandData.ownedClaimDuration)
                {
                    owner = ship;
                    shaderRingMaterial.SetColor("color_", owner.ShipStats.Color);
                    spendTime = 0;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, islandData.claimRadius);
        }

        public bool IsTargetValid(ShipMovement ship)
        {
            return ship != this.owner;
        }
    }
}