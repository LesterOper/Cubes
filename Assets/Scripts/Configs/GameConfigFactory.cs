using UnityEngine;

namespace Configs
{
    public class GameConfigFactory
    {
        public static IGameConfiguration GetConfiguration(ConfigurationType configurationType, string jsonPath = null,
            CubesConfig cubesConfig = null)
        {
            switch (configurationType)
            {
                case ConfigurationType.SO:
                {
                    return cubesConfig;
                }
                case ConfigurationType.JSON:
                {
                    JsonConfiguration json = new JsonConfiguration();
                    json.ReadJson(jsonPath);
                    return json;
                }
                default:
                {
                    throw new System.ArgumentException("Unknown source of configuration");
                }
            }
        }
    }
    
    public enum ConfigurationType
    {
        SO,
        JSON
    }
}