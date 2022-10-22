using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera cam;
        [SerializeField] private bool isUI;
        private void Start()
        {
            cam = Camera.main;
        }
        void Update()
        {
            transform.LookAt(cam.transform);
            if (isUI)
                transform.Rotate(Vector3.up, 180);
        }
    }
}