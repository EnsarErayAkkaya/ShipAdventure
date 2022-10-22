using EEA.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.Loot
{
    public class Loot : MonoBehaviour
    {
        [SerializeField] private LootData lootData;

        private void Start()
        {
            Instantiate(lootData.lootPrefabs[Random.Range(0, lootData.lootPrefabs.Count)], transform.position, Quaternion.identity);
        }
    }
}