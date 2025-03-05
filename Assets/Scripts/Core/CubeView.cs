using System;
using Configs;
using DG.Tweening;
using Input;
using Managers;
using Signals;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Core
{
    public class CubeView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer cubeRendererComponent;
        [SerializeField] private DragCube dragCube;
        [SerializeField] private AnimationCurve setOnTowerCurve;
        [SerializeField] private float setOnTowerAnimationDuration;
        [SerializeField] private ParticleSystem despawnEffect;
        [SerializeField] private ParticleSystem setTowerEffect;

        [Inject] private DragManager _dragManager;
        [Inject] private SignalBus _signalBus;
        private CubeData _cubeData;

        public Color GetColor() => _cubeData.cubeColor;

        private void Awake()
        {
            dragCube.OnDragStartFromTower += FireDragManagerCubeSetSignal;
        }

        private void FireDragManagerCubeSetSignal(PointerEventData eventData)
        {
            _signalBus.Fire(new CubeRemovedFromTowerSignal(){CubeView = this, EventData = eventData});
        }

        public void Initialize(CubeData cubeData)
        {
            _cubeData = cubeData;
            cubeRendererComponent.color = _cubeData.cubeColor;
        }

        public void StartDragging(PointerEventData eventData)
        {
            dragCube.StartDragging(eventData);
            _dragManager.SetDraggableCubeView(this);
        }

        public void HideCube()
        {
            gameObject.SetActive(false);
        }
        
        public void ShowCube()
        {
            gameObject.SetActive(true);
        }

        public Tween SetOnTowerAnimation(float yPosition)
        {
            return transform.DOMoveY(yPosition, setOnTowerAnimationDuration).SetEase(setOnTowerCurve).OnComplete(() =>
            {
                setTowerEffect.Play();
            });
        }

        public void DestroyCubeAnimation(float yPosition)
        {
            cubeRendererComponent.DOFade(0,0.2f).OnComplete(ReturnToPool);
            despawnEffect.Play();
        }

        private void ReturnToPool()
        {
            _signalBus.Fire(new CubeReturnPoolSignal() {CubeView = this});
        }

        public float GetUpperCubePosition()
        {
            var yPosition = transform.position.y;
            yPosition += cubeRendererComponent.size.y;
            return yPosition;
        }

        public (float leftPositionLimit, float rightPositionLimit) GetLeftRightLimits()
        {
            var xPosition = transform.position.x;
            var xLeftPositionLimit = xPosition - cubeRendererComponent.size.x / 2f;
            var xRightPositionLimit = xPosition + cubeRendererComponent.size.x / 2f;
            return (xLeftPositionLimit, xRightPositionLimit);
        }

        public float GetYSize() => cubeRendererComponent.size.y;
        public float GetWidth() => cubeRendererComponent.bounds.size.x;
    }
}