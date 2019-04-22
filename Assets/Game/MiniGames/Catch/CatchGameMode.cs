using Game.Mode;
using Game.Core;
using UnityEngine;
using System.Collections.Generic;
using Game.MiniGames.Catch.Data;
using Game.Utils;

namespace Game.MiniGames.Catch
{
    public sealed class CatchGameMode : AbstractGameMode
    {
        private const int INITIAL_POOL_SIZE = 5;

        private readonly CatchSceneInit m_SceneInitData;
        private readonly TrackedObjectPool<CatchGameFallingItem> m_CollectablePool;

        private float m_UnmodifiedTimeRemaining;
        private float m_TimeRemaining;
        private float m_TimeUntilNextSpawn;
        private int m_CurrentScore = 0;
        private readonly Dictionary<CatchSpawnableObject, int> m_NumExistingItems;
        private readonly List<CatchSpawnableObject> m_SpawnableObjects;

        public CatchGameMode(CatchSceneInit sceneInitData)
        {
            m_SceneInitData = sceneInitData;
            m_CollectablePool = new TrackedObjectPool<CatchGameFallingItem>(PopulatePoolCallback, INITIAL_POOL_SIZE);
            m_SpawnableObjects = new List<CatchSpawnableObject>(m_SceneInitData.m_ObjectsToSpawn.Length);
            m_NumExistingItems = new Dictionary<CatchSpawnableObject, int>(m_SceneInitData.m_ObjectsToSpawn.Length);

            m_UnmodifiedTimeRemaining = m_TimeRemaining = m_SceneInitData.m_InitialSeconds;
            m_TimeUntilNextSpawn = 1.0f;

            m_SceneInitData.m_PlayerCharacter.OnClaimedItem += OnClaimedItem;
        }

        private CatchGameFallingItem PopulatePoolCallback(Pool<CatchGameFallingItem> pool)
        {
            GameObject newItem = GameObject.Instantiate(m_SceneInitData.m_CollectablePrefab.gameObject);
            newItem.SetActive(false);

            newItem.transform.parent = m_SceneInitData.m_CollectablePrefabSpawnContainer;

            var cmp = newItem.GetComponent<CatchGameFallingItem>();
            cmp.SetPool(pool);

            return cmp;
        }

        public override void Destroy()
        {
            base.Destroy();

            m_CollectablePool.DestroyPool(MonoBehaviourUtils.DestoryBehaviourGameObject);

            m_SceneInitData.m_PlayerCharacter.OnClaimedItem -= OnClaimedItem;
        }

        private void UpdateTimeRemaining()
        {
            m_TimeRemaining = Mathf.Max(0.0f, m_TimeRemaining - Time.deltaTime);
            m_UnmodifiedTimeRemaining = Mathf.Max(0.0f, m_UnmodifiedTimeRemaining - Time.deltaTime);
            UpdateTimeRemainingText();

            if (m_TimeRemaining <= 0.0f)
            {
                m_SceneInitData.ShowEndGameDialog(m_CurrentScore);

                GameModeManager.Instance.PopMode();
            }
        }

        private void UpdateTimeRemainingText()
        {
            m_SceneInitData.m_TimerText.text = m_TimeRemaining.ToString("0.00");
        }

        private CatchSpawnableObject PickSpawnedItem()
        {
            m_NumExistingItems.Clear();

            foreach (var activeItem in m_CollectablePool.ActiveItems)
            {
                m_NumExistingItems[activeItem.ItemData] = m_NumExistingItems.GetOrElse(activeItem.ItemData, 0) + 1;
            }

            int totalWeight = 0;
            m_SpawnableObjects.Clear();
            foreach (var spawnCandidate in m_SceneInitData.m_ObjectsToSpawn)
            {
                if (spawnCandidate.CanSpawn(m_CurrentScore, m_NumExistingItems.GetOrElse(spawnCandidate, 0)))
                {
                    totalWeight += spawnCandidate.SpawnWeight;
                    m_SpawnableObjects.Add(spawnCandidate);
                }
            }

            int randomWeight = Random.Range(0, totalWeight);
            foreach (var spawnCandidate in m_SpawnableObjects)
            {
                if (spawnCandidate.CanSpawn(m_CurrentScore, m_NumExistingItems.GetOrElse(spawnCandidate, 0)))
                {
                    randomWeight -= spawnCandidate.SpawnWeight;
                    if (randomWeight <= 0)
                    {
                        return spawnCandidate;
                    }
                }
            }

            // failed to find something 
            return null;
        }

        private void UpdateSpawns()
        {
            m_TimeUntilNextSpawn -= Time.deltaTime;
            if (m_TimeUntilNextSpawn <= 0.0f)
            {
                var spawnData = PickSpawnedItem();
                if (spawnData != null)
                {
                    var spawnPos = m_SceneInitData.GetSpawnPosition();
                    float fallSpeed = m_SceneInitData.GetFallSpeed(m_UnmodifiedTimeRemaining);
                    m_CollectablePool.New().Init(spawnData, fallSpeed, spawnPos, m_SceneInitData.m_MinHeightPoint.position.y);
                }

                m_TimeUntilNextSpawn = m_SceneInitData.TimeUntilNextSpawn(m_UnmodifiedTimeRemaining);
            }
        }

        public override void Update()
        {
            base.Update();

            UpdateTimeRemaining();
            UpdateSpawns();
        }

        private void OnClaimedItem(CatchSpawnableObject caughtObject)
        {
            if (caughtObject.AwardScore > 0)
            {
                m_CurrentScore += caughtObject.AwardScore;
                m_SceneInitData.m_ScoreText.text = StringUtils.CommaSeperateNumber(m_CurrentScore);
            }

            if (caughtObject.Modifier != null)
            {
                m_TimeRemaining = caughtObject.Modifier.ModifyTime(m_TimeRemaining);
                UpdateTimeRemainingText();
            }
        }
    }
}
