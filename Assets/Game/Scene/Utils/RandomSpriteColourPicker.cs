using UnityEngine;

namespace Game.Scene.Utils
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class RandomSpriteColourPicker : MonoBehaviour
    {
        [SerializeField]
        private Color[] m_Colours = null;

        [ContextMenu("Execute")]
        private void Awake()
        {
            GetComponent<SpriteRenderer>().color = m_Colours.SelectRandom();
        }
    }
}
