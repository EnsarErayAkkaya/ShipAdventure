using EEA.Ship;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private ShipMovement shipMovement;
        [SerializeField] private ShipCannonShoot shipCannonShoot;

        private Camera mainCamera;

        public ShipMovement ShipMovement => shipMovement;
        public ShipCannonShoot ShipCannonShoot => shipCannonShoot;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        /*private void OnEnable()
        {
            LeanTouch.OnFingerUpdate += OnFingerUpdate;
        }

        private void OnDisable()
        {
            LeanTouch.OnFingerUpdate -= OnFingerUpdate;
        }

        private void OnFingerUpdate(LeanFinger finger)
        {
            if (finger.IsOverGui && finger.StartedOverGui) return;

            if(finger.Down ||finger.Tap)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, 200, shootableLayer, QueryTriggerInteraction.Collide))
                {
                    if(hit.collider.transform.GetComponent<ShipStat>() != null)
                }
            }
        }

        void ComputerControls()
        {
            float hInput = Input.GetAxis("Horizontal");
            float vInput = Input.GetAxis("Vertical");
            
            if(hInput != 0)
            {
                shipMovement.ModifyWheel(hInput);
            }

            if (vInput != 0)
            {
                shipMovement.ModifySail(vInput);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                shipMovement.ToggleAnchor();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                shipCannonShoot.ShootRightCannons();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                shipCannonShoot.ShootLeftCannons();
            }
        }*/
    }
}