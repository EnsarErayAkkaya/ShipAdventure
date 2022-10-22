using EEA.Ship;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace EEA.Enemy
{
    public class CheckPerimeter
    {
        public static List<Collider> Check(Collider collider, Transform transform, float radius, LayerMask layerMask)
        {
            var colliders = Physics.OverlapSphere(transform.position, radius, layerMask, QueryTriggerInteraction.Collide).ToList();

            colliders.Remove(collider);

            return colliders;
        }
    }
}