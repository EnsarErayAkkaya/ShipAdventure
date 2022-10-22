using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EEA.UI
{
    public class ProgressBarBase : MonoBehaviour
    {
        
        [Header("Filler")]
        [SerializeField] protected Image filler;
        [SerializeField] private float fillDuration = 0.2f;

        public float CurrentFillAmount => _currentFillAmount;
        private float _currentFillAmount = 0f;
        private Coroutine _coroutine;

        private void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            _currentFillAmount = 0f;
            filler.fillAmount = _currentFillAmount;
            _coroutine = null;
        }

        public void ResetBar()
        {
            filler.fillAmount = 0;
        }

        public void UpdateProgress(float percentage, float begin, float end)
        {
            _currentFillAmount = Mathf.Lerp(begin, end, percentage);
            _currentFillAmount = _currentFillAmount > 1f ? 1f : _currentFillAmount < 0f ? 0f : _currentFillAmount;

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _coroutine = StartCoroutine(Progress());
        }

        public void UpdateProgressInstant(float percentage)
        {
            filler.fillAmount = _currentFillAmount;
            OnProgressChangeCompleted(_currentFillAmount);
        }

        private IEnumerator Progress()
        {
            var initialFillAmount = filler.fillAmount;

            var timer = 0f;
            while (timer <= fillDuration)
            {
                var nextFill = Mathf.Lerp(initialFillAmount, _currentFillAmount, timer / fillDuration);
                filler.fillAmount = nextFill;

                yield return null;
                timer += Time.deltaTime;
            }

            filler.fillAmount = _currentFillAmount;
            OnProgressChangeCompleted(_currentFillAmount);
        }

        protected virtual void OnProgressChangeCompleted(float progress)
        {

        }
        
    }
}
