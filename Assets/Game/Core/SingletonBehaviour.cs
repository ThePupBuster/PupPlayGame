using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Core
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour
        where T : class
    {
        public static T Instance { get; private set; } = null;

        protected virtual void Awake()
        {
            Instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            if (Instance == (this as T))
            {
                Instance = null;
            }
        }
    }
}
