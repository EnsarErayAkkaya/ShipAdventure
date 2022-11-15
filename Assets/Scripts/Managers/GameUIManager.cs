using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.Managers
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject gameUI;
        [SerializeField] private GameObject winUI;
        [SerializeField] private GameObject loseUI;

        public void HideGameUI()
        {
            gameUI.SetActive(false);
        }

        public void ShowWinUI()
        {
            winUI.SetActive(true);
        }

        public void ShowLoseUI()
        {
            loseUI.SetActive(true);
        }
    }
}