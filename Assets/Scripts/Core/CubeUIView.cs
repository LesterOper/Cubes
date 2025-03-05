using System;
using Input;
using Signals;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Core
{
    public class CubeUIView : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image image;
        private int _index;

        [Inject] private SignalBus _signalBus;

        public void Initialize(Color color, int cubeIndex)
        {
            _index = cubeIndex;
            image.color = new Color(color.r, color.g, color.b, 1);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _signalBus.Fire(new CubeChosenFromScrollSignal(){index = _index, eventData = eventData});
        }
    }
}