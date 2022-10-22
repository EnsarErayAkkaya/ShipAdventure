using System.Collections.Generic;
using UnityEngine;
namespace EEA.Services
{
    [System.Serializable]
    public class Popup
    {
        public string id;
        public BasePopup popup;
    }
    [CreateAssetMenu(fileName = "PopupSettings", menuName = "Scriptable Objects/Popup Settings")]
    public class PopupSettings : ScriptableObject
    {
        public List<Popup> popups;
    }
}
