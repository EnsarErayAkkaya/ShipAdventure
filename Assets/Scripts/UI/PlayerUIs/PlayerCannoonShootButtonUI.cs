using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.UI
{
    public class PlayerCannoonShootButtonUI : MonoBehaviour
    {
        private Player.Player player;

        private void Start()
        {
            player = FindObjectOfType<Player.Player>();
        }

        public void RightCannonShoot()
        {
            player.ShipCannonShoot.ShootRightCannons();
        }

        public void LeftCannonShoot()
        {
            player.ShipCannonShoot.ShootLeftCannons();
        }
    }
}