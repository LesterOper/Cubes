using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Input
{
    public class InputSystem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler
    {
        public static event Action<PointerEventData, GameObject> OnPointerDownEvent;
        public static event Action<PointerEventData, GameObject> OnPointerUpEvent;
        public static event Action<PointerEventData> OnDragEvent;

        public void OnPointerDown(PointerEventData eventData)
        {
            var hitObject = GetWorldObjectUnderPointer(eventData) ?? GetUIObjectUnderPointer(eventData);
            OnPointerDownEvent?.Invoke(eventData, hitObject);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            var hitObject = GetWorldObjectUnderPointer(eventData) ?? GetUIObjectUnderPointer(eventData);
            OnPointerUpEvent?.Invoke(eventData, hitObject);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDragEvent?.Invoke(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //OnDragEvent?.Invoke(eventData);
        }

        private GameObject GetUIObjectUnderPointer(PointerEventData eventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0 ? results[0].gameObject : null;
        }
        
        private GameObject GetWorldObjectUnderPointer(PointerEventData eventData)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            return hit.collider != null ? hit.collider.gameObject : null;
        }
    }
}