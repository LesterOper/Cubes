using System;
using Configs;
using Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Controllers
{
    public class ScrollController : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private CubeUIView cubePrefab;
        [SerializeField] private Transform cubesParent;
        [Inject] private DiContainer _diContainer;
        
        public void Initialize(CubeData[] cubeDatas)
        {
            for (int i = 0; i < cubeDatas.Length; i++)
            {
                var cubeUI = _diContainer.InstantiatePrefabForComponent<CubeUIView>(cubePrefab, cubesParent);
                cubeUI.Initialize(cubeDatas[i].cubeColor, i);
            }
        }

        public void EnableScroll(bool enable)
        {
            scrollRect.enabled = enable;
        }
    }
}