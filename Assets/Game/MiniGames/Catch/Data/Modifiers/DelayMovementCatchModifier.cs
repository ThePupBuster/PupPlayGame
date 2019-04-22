using UnityEngine;

namespace Game.MiniGames.Catch.Data.Modifiers
{
    [CreateAssetMenu(fileName = "DelayModifierData", menuName = "CatchGame/DelayModifier")]
    public sealed class DelayMovementCatchModifier : AbstractCatchModifier
    {
        private sealed class DelayData
        {
            internal float m_EndTime;
            internal GameObject m_Animation;
        }

        [SerializeField]
        private float m_DelayTime = 0.0f;

        [SerializeField]
        private GameObject m_DelayedAnimation = null;

        public override object InitData(Transform playerTransform)
        {
            var animation = GameObject.Instantiate(m_DelayedAnimation);
            animation.transform.SetParent(playerTransform);
            animation.transform.localPosition = Vector3.zero;

            return new DelayData()
            {
                m_EndTime = Time.timeSinceLevelLoad + m_DelayTime,
                m_Animation = animation
            };
        }

        public override void Destory(object data)
        {
            base.Destory(data);

            GameObject.Destroy(((DelayData)data).m_Animation);
        }

        public override bool IsDone(object data)
        {
            return ((DelayData)data).m_EndTime < Time.timeSinceLevelLoad;
        }

        public override string GetActiveText(object data)
        {
            float timeRemaining = ((DelayData)data).m_EndTime - Time.timeSinceLevelLoad;

            return timeRemaining.ToString(" 0.00s");
        }

        public override float UpdateMovement(object data, float moveXDistance)
        {
            return 0.0f;
        }
    }
}
