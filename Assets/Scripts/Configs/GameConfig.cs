using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = nameof(GameConfig), menuName = "Configs/" + nameof(GameConfig))]
    public class GameConfig : ScriptableObject
    {
        public ConfigurationType ConfigurationType;
        public string jsonPath;
        public GameplayConditionType GameplayConditionType;
    }

    public enum GameplayConditionType
    {
        Free,
        OneColor,
    }
}