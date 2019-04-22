using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Core
{
    [AddComponentMenu("")]
    public abstract class AutoCreatedSingletonBehaviour<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static T s_Instance = null;
        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name + " Container");
                    DontDestroyOnLoad(obj);
                    s_Instance = obj.AddComponent<T>();
                }
                return s_Instance;
            }
        }

        public virtual void Awake()
        {
            s_Instance = this as T;
        }

        public virtual void OnDestroy()
        {
            if (s_Instance == (this as T))
            {
                s_Instance = null;
            }
        }
    }
}
