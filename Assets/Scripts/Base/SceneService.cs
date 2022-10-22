using DevLocker.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;

namespace EEA.Services
{
    public class SceneService : MonoBehaviour
    {
        [Header("Referances")]
        [Tooltip("Menu or Home Scene")]
        [SerializeField] private SceneReference MetaScene;
        [SerializeField] private SceneLoader sceneLoader;

        private List<SceneReference> scenesInProgress = new List<SceneReference>();

        /// <summary>
        /// Load Meta Scene which is Menu or Home Scene
        /// </summary>
        /// <returns></returns>
        public async UniTask LoadMetaScene(bool showLoading = true)
        {
            if(MetaScene.IsEmpty)
            {
                Debug.LogError("Meta Scene is empty!!");
                return;
            }
            else if (scenesInProgress.Any(s => s == MetaScene))
            {
                Debug.LogError("A Scene is already loading");
                return;
            }
            IProgress<float> progress = new Progress<float>();
            if (showLoading)
            {
                await sceneLoader.StartLoading(progress as Progress<float>);
            }
            scenesInProgress.Add(MetaScene);

            await SceneManager.LoadSceneAsync(MetaScene.SceneName, LoadSceneMode.Additive).ToUniTask(progress);
            if (showLoading)
            {
                await sceneLoader.EndLoading(progress as Progress<float>);
            }
            scenesInProgress.Remove(MetaScene);
        }

        public async UniTask UnloadMetaScene()
        {
            if (MetaScene.IsEmpty)
            {
                Debug.LogError("Meta Scene is empty!!");
                return;
            }
            else if (scenesInProgress.Any(s => s == MetaScene))
            {
                Debug.LogError("A Scene is already loading");
                return;
            }

            scenesInProgress.Add(MetaScene);

            await SceneManager.UnloadSceneAsync(MetaScene.SceneName);
            scenesInProgress.Remove(MetaScene);
        }

        public async UniTask LoadScene(SceneReference sceneReference, bool showLoading = true, bool isFromMeta = false)
        {
            if (sceneReference.IsEmpty)
            {
                Debug.LogError("Given scene reference is empty!!");
                return;
            }
            else if (scenesInProgress.Any(s => s == sceneReference))
            {
                Debug.LogError("A Scene is already loading");
                return;
            }
            scenesInProgress.Add(sceneReference);

            IProgress<float> progress = new Progress<float>();
            if(showLoading)
                await sceneLoader.StartLoading(progress as Progress<float>);
           
            if(isFromMeta)
                await UnloadMetaScene();

            await SceneManager.LoadSceneAsync(sceneReference.SceneName, LoadSceneMode.Additive).ToUniTask(progress);

            if (showLoading)
                await sceneLoader.EndLoading(progress as Progress<float>);
            scenesInProgress.Remove(sceneReference);
        }

        public async UniTask UnloadScene(SceneReference sceneReference)
        {
            if (sceneReference.IsEmpty)
            {
                Debug.LogError("Given scene reference is empty!!");
                return;
            }
            else if (scenesInProgress.Any(s => s == sceneReference))
            {
                Debug.LogError("A Scene is already loading");
                return;
            }
            scenesInProgress.Add(sceneReference);

            await SceneManager.UnloadSceneAsync(sceneReference.SceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            scenesInProgress.Remove(sceneReference);
        }
    }
}