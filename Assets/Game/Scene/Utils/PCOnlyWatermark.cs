using UnityEngine;

namespace Game.Scene.Utils
{
    public sealed class PCOnlyWatermark : MonoBehaviour
    {
        private void Awake()
        {
#if UNITY_ANDROID || UNITY_IOS
            GameObject.Destroy(gameObject);
#else
            GameObject.DontDestroyOnLoad(gameObject);
#endif
        }
    }
}
