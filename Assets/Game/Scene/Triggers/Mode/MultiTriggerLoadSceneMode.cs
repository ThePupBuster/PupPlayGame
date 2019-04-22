using UnityEngine;
using System.Collections.Generic;

namespace Game.Scene.Triggers.Mode
{
    public sealed class MultiTriggerLoadSceneMode : TriggerShowLoadSceneButton
    {
        private readonly List<MonoBehaviour> m_TriggeredBy = new List<MonoBehaviour>();

        public void Trigger(MonoBehaviour triggeredByObject)
        {
            m_TriggeredBy.Add(triggeredByObject);

            if (m_TriggeredBy.Count == 1)
            {
                SetButtonEnabled(true);
            }
        }

        public void RemoveTrigger(MonoBehaviour triggeredByObject)
        {
            m_TriggeredBy.Remove(triggeredByObject);

            if (m_TriggeredBy.Count == 0)
            {
                SetButtonEnabled(false);
            }
        }
    }
}
