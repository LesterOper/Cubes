using Configs;
using UnityEngine;

namespace Core
{
    public class GameplayConditionsFactory
    {
        public static IGameplayCondition GetGameplayCondition(GameplayConditionType gameplayConditionType)
        {
            switch (gameplayConditionType)
            {
                case GameplayConditionType.Free:
                {
                    return new FreeGameplayCondition();
                }
                case GameplayConditionType.OneColor:
                {
                    return new OneColorGameplayCondition();
                }
                default:
                {
                    Debug.Log("Unknown gameplay condition");
                    return new FreeGameplayCondition();
                }
            }
        }
    }
}