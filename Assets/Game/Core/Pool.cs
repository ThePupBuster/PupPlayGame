using System;
using System.Collections.Generic;

namespace Game.Core
{
    public class Pool<T>
    {
        private readonly Func<Pool<T>, T> m_NewObject;
        private readonly Queue<T> m_FreeObjects;

        public Pool(Func<Pool<T>, T> newObject, int initialSize = 0)
        {
            m_NewObject = newObject;
            m_FreeObjects = new Queue<T>(initialSize);

            for (int i = 0; i < initialSize; ++i)
            {
                m_FreeObjects.Enqueue(m_NewObject(this));
            }
        }

        public bool IsExhausted
        {
            get { return m_FreeObjects.Count == 0; }
        }

        public virtual T New()
        {
            if (IsExhausted)
            {
                return m_NewObject(this);
            }

            return m_FreeObjects.Dequeue();
        }

        public virtual void Free(T item)
        {
            m_FreeObjects.Enqueue(item);
        }

        public virtual void DestroyPool(Action<T> destroyCallback)
        {
            while (m_FreeObjects.Count > 0)
            {
                destroyCallback(m_FreeObjects.Dequeue());
            }
        }
    }
}
