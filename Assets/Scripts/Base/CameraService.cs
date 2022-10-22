using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace EEA
{
    public class CameraService : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera mainCamera;
        [SerializeField] private CinemachineVirtualCamera cityCamera;

        private List<CinemachineVirtualCamera> virtualCams;

        private void Awake()
        {
            virtualCams = FindObjectsOfType<CinemachineVirtualCamera>().ToList();
        }

        public void ActivateCamera(CinemachineVirtualCamera virtualCamera)
        {
            if(virtualCams.Contains(virtualCamera) == false)
            {
                virtualCams.Add(virtualCamera);
            }

            foreach (var item in virtualCams)
            {
                item.enabled = item == virtualCamera;
            }
        }
        public void ActivateMainCamera()
        {
            foreach (var item in virtualCams)
            {
                item.enabled = item == mainCamera;
            }
        }
        public void ToggleCityCamera()
        {
            if (!cityCamera.gameObject.activeInHierarchy)
            {
                ActivateCamera(cityCamera);
            }
            else
            {
                ActivateMainCamera();
            }
        }
    }
}