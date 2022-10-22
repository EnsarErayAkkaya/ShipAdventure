using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.CollisionSystem
{
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
}