using System;
using Save;
using Signals;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class GlobalManager : MonoBehaviour
    {
        private static GlobalManager _instance;

        public static GlobalManager Instance => _instance;
        [Inject] private SignalBus _signalBus;
        

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else DestroyImmediate(gameObject);
            
            DontDestroyOnLoad(this);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus == false)
            {
                _signalBus.Fire(new SaveSignal());
            }
        }
    }
}