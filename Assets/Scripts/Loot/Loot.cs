using EEA.General;
using EEA.Ship;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.Loot
{
    public class Loot : MonoBehaviour, IAITarget
    {
        [SerializeField] private LootData lootData;
        private LootObject[] lootObjects;

        public LootObject[] loots => lootObjects;

        public Transform GetAITargetTranform => transform;

        public AITargetType TargetType => AITargetType.Loot;

        public bool IsTargetValid(ShipMovement ship)
        {
            return loots.Length > 0;
        }

        public void Set(string parentId)
        {
            GameObject go = Instantiate(lootData.lootPrefabs[Random.Range(0, lootData.lootPrefabs.Count)], transform.position, Quaternion.identity, transform);
            lootObjects = go.transform.GetComponentsInChildren<LootObject>();
            foreach (var obj in lootObjects)
            {
                obj.Set(parentId);
            }
        }
    }
}