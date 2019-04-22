using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Game.Core
{
    public abstract class Singleton<T>
        where T:class, new()
    {
        private static T s_Instance = null;
        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new T();
                }
                return s_Instance;
            }
        }
    }
}
