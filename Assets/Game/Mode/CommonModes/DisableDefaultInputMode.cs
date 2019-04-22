using Game.TouchInput;

namespace Game.Mode.CommonModes
{
    public sealed class DisableDefaultInputMode : AbstractGameMode
    {
        private bool m_WasInputEnabled;

        public override void Awake()
        {
            base.Awake();

            m_WasInputEnabled = InputManager.Instance.gameObject.activeInHierarchy;
            InputManager.Instance.gameObject.SetActive(false);
        }

        public override void Destroy()
        {
            base.Destroy();

            InputManager.Instance.gameObject.SetActive(m_WasInputEnabled);
        }
    }
}
