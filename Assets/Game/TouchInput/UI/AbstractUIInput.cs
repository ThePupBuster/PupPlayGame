using System;
using UnityEngine;

namespace Game.TouchInput.UI
{
    public abstract class AbstractUIInput : MonoBehaviour
    {
        [SerializeField]
        private string m_InputNameKey = Guid.NewGuid().ToString();

        public string InputNameKey
        {
            get { return m_InputNameKey; }
        }

        public virtual bool Active
        {
            get
            {
                return gameObject.activeInHierarchy;
            }

            set
            {
                gameObject.SetActive(value);
            }
        }

        protected virtual void Awake()
        {
            if (InputManager.Instance)
            {
                InputManager.Instance.Register(this);
            }
        }

        protected virtual void OnDestroy()
        {
            if (InputManager.Instance)
            {
                InputManager.Instance.Unregister(this);
            }
        }
    }
}
