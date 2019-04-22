using Game.Mode;
using Game.Mode.CommonModes;
using Game.Scene.Init;
using TMPro;
using UnityEngine;

namespace Game.MiniGames.Stacking
{
    public sealed class StackingSceneInit : AbstractMiniGameSceneInit
    {
        public TextMeshProUGUI m_ScoreText = null;
        public GameObject[] m_PoolNoodles = null;
        public GameObject m_Floor = null;
        public Camera m_Camera = null;

        protected override void Awake()
        {
            base.Awake();

            GameModeManager.Instance.PushMode(new DisableDefaultInputMode());
        }

        public override void SetGameUIElementsActive(bool active)
        {
            base.SetGameUIElementsActive(active);

            m_Floor.SetActive(active);
        }

        public override void StartGame()
        {
            base.StartGame();

            GameModeManager.Instance.PushMode(new StackingGameMode(this));
        }
    }
}
