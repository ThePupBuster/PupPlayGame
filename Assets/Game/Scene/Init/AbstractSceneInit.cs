using Game.Mode;
using UnityEngine;

namespace Game.Scene.Init
{
    public abstract class AbstractSceneInit : MonoBehaviour
    {
        public void ReturnToPreviousModeCheckpoint()
        {
            GameModeManager.Instance.PopCheckpoint();
        }
    }
}
