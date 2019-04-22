using UnityEngine;

namespace Game.MiniGames.Catch.Data.Modifiers
{
    [CreateAssetMenu(fileName = "AddTimeModifierData", menuName = "CatchGame/AddTimeModifier")]
    public sealed class AddTimeCatchModifier : AbstractCatchModifier
    {
        private class TimeData
        {
            public float m_ActiveTimeout;
            public string m_DisplayText;
        }

        [SerializeField]
        private float m_UIDisplayTime = 3.0f;

        [SerializeField]
        private float m_SecondsToAdd = 0.0f;

        public override object InitData(Transform playerTransform)
        {
            return new TimeData()
            {
                m_ActiveTimeout = m_UIDisplayTime + Time.timeSinceLevelLoad,
                m_DisplayText = string.Format("+{0} Seconds", (int)m_SecondsToAdd)
            };
        }

        public override string GetActiveText(object data)
        {
            return ((TimeData)data).m_DisplayText;
        }

        public override bool IsDone(object data)
        {
            return ((TimeData)data).m_ActiveTimeout < Time.timeSinceLevelLoad;
        }

        public override float ModifyTime(float timeRemaining)
        {
            return timeRemaining + m_SecondsToAdd;
        }
    }
}
