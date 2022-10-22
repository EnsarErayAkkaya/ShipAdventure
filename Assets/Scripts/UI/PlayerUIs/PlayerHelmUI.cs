using DG.Tweening;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EEA.UI
{
    public class PlayerHelmUI : MonoBehaviour
    {
        /*[SerializeField] private RectTransform helmParent;
        [SerializeField] private RectTransform helmIdleRect;
        [SerializeField] private Camera UICamera;*/
        [SerializeField] private Image helmImage;
        [SerializeField] private float angle;
        [SerializeField] private float wheelTurnMultiplier;

        private Player.Player player;

        private void Start()
        {
            player = FindObjectOfType<Player.Player>();
        }
        private void OnEnable()
        {
            LeanTouch.OnFingerUpdate += OnFingerUpdate;
        }

        private void OnDisable()
        {
            LeanTouch.OnFingerUpdate -= OnFingerUpdate;
        }

        private void OnFingerUpdate(LeanFinger finger)
        {
            if (finger.IsOverGui || finger.StartedOverGui) return;

            /*if(finger.Down && !finger.Tap) // activate
            {
                Vector2 anchoredPos;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(helmParent, finger.ScreenPosition, UICamera, out anchoredPos);

                helmImage.rectTransform.anchoredPosition = anchoredPos;

                //helmImage.DOFade(1, .35f);
            }
            else if (finger.Up || finger.Tap) // deactivate
            {
                //helmImage.rectTransform.anchoredPosition = Vector2.zero;
                //helmImage.DOFade(0, .35f);
            }
            else
            {*/
                float delta = finger.ScreenDelta.x / Screen.width * wheelTurnMultiplier;

                player.ShipMovement.ModifyWheel(delta);

                float wheelAngle = player.ShipMovement.WheelAngle;
                wheelAngle = -((wheelAngle + 1) * .5f * (angle * 2) - angle);
                helmImage.transform.rotation = Quaternion.Euler(helmImage.transform.rotation.eulerAngles.x, helmImage.transform.rotation.eulerAngles.y, wheelAngle);
            //}
        }
    }
}