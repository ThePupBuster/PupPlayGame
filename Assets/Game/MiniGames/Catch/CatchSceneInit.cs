using Game.MiniGames.Catch.Data;
using Game.Mode;
using Game.Mode.CommonModes;
using Game.Scene.Init;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Game.MiniGames.Catch
{
    public sealed class CatchSceneInit : AbstractMiniGameSceneInit
    {
        public TextMeshProUGUI m_ScoreText = null;
        public TextMeshProUGUI m_TimerText = null;
        public Transform m_ScreenLeftPoint = null;
        public Transform m_ScreenRightPoint = null;
        public Transform m_SpawnHeightPoint = null;
        public Transform m_MinHeightPoint = null;

        public CatchSpawnableObject[] m_ObjectsToSpawn = null;

        public CatchGameFallingItem m_CollectablePrefab = null;
        public Transform m_CollectablePrefabSpawnContainer = null;
        public CatchGamePlayer m_PlayerCharacter = null;
        public float m_InitialSeconds = 30.0f;

        public float m_MinSpawnTimeAtStart = 0.0f;
        public float m_MinSpawnTimeAtEnd = 0.0f;
        public float m_MaxSpawnTimeAtStart = 0.0f;
        public float m_MaxSpawnTimeAtEnd = 0.0f;

        public float m_MinFallSpeedAtStart = 0.0f;
        public float m_MinFallSpeedAtEnd = 0.0f;
        public float m_MaxFallSpeedAtStart = 0.0f;
        public float m_MaxFallSpeedAtEnd = 0.0f;

        public AnimationCurve m_MinSpawnTimeCurve = null;
        public AnimationCurve m_MaxSpawnTimeCurve = null;

        public AnimationCurve m_MinFallSpeedCurve = null;
        public AnimationCurve m_MaxFallSpeedCurve = null;

        [ContextMenu("Sort By Weight")]
        private void SortByWeight()
        {
            m_ObjectsToSpawn = m_ObjectsToSpawn.OrderBy(o => o.SpawnWeight).ToArray();
        }

        protected override void Awake()
        {
            base.Awake();

            SortByWeight();

#if UNITY_EDITOR
            foreach (var obj in m_ObjectsToSpawn)
            {
                if (obj.DebugForce)
                {
                    m_ObjectsToSpawn = new CatchSpawnableObject[] { obj };
                    break;
                }
            }
#endif

            GameModeManager.Instance.PushMode(new DisableDefaultInputMode());
        }

        public override void StartGame()
        {
            base.StartGame();

            GameModeManager.Instance.PushMode(new CatchGameMode(this));
        }

        public override void SetGameUIElementsActive(bool active)
        {
            base.SetGameUIElementsActive(active);
            m_PlayerCharacter.gameObject.SetActive(active);
        }

        public float TimeUntilNextSpawn(float timeRemaining)
        {
            return CalculateValueFromTime(timeRemaining, m_MinSpawnTimeAtStart, m_MinSpawnTimeAtEnd, m_MinSpawnTimeCurve, m_MaxSpawnTimeAtStart, m_MaxSpawnTimeAtEnd, m_MaxSpawnTimeCurve);
        }

        public float GetFallSpeed(float timeRemaining)
        {
            return CalculateValueFromTime(timeRemaining, m_MinFallSpeedAtStart, m_MinFallSpeedAtEnd, m_MinFallSpeedCurve, m_MaxFallSpeedAtStart, m_MaxFallSpeedAtEnd, m_MaxFallSpeedCurve);
        }

        private float CalculateValueFromTime(float timeRemaining, float minAtStart, float minAtEnd, AnimationCurve minCurve, float maxAtStart, float maxAtEnd, AnimationCurve maxCurve)
        {
            float percThroughTime = 1.0f - Mathf.Clamp01(timeRemaining / m_InitialSeconds);

            float minPerc = Mathf.Clamp01(minCurve.Evaluate(percThroughTime));
            float maxPerc = Mathf.Clamp01(maxCurve.Evaluate(percThroughTime));

            float minTime = Mathf.Lerp(minAtStart, minAtEnd, minPerc);
            float maxTime = Mathf.Lerp(maxAtStart, maxAtEnd, maxPerc);

            return UnityEngine.Random.Range(minTime, maxTime);
        }

        public Vector3 GetSpawnPosition()
        {
            return new Vector3(
                Mathf.Lerp(m_ScreenLeftPoint.position.x, m_ScreenRightPoint.position.x, UnityEngine.Random.value),
                m_SpawnHeightPoint.position.y
            );
        }
    }
}
