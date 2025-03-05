using System.Collections.Generic;

namespace Core
{
    public class OneColorGameplayCondition : IGameplayCondition
    {
        public bool CanPutCubeOnTower(Stack<CubeView> tower, CubeView target)
        {
            if (tower.TryPeek(out CubeView cube))
            {
                return cube.GetColor().Equals(target.GetColor());
            }
            else return true;
        }
    }
}