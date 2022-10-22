using Lean.Touch;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EEA.UI
{
    public class PlayerCannonShootUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private RectTransform shootHandleUI;
        [SerializeField] private float shootDistance = 200;
        [SerializeField] private float shootCloseCheck = 10;
        [SerializeField] private float returnDuration = .3f;
        [SerializeField] private float smallerAmount = .75f;

        private Player.Player player;
        private float x;
        private bool isDragging;
        private float t;
        private float returnStartPos;
        private Vector2 startingSize;

        private void Start()
        {
            player = FindObjectOfType<Player.Player>();
            startingSize = shootHandleUI.sizeDelta;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
            shootHandleUI.sizeDelta = startingSize * smallerAmount;
        }

        public void OnDrag(PointerEventData eventData)
        {
            x = shootHandleUI.anchoredPosition.x + eventData.delta.x;
            x = Mathf.Clamp(x, -shootDistance, shootDistance);

            shootHandleUI.anchoredPosition = new Vector2(x, 0);
        }

        private void Update()
        { 
            if(!isDragging)
            {
                x = Mathf.Lerp(returnStartPos, 0, t);
                t += Time.deltaTime / returnDuration;

                shootHandleUI.anchoredPosition = new Vector2(x, 0);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            shootHandleUI.sizeDelta = startingSize;
            returnStartPos = x;
            t = 0;
            isDragging = false;
            if (Mathf.Abs(shootHandleUI.anchoredPosition.x) + shootCloseCheck > shootDistance)
            {
                if (shootHandleUI.anchoredPosition.x > 0)
                {
                    player.ShipCannonShoot.ShootRightCannons();
                }
                else
                {
                    player.ShipCannonShoot.ShootLeftCannons();
                }
            }
        }
    }
}