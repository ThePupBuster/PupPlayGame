using Game.MiniGames.Catch.Data.Modifiers;
using System;
using UnityEngine;

namespace Game.MiniGames.Catch.Data
{
    [Serializable]
    public sealed class CatchSpawnableObject
    {
        [SerializeField]
        private bool m_DebugForce = false;

        [SerializeField]
        private int m_MaxOnScreen = -1;

        [SerializeField]
        private int m_SpawnWeight = 0;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float m_RandomChance = 1.0f;

        [SerializeField]
        private int m_MinSpawnScore = -1;

        [SerializeField]
        private int m_MaxSpawnScore = -1;

        [SerializeField]
        private int m_AwardScore = 0;

        [SerializeField]
        private Sprite m_ItemToSpawn = null;

        [SerializeField]
        private AbstractCatchModifier m_CatchModifier = null;

        [SerializeField]
        private GameObject m_ExplosionOnCapture = null;

        public bool CanSpawn(int currentScore, int numExisting)
        {
            if (m_MaxOnScreen > 0 && numExisting >= m_MaxOnScreen)
                return false;

            if (UnityEngine.Random.value > m_RandomChance)
                return false;

            if (MinSpawnScore <= 0 && MaxSpawnScore <= 0)
                return true;

            if (MinSpawnScore <= 0)
                return currentScore <= MaxSpawnScore;

            if (MaxSpawnScore <= 0)
                return currentScore >= MinSpawnScore;

            return currentScore >= MinSpawnScore && currentScore <= MaxSpawnScore;
        }

        public bool DebugForce
        {
            get { return m_DebugForce; }
        }

        public int SpawnWeight
        {
            get { return m_SpawnWeight; }
        }

        public int MinSpawnScore
        {
            get { return m_MinSpawnScore; }
        }

        public int MaxSpawnScore
        {
            get { return m_MaxSpawnScore; }
        }

        public int AwardScore
        {
            get { return m_AwardScore; }
        }

        public Sprite SpriteToDisplay
        {
            get { return m_ItemToSpawn; }
        }

        public AbstractCatchModifier Modifier
        {
            get { return m_CatchModifier; }
        }

        public GameObject ExplosionOnCapture
        {
            get { return m_ExplosionOnCapture; }
        }
    }
}
