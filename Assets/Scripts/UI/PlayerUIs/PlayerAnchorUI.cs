using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EEA.UI
{
    public class PlayerAnchorUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private RectTransform seaImage;

        [SerializeField] private float seaAnchoredPos;
        [SerializeField] private float seaAnchorCollectedPos;
        [SerializeField] private float seaChangeDuration;
        [SerializeField] private Ease ease;

        private Player.Player player;
        private bool anchor;
        private void Start()
        {
            player = FindObjectOfType<Player.Player>();
            UpdateUI();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            player.ShipMovement.ToggleAnchor();

            UpdateUI();
        }

        private void UpdateUI()
        {
            float y = seaImage.anchoredPosition.y;
            float target;
            if (player.ShipMovement.Anchor)
            {
                target = seaAnchoredPos;
            }
            else
            {
                target = seaAnchorCollectedPos;
            }

            DOTween.Kill("PlayerAnchorUI");

            DOTween.To(() => y, (z) => y = z, target, seaChangeDuration)
                .OnUpdate(() =>
                {
                    seaImage.anchoredPosition = new Vector2(0, y);
                })
                .SetEase(ease)
                .SetId("PlayerAnchorUI");
        }
    }
}