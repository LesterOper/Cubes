using System;
using UnityEngine;
using Zenject;

namespace Configs
{
    [CreateAssetMenu(fileName = nameof(CubesConfig), menuName = "Configs/" + nameof(CubesConfig))]
    public class CubesConfig : ScriptableObject, IGameConfiguration
    {
        [SerializeField] private CubeData[] _cubes;
        public CubeData[] GetCubes() => _cubes;
    }

    [Serializable]
    public class CubeData
    {
        public Color cubeColor = Color.white;
    }
}
