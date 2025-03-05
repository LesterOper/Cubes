using System;
using Configs;
using Core;
using Input;
using Managers;
using ObjectPools;
using Save;
using Signals;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private CubesConfig cubesConfig;
        [SerializeField] private ScrollController scrollController;
        [SerializeField] private TowerController towerController;
        [SerializeField] private Transform leftScreenSide;
        [SerializeField] private Transform rightScreenSide;
        [SerializeField] private GameConfig gameConfig;
        [Inject] private GameManager _gameManager;
        [Inject] private DragManager _dragManager;
        [Inject] private SignalBus _signalBus;
        [Inject] private SaveLoadManager _saveLoadManager;

        private IGameConfiguration _gameConfiguration;
        
        private void Awake()
        {
            _signalBus.Subscribe<DragEndedSignal>(OnDragEndHandler);
            _signalBus.Subscribe<CubeChosenFromScrollSignal>(OnCubeClickedEventHandler);
        }

        private void Start()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {   
            _gameManager.Initialize();
            transform.localScale = _gameManager.GetScreenScale();
            _dragManager.Initialize(leftScreenSide.position, rightScreenSide.position);
            Configure();
        }

        private void Configure()
        {
            var progress = _saveLoadManager.Load();
            if (progress != null)
            {
                for (int i = 0; i < progress.cubes.Count; i++)
                {
                    var cube = _gameManager.GetCubeFromPool(new CubeData() {cubeColor = progress.cubes[i].cubeColor});
                    cube.transform.position = progress.cubes[i].cubePosition;
                    towerController.AddLoadedCubeInTower(cube);
                }
            }
            _gameConfiguration = GameConfigFactory.GetConfiguration(gameConfig.ConfigurationType, jsonPath: gameConfig.jsonPath, cubesConfig: cubesConfig);
            scrollController.Initialize(_gameConfiguration.GetCubes());
            towerController.Initialize(GameplayConditionsFactory.GetGameplayCondition(gameConfig.GameplayConditionType));
        }

        private void OnCubeClickedEventHandler(CubeChosenFromScrollSignal chosenFromScrollSignal)
        {
            if (!towerController.CanBuildHigher())
            {
                _signalBus.Fire(new LocalizeHintSignal() {term = LocalizationManager.outScreenSize});
                return;
            }
            scrollController.EnableScroll(false);
            var cube = _gameManager.GetCubeFromPool(_gameConfiguration.GetCubes()[chosenFromScrollSignal.index]);
            cube.StartDragging(chosenFromScrollSignal.eventData);
        }

        private void OnDragEndHandler()
        {
            scrollController.EnableScroll(true);
            _dragManager.CheckCubePosition();
        }
    }
}