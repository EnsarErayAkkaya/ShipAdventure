using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.Island
{
    [CreateAssetMenu(fileName = "IslandData", menuName = "Scriptable Objects/IslandData")]
    public class IslandData : ScriptableObject
    {
        public float claimRadius;
        public float claimDuration;
        public float ownedClaimDuration;
    }
}