using System.Collections.Generic;

namespace Core
{
    public class FreeGameplayCondition : IGameplayCondition
    {
        public bool CanPutCubeOnTower(Stack<CubeView> tower, CubeView target)
        {
            return true;
        }
    }
}