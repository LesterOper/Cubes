using Core;
using Zenject;

namespace ObjectPools
{
    public class CubesPool : MonoMemoryPool<CubeView>
    {
        protected override void OnCreated(CubeView item)
        {
            base.OnCreated(item);
        }

        protected override void OnDespawned(CubeView item)
        {
            base.OnDespawned(item);
        }
    }
}