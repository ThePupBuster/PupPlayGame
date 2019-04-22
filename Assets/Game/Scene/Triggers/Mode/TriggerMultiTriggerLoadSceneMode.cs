using Game.Utils;
using UnityEngine;

namespace Game.Scene.Triggers.Mode
{
    public sealed class TriggerMultiTriggerLoadSceneMode : MonoBehaviour
    {
        [SerializeField]
        private MultiTriggerLoadSceneMode m_TargetTrigger = null;

        private bool m_AddedTrigger = false;

        private void OnDestroy()
        {
            if (m_TargetTrigger && m_AddedTrigger)
            {
                m_TargetTrigger.RemoveTrigger(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!m_AddedTrigger && collision.CompareTag(CollisionTags.PLAYER))
            {
                m_AddedTrigger = true;
                m_TargetTrigger.Trigger(this);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (m_AddedTrigger && collision.CompareTag(CollisionTags.PLAYER))
            {
                m_AddedTrigger = false;
                m_TargetTrigger.RemoveTrigger(this);
            }
        }
    }
}
