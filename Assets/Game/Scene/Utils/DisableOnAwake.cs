using UnityEngine;

namespace Game.Scene.Utils
{
    public sealed class DisableOnAwake : MonoBehaviour
    {
        public void Awake()
        {
            gameObject.SetActive(false);
            Destroy(this);
        }
    }
}
