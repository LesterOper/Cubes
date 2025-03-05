using System.Collections.Generic;

namespace Core
{
    public interface IGameplayCondition
    {
        bool CanPutCubeOnTower(Stack<CubeView> tower, CubeView target);
    }
}