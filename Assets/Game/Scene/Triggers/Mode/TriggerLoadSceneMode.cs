using Game.Mode;
using Game.Mode.CommonModes;
using Game.Utils;
using UnityEngine;

namespace Game.Scene.Triggers.Mode
{
    public sealed class TriggerLoadSceneMode : MonoBehaviour
    {
        private const float MIN_WAIT_TIME = 1f;

        [SerializeField]
        private string m_SceneName = string.Empty;

        private float m_MinTriggerWait = 0f;

        private void OnEnable()
        {
            m_MinTriggerWait = Time.timeSinceLevelLoad + MIN_WAIT_TIME;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Time.timeSinceLevelLoad <= m_MinTriggerWait)
                return;

            if (collision.CompareTag(CollisionTags.PLAYER))
            {
                GameModeManager.Instance.PushCheckpoint();
                GameModeManager.Instance.PushMode(new LoadSceneAdditiveMode(m_SceneName, disableCamera:true));
            }
        }
    }
}
