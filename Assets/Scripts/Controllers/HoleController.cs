using System;
using Core;
using DG.Tweening;
using Signals;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class HoleController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer holeRenderer;
        [SerializeField] private Transform maskTr;
        [SerializeField] private ParticleSystem throwEffect;
        
        [Inject] private SignalBus _signalBus;

        private void Awake()
        {
            _signalBus.Subscribe<DropCubeSignal>(DropCube);
        }

        private void DropCube(DropCubeSignal signal)
        {
            Debug.Log(holeRenderer.transform.position.x);
            float holeLeftArea = holeRenderer.transform.position.x - (holeRenderer.bounds.size.x/2 * 0.8f);
            Debug.Log(holeLeftArea);
            float holeRightArea = holeRenderer.transform.position.x + (holeRenderer.bounds.size.x/2 * 0.8f);
            Debug.Log(holeRightArea);
            float xCubePosition = signal.CubeView.transform.position.x;

            if (holeLeftArea < xCubePosition && holeRightArea > xCubePosition)
            {
                signal.CubeView.transform.DOMoveY(maskTr.position.y, 0.3f).OnComplete(() =>
                {
                    _signalBus.Fire(new CubeReturnPoolSignal() {CubeView = signal.CubeView});
                    _signalBus.Fire(new LocalizeHintSignal(){term = LocalizationManagerDebugConstantsKey.thrownCube});
                    throwEffect.Play();
                });
                
            }
            else signal.CubeView.DestroyCubeAnimation(0);
        }
    }
}