using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core;
using UnityEngine;
using Zenject;

namespace Save
{
    public class SaveLoadManager : IInitializable
    {
        private string saveFile = "save.json";
        private string savePath;
        private GameProgress _gameProgress;

        public void Initialize()
        {
            savePath = Path.Combine(Application.persistentDataPath, saveFile);
        }

        public void Save(Stack<CubeView> tower)
        {
            _gameProgress = new GameProgress();
            _gameProgress.cubes = new List<CubeProgressSaveData>();
            List<CubeView> towerList = tower.ToList();
            towerList.Reverse();
            for (int i = 0; i < towerList.Count; i++)
            {
                CubeProgressSaveData saveData = new CubeProgressSaveData();
                saveData.cubeColor = towerList[i].GetColor();
                saveData.cubePosition = towerList[i].transform.position;
                _gameProgress.cubes.Add(saveData);
            }

            string json = JsonUtility.ToJson(_gameProgress, true);
            File.WriteAllText(savePath, json);
        }

        public GameProgress Load()
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                GameProgress progress = JsonUtility.FromJson<GameProgress>(json);
                Debug.Log("Progress loaded!");
                return progress;
            }
            else
            {
                Debug.LogWarning("No save file found, starting fresh.");
                return null;
            }
        }
    }

    [Serializable]
    public class GameProgress
    {
        public List<CubeProgressSaveData> cubes;
    }

    [Serializable]
    public class CubeProgressSaveData
    {
        public Color cubeColor;
        public Vector3 cubePosition;
    }
}