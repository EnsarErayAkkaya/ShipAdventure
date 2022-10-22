using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
namespace EEA.Services
{
    public class SaveService : MonoBehaviour
    {
        public SaveData saveData = new SaveData();

        public const string saveFile = "/gamesave.save";

        public void SaveGame()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + saveFile);
            bf.Serialize(file, saveData);
            file.Close();

            Debug.Log("Game Saved");
        }
        public void LoadGame()
        {
            if (File.Exists(Application.persistentDataPath + saveFile))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + saveFile, FileMode.Open);
                saveData = (SaveData)bf.Deserialize(file);
                file.Close();
            }
            else
            {
                saveData = new SaveData();
                Debug.Log("No game saved!");
            }
        }
    }
}