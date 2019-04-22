using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MiniGames.Catch.UI
{
    public sealed class ActiveModifierUIElement : MonoBehaviour
    {
        [SerializeField]
        private Image m_DisplayImage = null;

        [SerializeField]
        private TextMeshProUGUI m_ActiveText = null;

        public float Height
        {
            get { return m_DisplayImage.rectTransform.sizeDelta.y; }
        }

        public void SetText(string text)
        {
            m_ActiveText.text = text;
        }

        public void SetSprite(Sprite sprite)
        {
            m_DisplayImage.sprite = sprite;
        }
    }
}
