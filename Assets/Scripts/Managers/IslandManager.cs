using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EEA.Player;
using System.Linq;
using EEA.Attributes;

namespace EEA.Managers
{
    public class IslandManager : MonoBehaviour
    {
        [SerializeField] private AttributesData attributesData;
        private Island.Island[] islands;

        private void Start()
        {
            islands = FindObjectsOfType<Island.Island>();

            for (int i = 0; i < islands.Length; i++)
            {
                islands[i].SetAttribute(attributesData.data[i % attributesData.data.Count]);
            }
        }
    }
}