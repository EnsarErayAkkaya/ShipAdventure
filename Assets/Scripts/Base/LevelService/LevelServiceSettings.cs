using DevLocker.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Services
{
    [CreateAssetMenu(fileName = "LevelsData", menuName = "Scriptable Objects/LevelsData")]
    public class LevelServiceSettings: ScriptableObject
    {
        public List<LevelData> levels = new List<LevelData>();

    }
    [Serializable]
    public class LevelData
    {
        public SceneReference sceneReference;
        public GameObject levelPrefab;
        public int index;
    }
}