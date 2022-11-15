using System.Collections.Generic;
using UnityEngine;

namespace EEA.Loot
{
    [CreateAssetMenu(fileName = "LootData", menuName = "Scriptable Objects/Loot Data")]
    public class LootData : ScriptableObject
    {
        public List<GameObject> lootPrefabs;
    }
}