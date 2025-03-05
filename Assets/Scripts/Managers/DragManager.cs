using System;
using Core;
using Signals;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class DragManager
    {
        private Vector3 _leftSidePosition;
        private Vector3 _rightSidePosition;
        private CubeView _cubeView;
        [Inject] private SignalBus _signalBus;

        public void Initialize(Vector3 leftSidePosition, Vector3 rightSidePosition)
        {
            _leftSidePosition = leftSidePosition;
            _rightSidePosition = rightSidePosition;
            _signalBus.Subscribe<DragManagerCubeSetSignal>(SetDraggableCubeView);
        }

        public void SetDraggableCubeView(DragManagerCubeSetSignal signal)
        {
            _cubeView = signal.CubeView;
        }
        
        public void SetDraggableCubeView(CubeView cubeView)
        {
            _cubeView = cubeView;
        }

        public void CheckCubePosition()
        {
            if (_cubeView == null)
            {
                Debug.Log("Cube is null");
                return;
            }
            var position = _cubeView.transform.position;
            float leftDistance = Vector3.Distance(position, _leftSidePosition);
            float rightDistance = Vector3.Distance(position, _rightSidePosition);
            if (leftDistance < rightDistance)
            {
                _signalBus.Fire(new DropCubeSignal(){CubeView = _cubeView});
            }
            else
            {
                _signalBus.Fire(new PushCubeOnTowerSignal(){CubeView = _cubeView});
            }

            _cubeView = null;
        }
    }
}