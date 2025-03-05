using System;
using Controllers;
using Core;
using Managers;
using Signals;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Input
{
    public class DragCube : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler
    {
        private bool _isDragging;
        private Vector3 _offset;

        public event Action<PointerEventData> OnDragStartFromTower;

        [Inject] private SignalBus _signalBus;

        public void StartDragging(PointerEventData pointerEventData)
        {
            _isDragging = true;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pointerEventData.position);
            worldPosition.z = 0;
            _offset = transform.position - worldPosition;
        }

        private void Update()
        {
            if(_isDragging == false) return;

            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
                _signalBus.Fire(new DragEndedSignal());
                return;
            }
            
            var position = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            position.z = 0;
            transform.position = position + _offset;
        }


        public void OnPointerDown(PointerEventData eventData)
        { }

        public void OnDrag(PointerEventData eventData)
        { }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(_isDragging) return;
            StartDragging(eventData);
            OnDragStartFromTower?.Invoke(eventData);
        }
    }
}