using UnityEngine;
using UnityEngine.UI;

namespace Game.TouchInput.UI
{
    public sealed class StandardScreenButton : AbstractUIInput, IInputButton
    {
        [SerializeField]
        private Button m_Button = null;

        public event OnInputButtonPressedDelegate OnPress;

        protected override void Awake()
        {
            base.Awake();

            m_Button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            OnPress?.Invoke(InputNameKey);
        }
    }
}
