using UnityEngine;
using EEA.Managers;
using System;
using EEA.General;

namespace EEA.Ship
{
    public class ShipMovement : MonoBehaviour
    {
        [Header("Player Update Modifiers")]
        [SerializeField] private float wheelModifyMultiplier;
        [SerializeField] private float sailModifyMultiplier;
        
        [Header("Values (No need to serialize)")]
        [SerializeField] private float sail = 0; // 0(close) - 1(open)
        [SerializeField] private float wheelAngle = 0; // -1(left) - 1(right)
        
        [Header("Speed Modifiers")]
        [SerializeField] private float minSpeed;
        [SerializeField] private float windSpeedEffect;
        [SerializeField] private float sailSpeedEffect;
        [SerializeField] private float speedChangeStep = .1f;

        [Header("Rotation Modifiers")]
        [SerializeField] private float rotationChangeMultiplier;

        [Header("Anchor")]
        [SerializeField] private float anchorFallDuration;
        [SerializeField] private float anchorCollectDuration;

        [Header("Collision")]
        [SerializeField] private float dragFriction = .9f;
        [SerializeField] private float dragMultiplier = 5;
        [SerializeField] private float collisionCloseAngle = 45;
        
            
        private Vector3 halfBoundingBox;


        private WindManager WindManager;
        private float angle;
        private float speed;
        private Vector3 drag;
        
        private float anchor = 0; // 0 anchore on ground, 1 anchor collected
        private bool anchorFree = true;  // true anchor falling, false anchor collecting or collected

        public Vector3 HalfBoundingBox => halfBoundingBox;
        public float Sail => sail;
        public float WheelAngle => wheelAngle;
        /// <summary>
        /// If true means iron released and ship stop else iron collected and ship free
        /// </summary>
        public bool Anchor => anchorFree;

        private void Start()
        {
            WindManager = WindManager.instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent<ShipMovement>(out var otherShip))
            {
                Vector3 dir = (otherShip.transform.position - transform.position).normalized;
                float angle = Mathf.Abs(Vector3.SignedAngle(transform.forward, dir, Vector3.up));

                if (angle < collisionCloseAngle)
                {
                    speed *= .5f;
                    otherShip.drag = speed * transform.forward * dragMultiplier;
                }
            }
            else if (other.transform.TryGetComponent<Obstacle>(out var obstacle))
            {
                Vector3 dir = (obstacle.transform.position - transform.position).normalized;
                float angle = Mathf.Abs(Vector3.SignedAngle(transform.forward, dir, Vector3.up));
                angle = Math.Max(angle, 90) / 90.0f; 

                speed *= Mathf.Lerp(1,0, angle);
                
            }
        }

        private void FixedUpdate()
        {
            CalculateSpeed();

            transform.position += (drag.magnitude < .2f) ? (speed * transform.forward * Time.deltaTime) : (drag * Time.deltaTime);

            drag *= dragFriction * Time.deltaTime;

            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 
                (wheelAngle * rotationChangeMultiplier * Time.deltaTime * anchor), 0);
            
        }

        private void CalculateSpeed()
        {
            if (anchorFree)
            {
                anchor -= Time.deltaTime / anchorFallDuration;
            }
            else
            {
                anchor += Time.deltaTime / anchorCollectDuration;
            }

            anchor = Mathf.Clamp01(anchor);

            Vector2 forward = new Vector2(transform.forward.x, transform.forward.z);  
            Vector2 wind = WindManager.instance.wind;

            angle = Vector2.Angle(forward, wind);

            float target_speed = minSpeed;

            if (angle < 90) // if wind effects
            {
                target_speed = ((90 - angle) / 90) * windSpeedEffect * sail;
            }

            if (sail > 0) // opening sail even against wind increase speed a little
            {
                target_speed += sailSpeedEffect * sail;
            }

            target_speed *= anchor; // if anchor on ground (0) speed is 0 if anchor collected speed is itself

            bool speedIncreasing = target_speed > speed;

            if (speedIncreasing)
            {
                speed = speed + (speedChangeStep * Time.deltaTime) > target_speed ? target_speed : speed + (speedChangeStep * Time.deltaTime);
            }
            else
            {
                speed = speed - (speedChangeStep * Time.deltaTime) < target_speed ? target_speed : speed - (speedChangeStep * Time.deltaTime);
            }

            if(anchor == 0)
            {
                speed = 0;
            }
        }


        /// <summary>
        /// value must be -1 or 1
        /// </summary>
        /// <param name="value"></param>
        public void ModifyWheel(float value)
        {
            wheelAngle += Time.deltaTime * wheelModifyMultiplier * value;
            wheelAngle = Mathf.Clamp(wheelAngle, -1.0f, 1.0f);
        }

        /// <summary>
        /// value must be -1 or 1
        /// </summary>
        /// <param name="value"></param>
        public void SetWheel(float value)
        {
            wheelAngle = Mathf.Clamp(value, -1.0f, 1.0f);
        }

        /// <summary>
        /// value must be -1 or 1
        /// </summary>
        /// <param name="value"></param>
        public void ModifySail(float value)
        {
            sail += Time.deltaTime * sailModifyMultiplier * value;
            sail = Mathf.Clamp01(sail);
        }

        /// <summary>
        /// value must be 0 or 1
        /// </summary>
        /// <param name="value"></param>
        public void SetSail(float value)
        {
            sail = value;
            sail = Mathf.Clamp01(sail);
        }

        public void ToggleAnchor()
        {
            anchorFree = !anchorFree;
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Vector3 forward = transform.forward;
                Vector2 tempWind = WindManager.instance.wind;
                Vector3 wind = new Vector3(tempWind.x, 0, tempWind.y);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position + forward, transform.position + forward * 2);
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position + wind, transform.position + wind * 2);
                /*Gizmos.color = Color.green;
                Gizmos.DrawWireCube(transform.position, halfBoundingBox * 2);*/
            }
        }
    }
}