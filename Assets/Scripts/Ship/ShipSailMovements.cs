using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.Ship
{
    public class ShipSailMovements : MonoBehaviour
    {
        [SerializeField] private ShipMovement shipMovement;
        [SerializeField] private SkinnedMeshRenderer sail;

        void Update()
        {
            float value = (1 - shipMovement.Sail) * 100;
            sail.SetBlendShapeWeight(0, value);
        }
    }
}