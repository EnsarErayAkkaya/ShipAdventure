using DG.Tweening;
using EEA.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Lean.Touch.LeanGestureToggle;

namespace EEA.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private ProgressBarBase progressBarBase;

        [SerializeField] private BaseStat baseStat;

        public ProgressBarBase ProgressBar => progressBarBase;

        private void OnEnable()
        {
            baseStat.onHealthChange += OnHealthChange;
        }
        private void OnDisable()
        {
            baseStat.onHealthChange -= OnHealthChange;
        }

        private void OnHealthChange(float value)
        {
            if(value <= 0)
            {
                progressBarBase.gameObject.SetActive(false);
                return;
            }
            progressBarBase.UpdateProgress(value, 0, 1);
        }
    }
}