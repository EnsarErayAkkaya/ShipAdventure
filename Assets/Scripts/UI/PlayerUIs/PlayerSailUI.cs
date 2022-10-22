using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EEA.UI
{
    public class PlayerSailUI : MonoBehaviour, IDragHandler
    {
        [SerializeField] private RectTransform sailHandle;
        [SerializeField] private float maxDistance;
        
        private Player.Player player;
        private float y;

        private void Start()
        {
            player = FindObjectOfType<Player.Player>();

            sailHandle.anchoredPosition = new Vector2(0, maxDistance * player.ShipMovement.Sail);
        }

        public void OnDrag(PointerEventData eventData)
        {
            y = sailHandle.anchoredPosition.y + eventData.delta.y;
            y = Mathf.Clamp(y, 0, maxDistance);

            sailHandle.anchoredPosition = new Vector2(0, y);

            player.ShipMovement.SetSail(y / maxDistance);
        }
    }
}