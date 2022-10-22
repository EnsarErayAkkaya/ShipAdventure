using EEA.Ship;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using System;
using System.Drawing;

namespace EEA.Managers
{
    [Serializable]
    public class Box
    {
        private Vector3 first;
        private Vector3 second;
        private Vector3 third;
        private Vector3 fourth;

        private Vector3[] vertices;

        public Box(Vector3 halfSize)
        {
            first = new Vector3(-halfSize.x, halfSize.y, -halfSize.z);
            second = new Vector3(-halfSize.x, halfSize.y, halfSize.z);
            third = new Vector3(halfSize.x, halfSize.y, halfSize.z);
            fourth = new Vector3(halfSize.x, halfSize.y, -halfSize.z);
            vertices = new Vector3[4];
        }

        public Vector3[] GetVertices() => vertices;

        public bool IsOverlaps(Box other)
        {
            // l1 topleft, r1 bottomright -- l2 topleft, r2 topright

            Vector3 l1 = vertices[0];
            Vector3 l2 = other.GetVertices()[0];

            Vector3 r1 = vertices[3];
            Vector3 r2 = other.GetVertices()[3];

            // If one rectangle is on left side of other
            if (l1.x > r2.x || l2.x > r1.x)
                return false;

            // If one rectangle is above other
            if (r1.z > l2.z || r2.z > l1.z)
                return false;

            return true;
        }

        public Vector3[] ReturnBoxByAngle(Vector3 pos, float _angle)
        {
            var quat = Quaternion.AngleAxis(_angle, Vector3.up);

            vertices = new[]
            {
                pos + (quat * first), pos + (quat * second), pos + (quat * third), pos + (quat * fourth)
            };
            return vertices;
        }
    }
    public class ShipCollisionManager : MonoBehaviour
    {
        private List<ShipMovement> shipMovements;
        private List<Box> boxes;

        private void Start()
        {
            shipMovements = FindObjectsOfType<ShipMovement>().ToList();
            boxes = new List<Box>();
            foreach (var item in shipMovements)
            {
                boxes.Add(new Box(item.HalfBoundingBox));
            }
        }

       private void FixedUpdate()
        {      
            for (int i = 0; i < shipMovements.Count - 1; i++)
            {
                boxes[i].ReturnBoxByAngle(shipMovements[i].transform.position, Vector3.SignedAngle(Vector3.forward, shipMovements[i].transform.forward, Vector3.up));


                for (int j = i + 1; j < shipMovements.Count; j++)
                {
                    boxes[j].ReturnBoxByAngle(shipMovements[j].transform.position, Vector3.SignedAngle(Vector3.forward, shipMovements[j].transform.forward, Vector3.up));

                    if (boxes[i].IsOverlaps(boxes[j]))
                    {
                        Debug.Log("OVERLAPS");
                    }
                }
            }
        }
 

        private void OnDrawGizmos()
        {
            Gizmos.color = UnityEngine.Color.red;
            if (Application.isPlaying)
            {
                for (int i = 0; i < shipMovements.Count; i++)
                {
                    Vector3[] box = boxes[i].GetVertices();

                    Gizmos.DrawLine(box[0], box[1]);
                    Gizmos.DrawLine(box[1], box[2]);
                    Gizmos.DrawLine(box[2], box[3]);
                    Gizmos.DrawLine(box[3], box[0]);
                }
            }
        }
    }
}