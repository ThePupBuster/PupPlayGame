using Game.Mode;
using Game.Mode.CommonModes;
using Game.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scene.Triggers.Mode
{
    public class TriggerShowLoadSceneButton : MonoBehaviour
    {
        [SerializeField]
        private string m_SceneName = string.Empty;

        [SerializeField]
        private string m_ButtonText = string.Empty;

        [SerializeField]
        private Button m_InteractButton = null;

        [SerializeField]
        private TextMeshProUGUI m_ButtonTextField = null;

        private bool m_ButtonActive = false;

        private void RegisterInteractEvents()
        {
            UnregisterInteractEvents();
            m_InteractButton.onClick.AddListener(OnButtonClick);
        }

        private void UnregisterInteractEvents()
        {
            m_InteractButton.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            GameModeManager.Instance.PushCheckpoint();
            GameModeManager.Instance.PushMode(new LoadSceneAdditiveMode(m_SceneName, disableCamera: true));
        }

        protected void SetButtonEnabled(bool enabled)
        {
            m_ButtonActive = enabled;
            m_InteractButton.gameObject.SetActive(enabled);

            if (enabled)
            {
                m_ButtonTextField.text = m_ButtonText;
                RegisterInteractEvents();
            }
            else
            {
                UnregisterInteractEvents();
            }
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (!m_ButtonActive && collision.CompareTag(CollisionTags.PLAYER))
            {
                SetButtonEnabled(true);
            }
        }

        protected void OnTriggerExit2D(Collider2D collision)
        {
            if (m_ButtonActive && collision.CompareTag(CollisionTags.PLAYER))
            {
                SetButtonEnabled(false);
            }
        }
    }
}
