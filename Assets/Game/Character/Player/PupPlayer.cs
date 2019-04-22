using Game.Character.Common;
using Game.Core;
using Game.TouchInput;
using UnityEngine;

namespace Game.Character.Player
{
    public sealed class PupPlayer : SingletonBehaviour<PupPlayer>
    {
        private const string MOVEMENT_AXIS_NAME = "Joystick0";

        [SerializeField]
        private float m_Speed = 5;
        private IMovementController m_MovementController;

        private Vector3 m_TargetPos;

        protected override void Awake()
        {
            base.Awake();

            m_MovementController = GetComponent<IMovementController>();

            m_TargetPos = transform.position;
        }

        public void Update()
        {
            Vector2 direction = InputManager.Instance.GetCombinedMovementDirection(MOVEMENT_AXIS_NAME);
            m_MovementController.Move(direction * m_Speed);
        }
    }
}

