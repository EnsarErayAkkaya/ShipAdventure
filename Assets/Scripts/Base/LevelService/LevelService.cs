using Cysharp.Threading.Tasks;
using DevLocker.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.Services
{
    public struct LevelStartedData
    {
        public int currentLevelIndex;
    }
    public struct LevelStatusData
    {
        public float completePercent;
        public int currentLevelIndex;
    }
    public class LevelService : MonoBehaviour
    {
        [Header("Referances")]
        [SerializeField] protected SceneService sceneService;
        [SerializeField] protected LevelServiceSettings levelServiceSettings;

        public delegate void OnLevelStatusUpdated(LevelStatusData levelStatusData);
        public OnLevelStatusUpdated onLevelStatusChanged;

        public delegate void OnLevelStarted(LevelStartedData levelStartedData);
        public OnLevelStarted onLevelStarted;

        protected LevelData activeLevel;
        protected int maxLevelIndex = 0;

        public LevelData ActiveLevel => activeLevel;
        public int MaxLevelIndex => maxLevelIndex;

        private void Awake()
        {
            Init();
        }
        protected virtual void Init()
        {
        
        }

        public async UniTask LoadLevel(int index, bool showLoading, bool isFromMeta)
        {
            // TODO Load save data if any
            LevelData levelData = levelServiceSettings.levels.Find(s => s.index == index);
            if (levelData != null)
            {
                Debug.Log("Level Data Found");
                await sceneService.LoadScene(levelData.sceneReference, true, isFromMeta);

                activeLevel = levelData;
            }
        }

        public async UniTask UnloadLevel()
        {
            // TODO Load save data if any
            LevelData levelData = levelServiceSettings.levels.Find(s => s.index == activeLevel.index);
            await sceneService.UnloadScene(levelData.sceneReference);

            if(activeLevel.sceneReference == levelData.sceneReference)
                activeLevel = levelData;
        }

        public virtual void LevelStarted()
        {
            onLevelStarted.Invoke(new LevelStartedData()
            {
                currentLevelIndex = this.activeLevel.index
            });
        }

        public virtual void OnLevelStatusChanged(LevelStatusData _levelStatusData)
        {
            _levelStatusData.currentLevelIndex = activeLevel.index;
            SaveLevelData(_levelStatusData);
            onLevelStatusChanged?.Invoke(_levelStatusData);
        }

        // TODO Save level data
        protected virtual void SaveLevelData(LevelStatusData _levelStatusData)
        {
        }
    }
}