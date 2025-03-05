using System;
using Configs;
using Core;
using ObjectPools;
using Signals;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class GameManager
    {
        [Inject] private CubesPool _cubesPool;
        [Inject] private SignalBus _signalBus;

        public void Initialize()
        {
            _signalBus.Subscribe<CubeReturnPoolSignal>(ReturnToPool);
        }
        
        public Vector3 GetScreenScale()
        {
            float orthoSize = Camera.main.orthographicSize;
            float screenWidth = orthoSize * Camera.main.aspect;

            return new Vector3(screenWidth, orthoSize * 2, 1);
        }

        public CubeView GetCubeFromPool(CubeData cubeData)
        {
            var cube = _cubesPool.Spawn();
            var position = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            position.z = 0;
            cube.transform.position = position;
            cube.Initialize(cubeData);
            cube.ShowCube();
            return cube;
        }

        private void ReturnToPool(CubeReturnPoolSignal signal)
        {
            _cubesPool.Despawn(signal.CubeView);
        }
    }
}