using Core;
using Managers;
using ObjectPools;
using Save;
using Signals;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private CubeView cubePrefab;
        [SerializeField] private Transform poolParentTransform;
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.BindMemoryPool<CubeView, CubesPool>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(cubePrefab)
                .UnderTransform(poolParentTransform);
            Container.Bind<GameManager>().FromNew().AsSingle();
            Container.Bind<DragManager>().FromNew().AsSingle();
            Container.Bind<LocalizationManager>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveLoadManager>().FromNew().AsSingle();


            Container.DeclareSignal<DragEndedSignal>();
            Container.DeclareSignal<PushCubeOnTowerSignal>();
            Container.DeclareSignal<DropCubeSignal>();
            Container.DeclareSignal<CubeChosenFromScrollSignal>();
            Container.DeclareSignal<DragManagerCubeSetSignal>();
            Container.DeclareSignal<CubeReturnPoolSignal>();
            Container.DeclareSignal<CubeRemovedFromTowerSignal>();
            Container.DeclareSignal<LocalizeHintSignal>();
            Container.DeclareSignal<SaveSignal>();
        }
    }
}
