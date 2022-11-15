using EEA.Attributes;
using EEA.Managers;
using EEA.Ship;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Island
{
    public class Island : MonoBehaviour
    {
        [SerializeField] private IslandData islandData;
        [SerializeField] private MeshRenderer ringMeshRenderer;
        [SerializeField] private AttributeType attributeType;

        private ShipMovement owner = null;
        private float spendTime;

        private AttributeManager attributeManager;
        private ShipManager shipManager;
        private ShipMovement ship;

        private Material shaderRingMaterial;

        private void Start()
        {
            attributeManager = FindObjectOfType<AttributeManager>();
            shipManager = FindObjectOfType<ShipManager>();
            shaderRingMaterial = ringMeshRenderer.material;
        }

        private void FixedUpdate()
        {
            int closeShipCount = 0;
            for (int i = 0; i < shipManager.Ships.Count; i++)
            {
                if(Vector3.Distance(shipManager.Ships[i].transform.position, transform.position) <= islandData.claimRadius)
                {
                    closeShipCount++;
                    ship = shipManager.Ships[i];
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
                    attributeManager.Add(owner.ShipStats.ID, attributeType);
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
    }
}