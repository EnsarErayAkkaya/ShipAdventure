using UnityEngine;
using EEA.Managers;
using System;
using EEA.General;
using System.Collections.Generic;
using EEA.Attributes;

namespace EEA.Ship
{
    public class ShipMovement : MonoBehaviour, IAITarget
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
        [SerializeField] private Collider _collider;
        
        [Header("Referances")]
        [SerializeField] private ShipStats shipStats;
            
        //private Vector3 halfBoundingBox;

        private float windAngle;
        private float speed;
        private Vector3 drag;

        private float speedIncreasePercent;
        
        private float anchor = 0; // 0 anchore on ground, 1 anchor collected
        private bool anchorFree = true;  // true anchor falling, false anchor collecting or collected

        // AI Target Interface parameters
        public Transform GetAITargetTranform => transform;
        public AITargetType TargetType => AITargetType.Ship;
        //

        public ShipStats ShipStats => shipStats;
        public Collider Collider => _collider;
        //public Vector3 HalfBoundingBox => halfBoundingBox;
        public float Sail => sail;
        public float WheelAngle => wheelAngle;
        public float Angle => transform.rotation.eulerAngles.y;
        /// <summary>
        /// If true means iron released and ship stop else iron collected and ship free
        /// </summary>
        public bool Anchor => anchorFree;

        private void Start()
        {
            shipStats.onDeath += OnDeath;
            shipStats.onAttributesChange += OnAttributesChange;
        }

        private void OnAttributesChange(Dictionary<AttributeType, float> attributes)
        {
            if(attributes.ContainsKey(AttributeType.MOVE_SPEED_INCREASE_PERCENT))
            {
                speedIncreasePercent = attributes[AttributeType.MOVE_SPEED_INCREASE_PERCENT];
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent<ShipMovement>(out var otherShip))
            {
                Vector3 dir = (otherShip.transform.position - transform.position).normalized;
                float collisionAngle = Mathf.Abs(Vector3.SignedAngle(transform.forward, dir, Vector3.up));

                if (collisionAngle < collisionCloseAngle)
                {
                    speed *= .5f;
                    otherShip.drag = speed * transform.forward * dragMultiplier;
                }
            }
            else if (other.transform.TryGetComponent<Obstacle>(out var obstacle))
            {
                Vector3 dir = (obstacle.transform.position - transform.position).normalized;
                float collisionAngle = Mathf.Abs(Vector3.SignedAngle(transform.forward, dir, Vector3.up));
                collisionAngle = Math.Max(collisionAngle, 90) / 90.0f; 

                speed *= Mathf.Lerp(1,0, collisionAngle);
                
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.transform.TryGetComponent<ShipMovement>(out var otherShip))
            {
                Vector3 dir = (otherShip.transform.position - transform.position).normalized;
                float collisionAngle = Mathf.Abs(Vector3.SignedAngle(transform.forward, dir, Vector3.up));

                if (collisionAngle < collisionCloseAngle)
                {
                    speed *= .5f;
                    otherShip.drag = speed * transform.forward * dragMultiplier;
                }
            }
            else if (other.transform.TryGetComponent<Obstacle>(out var obstacle))
            {
                Vector3 dir = (obstacle.transform.position - transform.position).normalized;
                float collisionAngle = Mathf.Abs(Vector3.SignedAngle(transform.forward, dir, Vector3.up));
                collisionAngle = Math.Max(collisionAngle, 90) / 90.0f;

                speed *= Mathf.Lerp(1, 0, collisionAngle);

            }
        }

        private void OnDeath(string id)
        {
            Collider.enabled = false;
            if (!Anchor)
            {
                ToggleAnchor();
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

            windAngle = Vector2.Angle(forward, wind);

            // base speed
            float target_speed = minSpeed;

            if (windAngle < 90) // if wind effects
            {
                target_speed = ((90 - windAngle) / 90) * windSpeedEffect * sail;
            }

            if (sail > 0) // opening sail even against wind increase speed a little
            {
                target_speed += sailSpeedEffect * sail;
            }

            target_speed += target_speed * speedIncreasePercent;

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

        public bool IsTargetValid(ShipMovement ship)
        {
            return !this.shipStats.isDead;
        }
    }
}