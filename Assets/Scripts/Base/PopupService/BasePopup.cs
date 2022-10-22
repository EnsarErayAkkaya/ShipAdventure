using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace EEA.Services
{
    public class BasePopup : MonoBehaviour
    {
        public virtual void Show()
        {
            transform.DOScale(1, .75f).From(0);
        }


        public virtual void DestroyPopup()
        {
            Destroy(gameObject);

        }
    }
}