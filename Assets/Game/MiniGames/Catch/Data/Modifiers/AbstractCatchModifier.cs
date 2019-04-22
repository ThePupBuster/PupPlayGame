using UnityEngine;

namespace Game.MiniGames.Catch.Data.Modifiers
{
    public abstract class AbstractCatchModifier : ScriptableObject
    {
        [SerializeField]
        protected Sprite m_ActiveSprite;

        public virtual object InitData(Transform playerTransform)
        {
            return null;
        }

        public virtual void Destory(object data)
        {
        }

        public abstract bool IsDone(object data);

        public Sprite ActiveSprite
        {
            get { return m_ActiveSprite; }
        }

        public virtual string GetActiveText(object data)
        {
            return string.Empty;
        }

        public virtual float ModifyTime(float timeRemaining)
        {
            return timeRemaining;
        }

        public virtual float UpdateMovement(object data, float moveXDistance)
        {
            return moveXDistance;
        }
    }
}
