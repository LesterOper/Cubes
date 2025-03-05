using Core;
using UnityEngine.EventSystems;

namespace Signals
{
    public class CubeRemovedFromTowerSignal
    {
        public PointerEventData EventData;
        public CubeView CubeView;
    }
}