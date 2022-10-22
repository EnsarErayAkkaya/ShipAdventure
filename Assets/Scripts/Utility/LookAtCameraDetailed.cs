using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA
{
    public class LookAtCameraDetailed : MonoBehaviour
    {
        private Camera cam;
        [SerializeField] private bool isUI;
        private Vector3 rot;
        [SerializeField] private bool x;
        [SerializeField] private bool y;
        [SerializeField] private bool z;
        private void Start()
        {
            cam = Camera.main;
        }
        void Update()
        {
            rot = Quaternion.LookRotation((cam.transform.position - transform.position), Vector3.up).eulerAngles;
            rot = new Vector3(x ? rot.x : transform.rotation.x, y ? rot.y : transform.rotation.y, z ? rot.z : transform.rotation.z);
            transform.rotation = Quaternion.Euler(rot);
            //transform.LookAt(cam.transform);
            if (isUI)
                transform.Rotate(Vector3.up, 180);
        }
    }
}