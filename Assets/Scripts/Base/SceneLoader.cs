using Cysharp.Threading.Tasks;
using EEA.UI;
using System;
using UnityEngine;
namespace EEA
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private CanvasGroup loadingCanvas;
        [SerializeField] private ProgressBarBase loadingBar;

        public async UniTask StartLoading(Progress<float> progress)
        {
            progress.ProgressChanged += UpdateProgress;
            await ShowLoader();
            loadingBar.ResetBar();
        }

        public async UniTask EndLoading(Progress<float> progress)
        {
            progress.ProgressChanged -= UpdateProgress;
            loadingBar.UpdateProgress(1, 0, 1);
            await HideLoader();
        }

        private async UniTask HideLoader()
        {
            loadingCanvas.alpha = 1;
            while (loadingCanvas.alpha > 0)
            {
                loadingCanvas.alpha -= Time.deltaTime * 2f;
                await UniTask.Yield();
            }
            loadingCanvas.gameObject.SetActive(false);
        }

        private async UniTask ShowLoader()
        {
            loadingCanvas.gameObject.SetActive(true);
            loadingCanvas.alpha = 0;
            while (loadingCanvas.alpha < 1)
            {
                loadingCanvas.alpha += Time.deltaTime * 3f;
                await UniTask.Yield();
            }
        }

        private void UpdateProgress(object sender, float progress)
        {
            loadingBar.UpdateProgress(progress, 0, 1);
        }

    }
}
