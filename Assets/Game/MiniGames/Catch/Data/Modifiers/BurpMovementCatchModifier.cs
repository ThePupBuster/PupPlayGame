using UnityEngine;

namespace Game.MiniGames.Catch.Data.Modifiers
{
    [CreateAssetMenu(fileName = "BurpModifierData", menuName = "CatchGame/BurpModifier")]
    public sealed class BurpMovementCatchModifier : AbstractCatchModifier
    {
        private class BurpData
        {
            internal float m_EndTime;
            internal float m_NextStageTime;
            internal bool m_BlockingInput;
            internal GameObject m_BurpAnimation;
        }

        [SerializeField]
        private float m_ActiveTime = 0.0f;

        [SerializeField]
        private float m_DelayTimeMin = 0.0f;

        [SerializeField]
        private float m_DelayTimeMax = 0.0f;

        [SerializeField]
        private float m_DelayFequencyMin = 0.0f;

        [SerializeField]
        private float m_DelayFequencyMax = 0.0f;

        [SerializeField]
        private GameObject m_BurpAnimation = null;

        public override bool IsDone(object data)
        {
            return ((BurpData)data).m_EndTime < Time.timeSinceLevelLoad;
        }

        public override string GetActiveText(object data)
        {
            float timeRemaining = Mathf.Max(((BurpData)data).m_EndTime - Time.timeSinceLevelLoad, 0.0f);

            return timeRemaining.ToString(" 0.00s");
        }

        public override object InitData(Transform playerTransform)
        {
            var burpAnimation = GameObject.Instantiate(m_BurpAnimation);
            burpAnimation.transform.SetParent(playerTransform);
            burpAnimation.transform.localPosition = Vector3.zero;
            burpAnimation.SetActive(false);

            return new BurpData()
            {
                m_EndTime = Time.timeSinceLevelLoad + m_ActiveTime,
                m_NextStageTime = Random.Range(m_DelayTimeMin, m_DelayTimeMax),
                m_BlockingInput = true,
                m_BurpAnimation = burpAnimation
            };
        }

        public override void Destory(object data)
        {
            base.Destory(data);
            GameObject.Destroy(((BurpData)data).m_BurpAnimation);
        }

        public override float UpdateMovement(object data, float moveXDistance)
        {
            var burpData = (BurpData)data;

            float timeSingeLevelLoad = Time.timeSinceLevelLoad;

            if (burpData.m_NextStageTime < timeSingeLevelLoad)
            {
                burpData.m_BlockingInput = !burpData.m_BlockingInput;
                burpData.m_BurpAnimation.SetActive(burpData.m_BlockingInput);

                if (burpData.m_BlockingInput)
                {
                    burpData.m_NextStageTime = timeSingeLevelLoad + Random.Range(m_DelayTimeMin, m_DelayTimeMax);
                }
                else
                {
                    burpData.m_NextStageTime = timeSingeLevelLoad + Random.Range(m_DelayFequencyMin, m_DelayFequencyMax);
                }
            }

            return burpData.m_BlockingInput ? 0.0f : moveXDistance;
        }
    }
}
