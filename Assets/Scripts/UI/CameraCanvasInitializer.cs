using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA
{
    public class CameraCanvasInitializer : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        private void Start()
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        }
    }
}