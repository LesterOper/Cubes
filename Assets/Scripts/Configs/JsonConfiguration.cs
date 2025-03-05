using System;
using System.IO;
using UnityEngine;

namespace Configs
{
    public class JsonConfiguration : IGameConfiguration
    {
        private GameJsonConfig _gameConfig;

        public void ReadJson(string fileName)
        {
            TextAsset json = Resources.Load<TextAsset>(fileName);
            if(json != null)
            {
                _gameConfig = JsonUtility.FromJson<GameJsonConfig>(json.text);
            }
            else
            {
                Debug.LogError("Конфигурационный файл не найден!");
                _gameConfig = new GameJsonConfig(); // Фолбэк-значения
            }
        }
        public CubeData[] GetCubes() => _gameConfig.cubes;
    }

    [Serializable]
    public struct GameJsonConfig
    {
        public CubeData[] cubes;
    }
}