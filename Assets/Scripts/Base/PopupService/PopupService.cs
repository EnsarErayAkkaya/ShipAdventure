using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace EEA.Services
{
    public class PopupService : MonoBehaviour
    {
        [SerializeField] private PopupSettings popupSettings;
        [SerializeField] private Canvas popupCanvas;

        private Dictionary<string, BasePopup> popups = new Dictionary<string, BasePopup>();

        public void CreateAndShowPopup(string id)
        {
            if(!popups.ContainsKey(id))
            {
                BasePopup popupPrefab = popupSettings.popups.FirstOrDefault(s => s.id == id).popup;
                BasePopup popup = Instantiate(popupPrefab, popupCanvas.transform);
                popup.Show();
            }
        }

        public void DestroyPopup(string id)
        {
            if(popups.ContainsKey(id))
            {
                Destroy(popups[id].gameObject);
                popups.Remove(id);  
            }
        }
    }
}
