using DG.Tweening;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EEA.UI;
using EEA.Attributes;
using System;
using System.Xml.Linq;

namespace EEA.Ship
{
    public class ShipCannonShoot : MonoBehaviour
    {
        [SerializeField] private ShipStats shipStats;
        [SerializeField] private SkinnedMeshRenderer cannons;
        [SerializeField] private Cannonball cannonball;

        [SerializeField] private Transform[] rightCannonShootPoints;
        [SerializeField] private Transform[] leftCannonShootPoints;

        [SerializeField] private float cannonShootInterval = 1;

        [SerializeField] private float cannonShootAngle = 20;

        private float leftLastShootTime;
        private float rightLastShootTime;

        private float attackPowerIncreatePercent;

        [SerializeField] private LoadingSprite rightLoadingSprite;
        [SerializeField] private LoadingSprite leftLoadingSprite;

        private void Start()
        {
            shipStats.onAttributesChange += OnAttributesChange;
        }

        private void OnAttributesChange(Dictionary<AttributeType, float> attributes)
        {
            if (attributes.ContainsKey(AttributeType.ATTACK_POWER_INCREASE_PERCENT))
            {
                attackPowerIncreatePercent = attributes[AttributeType.ATTACK_POWER_INCREASE_PERCENT];
            }
        }

        public void ShootLeftCannons()
        {
            if(leftLastShootTime + cannonShootInterval > Time.time)
            {
                return;
            }

            SetCannonBlendShapes(1);

            leftLastShootTime = Time.time;

            ShootCannons(false, leftCannonShootPoints);

            leftLoadingSprite.Set(cannonShootInterval);
        }

        public void ShootRightCannons()
        {
            if (rightLastShootTime + cannonShootInterval > Time.time)
            {
                return;
            }

            SetCannonBlendShapes(0);

            rightLastShootTime = Time.time;

            ShootCannons(true, rightCannonShootPoints);
            
            rightLoadingSprite.Set(cannonShootInterval);
        }

        private void ShootCannons(bool isRight, Transform[] shootTransforms)
        {
            Vector3 dir = isRight ? transform.right : -transform.right;

            dir = Quaternion.AngleAxis(isRight ? cannonShootAngle : -cannonShootAngle, transform.forward) * dir;

            for (int i = 0; i < shootTransforms.Length; i++)
            {
                LeanPool.Spawn(cannonball, shootTransforms[i].position, Quaternion.identity).Set(shipStats.ID, dir, attackPowerIncreatePercent);
            }
        }

        private void SetCannonBlendShapes(int id)
        {
            float x = 0;
            DOTween.Kill(id+"CannonShoot");
            DOTween.To(() => x, (float y) => x = y, 100.0f, .2f)
                .OnUpdate(() =>
                {
                    cannons.SetBlendShapeWeight(id, x);
                })
                .SetEase(Ease.InOutElastic)
                .OnStart(() =>
                {
                    cannons.SetBlendShapeWeight(id, 0);
                })
                .OnComplete(() =>
                {
                    DOTween.To(() => x, (float y) => x = y, 0.0f, .15f)
                    .OnUpdate(() =>
                    {
                        cannons.SetBlendShapeWeight(id, x);
                    })
                    .SetId(id + "CannonShoot");
                })
                .SetId(id + "CannonShoot");
        }
    }
}