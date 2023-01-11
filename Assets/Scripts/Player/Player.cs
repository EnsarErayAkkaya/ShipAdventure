using EEA.General;
using EEA.Managers;
using EEA.Ship;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Player
{
    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField] private ShipMovement shipMovement;
        [SerializeField] private ShipCannonShoot shipCannonShoot;
        [SerializeField] private float maxSwipeDistance;
        [SerializeField] private float angleCloseLimit;

        private Camera cam;
        private float cameraOffsetRotation;

        public ShipMovement ShipMovement => shipMovement;
        public ShipCannonShoot ShipCannonShoot => shipCannonShoot;
        public GameObject GameObject => gameObject;

        private void Start()
        {
            shipMovement.ShipStats.SetIsPlayer(true);
        }

        /* private void Start()
         {
             cam = Camera.main;
             cameraOffsetRotation = cam.transform.rotation.y;
         }

         private void OnEnable()
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

             Vector2 fingerDir = finger.SwipeScaledDelta;
             //float targetAngle = Mathf.Atan2(fingerDir.x, fingerDir.y) * Mathf.Rad2Deg + 180;

             Vector2 shipDir = new Vector2(shipMovement.transform.forward.x, shipMovement.transform.forward.z);

             float angle = Vector2.SignedAngle(fingerDir, shipDir);

             Debug.Log(angle);

             shipMovement.SetWheel(Mathf.Sign(angle));
         }*/

        /*void ComputerControls()
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