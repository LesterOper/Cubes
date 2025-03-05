using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using DG.Tweening;
using Save;
using Signals;
using Unity.Collections;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class TowerController : MonoBehaviour
    {
        private Stack<CubeView> _tower;
        [Inject] private SignalBus _signalBus;
        [Inject] private SaveLoadManager _saveLoadManager;

        private IGameplayCondition _gameplayCondition; 
        public float _topPositionY;
        public float _topScreenY;
        public float _yBasePosition;
        public float _yCubeSize;

        private void Awake()
        {
            _tower = new Stack<CubeView>();
            _signalBus.Subscribe<PushCubeOnTowerSignal>(TryAddCubeToTower);
            _signalBus.Subscribe<CubeRemovedFromTowerSignal>(TryRemoveCubeFromTower);
            _signalBus.Subscribe<SaveSignal>(Save);
            _topScreenY = Camera.main.transform.position.y + Camera.main.orthographicSize;
        }

        public void Initialize(IGameplayCondition gameplayCondition)
        {
            _gameplayCondition = gameplayCondition;
        }

        public void AddLoadedCubeInTower(CubeView cubeView)
        {
            _tower.Push(cubeView);
            _topPositionY = _yBasePosition + _yCubeSize * _tower.Count;
        }

        private void Save()
        {
            _saveLoadManager.Save(_tower);
        }

        private void TryAddCubeToTower(PushCubeOnTowerSignal signal)
        {
            var cube = signal.CubeView;
            CubeView upperCube;
            if (_tower.Count == 0)
            {
                _yBasePosition = signal.CubeView.transform.position.y;
                _yCubeSize = signal.CubeView.GetYSize();
            }

            if (_gameplayCondition.CanPutCubeOnTower(_tower, cube) == false)
            {
                DespawnCube(cube);
                return;
            }
            
            if (_tower.TryPeek(out upperCube))
            {
                var upperPositionY = upperCube.GetUpperCubePosition();
                float differenceXPosition = Math.Abs(upperCube.transform.position.x - cube.transform.position.x);
                float width = cube.GetWidth() / 2f;
                bool correctPlaceY = cube.transform.position.y > upperPositionY;
                bool correctPlaceX = differenceXPosition < width;
                

                bool correctPlace = correctPlaceX && correctPlaceY;
                if (correctPlace)
                {
                    cube.SetOnTowerAnimation(upperPositionY);
                    
                    _tower.Push(cube);
                    _signalBus.Fire(new LocalizeHintSignal() {term = LocalizationManager.putCube});
                }
                else
                {
                    DespawnCube(cube);
                }
            }
            else
            {
                _tower.Push(cube);
                _signalBus.Fire(new LocalizeHintSignal() {term = LocalizationManager.putCube});
            }
            
            _topPositionY = _yBasePosition + _yCubeSize * _tower.Count;
        }

        private void DespawnCube(CubeView cubeView)
        {
            cubeView.DestroyCubeAnimation(0);
            _signalBus.Fire(new LocalizeHintSignal() {term = LocalizationManager.dropCube});
        }

        private void TryRemoveCubeFromTower(CubeRemovedFromTowerSignal signal)
        {
            if (_tower.Contains(signal.CubeView))
            {
                var bufCollection = _tower.ToList();
                bufCollection.Reverse();
                
                int index = bufCollection.IndexOf(signal.CubeView);
                for (int i = index; i < bufCollection.Count-1; i++)
                {
                    var cubeTargetPosition = _yBasePosition + _yCubeSize*i;
                    var cubeToMove = bufCollection[i + 1];
                    
                    cubeToMove.SetOnTowerAnimation(cubeTargetPosition);
                }
                
                if (bufCollection.Remove(signal.CubeView))
                {
                    Debug.Log("Deleted cube from tower");
                    _tower.Clear();
                    for (int i = 0; i < bufCollection.Count; i++)
                    {
                        _tower.Push(bufCollection[i]);
                    }
                    
                    signal.CubeView.StartDragging(signal.EventData);
                    _signalBus.Fire(new LocalizeHintSignal() {term = LocalizationManager.dropCube});
                    UpdateTower();
                }
                else Debug.Log("Cant delete cube from tower");
                
            }
            else Debug.Log("Cube doesnt exist in tower");
        }

        private void UpdateTower()
        {
            var bufCollection = _tower.ToList();
            bufCollection.Reverse();

            for (int i = 1; i < bufCollection.Count; i++)
            {
                var upperCube = bufCollection[i];
                var belowCube = bufCollection[i - 1];

                float differenceXPosition = Math.Abs(upperCube.transform.position.x - belowCube.transform.position.x);
                float width = belowCube.GetWidth() / 2f;

                if (differenceXPosition > width)
                {
                    bufCollection.RemoveAt(i);
                    upperCube.DestroyCubeAnimation(0);
                    i--;
                }
            }

            for (int i = 1; i < bufCollection.Count; i++)
            {
                var cubeTargetPosition = _yBasePosition + _yCubeSize*i;
                bufCollection[i].SetOnTowerAnimation(cubeTargetPosition);
            }

            _tower = new Stack<CubeView>(bufCollection);
            _topPositionY = _yBasePosition + _yCubeSize * _tower.Count;
        }

        public bool CanBuildHigher()
        {
            return _topPositionY + _yCubeSize <= _topScreenY;
        }
    }
}