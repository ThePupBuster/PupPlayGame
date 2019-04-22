using Game.Core;
using Game.MiniGames.Catch.Data;
using System;
using UnityEngine;

namespace Game.MiniGames.Catch
{
    public sealed class CatchGameFallingItem : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer m_SpriteRenderer = null;

        [SerializeField]
        private BoxCollider2D m_BoxCollider = null;

        private Pool<CatchGameFallingItem> m_Pool = null;
        private Transform m_Transform = null;
        private float m_Speed = 0f;
        private float m_MinY = 0f;

        public CatchSpawnableObject ItemData { get; private set; } = null;

        public void SetPool(Pool<CatchGameFallingItem> pool)
        {
            m_Pool = pool;
        }

        public void Init(CatchSpawnableObject itemData, float speed, Vector3 pos, float minY)
        {
            ItemData = itemData;

            transform.position = pos;
            m_SpriteRenderer.sprite = ItemData.SpriteToDisplay;
            m_Speed = speed;
            m_MinY = minY;

            m_BoxCollider.size = m_SpriteRenderer.sprite.bounds.size;

            gameObject.SetActive(true);
        }

        private void Awake()
        {
            m_Transform = transform;
        }

        private void Update()
        {
            m_Transform.position -= new Vector3(0f, m_Speed * Time.deltaTime, 0f);

            if (m_Transform.position.y < m_MinY)
            {
                Remove();
            }
        }

        public void Remove()
        {
            gameObject.SetActive(false);
            m_Pool.Free(this);
        }
    }
}
