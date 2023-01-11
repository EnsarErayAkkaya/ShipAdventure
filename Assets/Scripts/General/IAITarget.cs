using EEA.Ship;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.General
{
    public enum AITargetType
    {
        Ship, Island, Loot
    }
    public interface IAITarget
    {
        public Transform GetAITargetTranform { get; }

        public bool IsTargetValid(ShipMovement ship);

        public AITargetType TargetType { get; }
    }
}