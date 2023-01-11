using EEA.Ship;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.General
{
    public interface IPlayer
    {
        public GameObject GameObject { get; }
        public ShipMovement ShipMovement { get; }
        public ShipCannonShoot ShipCannonShoot { get; }
    }
}