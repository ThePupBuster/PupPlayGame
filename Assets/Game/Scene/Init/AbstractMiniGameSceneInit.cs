using Game.Mode;
using Game.Utils;
using TMPro;
using UnityEngine;

namespace Game.Scene.Init
{
    public abstract class AbstractMiniGameSceneInit : AbstractSceneInit
    {
        public GameObject m_StartDialog = null;
        public GameObject m_EndDialog = null;

        public GameObject m_GameActiveUIElementRoot = null;
        public TextMeshProUGUI m_EndGameScoreValue = null;

        protected virtual void Awake()
        {
            SetGameUIElementsActive(false);

            if (m_EndDialog)
            {
                m_EndDialog.SetActive(false);
            }

            if (m_StartDialog)
            {
                m_StartDialog.SetActive(true);
            }
        }

        public virtual void SetGameUIElementsActive(bool active)
        {
            if (m_GameActiveUIElementRoot)
            {
                m_GameActiveUIElementRoot.SetActive(active);
            }
        }

        public void ShowEndGameDialog(int finalScore)
        {
            if (m_EndGameScoreValue)
            {
                m_EndGameScoreValue.text = StringUtils.CommaSeperateNumber(finalScore);
            }

            if (m_EndDialog)
            {
                m_EndDialog.SetActive(true);
            }

            SetGameUIElementsActive(false);
        }

        public virtual void StartGame()
        {
            if (m_StartDialog)
            {
                Destroy(m_StartDialog);
                m_StartDialog = null;
            }

            if (m_EndDialog)
            {
                m_EndDialog.SetActive(false);
            }

            SetGameUIElementsActive(true);
        }

        public virtual void EndGame()
        {
            GameModeManager.Instance.PopCheckpoint();
        }
    }
}
