using System.Collections;
using UnityEngine;

namespace Game.Scene.Common
{
    public sealed class BasicAnimatedSprite : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer m_Renderer = null;

        [SerializeField]
        private Sprite[] m_Frames = null;

        [SerializeField]
        private float m_DelayBetweenFrames = 0.5f;

        [SerializeField]
        private bool m_RandomStartFrame = true;

        private int m_CurrentFrame = 0;
        private float m_NextUpdateTime = 0.0f;

        private void Awake()
        {
            if (m_RandomStartFrame)
            {
                m_CurrentFrame = Random.Range(0, m_Frames.Length);
            }
        }

        private void Update()
        {
            if (m_NextUpdateTime < Time.timeSinceLevelLoad)
            {
                m_NextUpdateTime = Time.timeSinceLevelLoad + m_DelayBetweenFrames;

                if (m_CurrentFrame >= m_Frames.Length)
                {
                    m_CurrentFrame = 0;
                }

                var frame = m_Frames[m_CurrentFrame];
                if (frame)
                {
                    m_Renderer.sprite = frame;
                }

                ++m_CurrentFrame;
            }
        }

        private IEnumerator PlayAnimation()
        {
            var waitCommand = new WaitForSeconds(m_DelayBetweenFrames);
            int currentFrame = 0;
            if (m_RandomStartFrame)
            {
                currentFrame = Random.Range(0, m_Frames.Length);
            }

            while (true)
            {
                if (currentFrame >= m_Frames.Length)
                {
                    currentFrame = 0;
                }

                var frame = m_Frames[currentFrame];
                if (frame)
                {
                    m_Renderer.sprite = frame;
                }

                ++currentFrame;

                yield return waitCommand;
            }
        }
    }
}
