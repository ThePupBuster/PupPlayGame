using UnityEngine;

namespace Game.Scene.Utils
{
    public sealed class RandomScalePicker : MonoBehaviour
    {
        [SerializeField]
        private Vector3[] m_AvailableRanges = null;

        [ContextMenu("Execute")]
        private void Awake()
        {
            transform.localScale = m_AvailableRanges.SelectRandom();
        }
    }
}
