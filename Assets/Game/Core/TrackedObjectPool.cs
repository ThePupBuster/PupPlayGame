using System;
using System.Collections.Generic;

namespace Game.Core
{
    public sealed class TrackedObjectPool<T> : Pool<T>
    {
        public List<T> m_ActiveObjects = new List<T>();

        public IList<T> ActiveItems
        {
            get
            {
                return m_ActiveObjects;
            }
        }

        public TrackedObjectPool(Func<Pool<T>, T> newObject, int initialSize = 0)
            : base(newObject, initialSize)
        {
        }

        public override T New()
        {
            T newObj = base.New();

            m_ActiveObjects.Add(newObj);

            return newObj;
        }

        public override void Free(T item)
        {
            m_ActiveObjects.Remove(item);
            base.Free(item);
        }

        public override void DestroyPool(Action<T> destroyCallback)
        {
            base.DestroyPool(destroyCallback);

            foreach (T obj in m_ActiveObjects)
            {
                destroyCallback(obj);
            }
            m_ActiveObjects.Clear();
        }
    }
}
