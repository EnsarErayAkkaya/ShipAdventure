using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EEA.UI
{
    public class LoadingSprite : MonoBehaviour
    {
        [SerializeField] private Image image;

        private Transform cam;
        private void Start()
        {
            cam = Camera.main.transform;
        }
        public void Set(float duration)
        {
            gameObject.SetActive(true);
            DOVirtual.Float(1, 0, duration, (float value) =>
            {
                transform.LookAt(cam);
                transform.Rotate(0, 180, 0);

                image.fillAmount = value;
            })
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }
}