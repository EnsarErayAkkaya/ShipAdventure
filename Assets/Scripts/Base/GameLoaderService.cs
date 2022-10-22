using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.Services
{
    public class GameLoaderService : MonoBehaviour
    {
        [Header("Referances")]
        [SerializeField] private SceneService sceneService;
        [SerializeField] private SceneLoader sceneLoader;
        [SerializeField] private SaveService saveService;

        public Progress<float> ProgressReport => (loadingProgress as Progress<float>);
        private IProgress<float> loadingProgress;

        private void Start()
        {
            loadingProgress = new Progress<float>();
            LoadGameAsync().Forget();
        }

        private async UniTask LoadGameAsync()
        {
            await sceneLoader.StartLoading(ProgressReport);

            saveService.LoadGame();

            await UniTask.Delay(TimeSpan.FromSeconds(.5f));
            loadingProgress.Report(.5f);

            await sceneService.LoadMetaScene(false);

            await sceneLoader.EndLoading(ProgressReport);
        }
    }
}