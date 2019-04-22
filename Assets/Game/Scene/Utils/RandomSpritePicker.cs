using UnityEngine;

namespace Game.Scene.Utils
{
    public sealed class RandomSpritePicker : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] m_Sprites = null;

        [ContextMenu("Execute")]
        private void Awake()
        {
            GetComponent<SpriteRenderer>().sprite = m_Sprites.SelectRandom();
        }
    }
}
