using UnityEngine;
using System;
using System.Collections.Generic;
using Game.Character.Common;

namespace Game.Character.AI
{
    public sealed class BasicSceneAI : MonoBehaviour
    {
        [Serializable]
        private enum State
        {
            Moving,
            Looking
        }

        private static readonly Vector2[] LOOK_DIRS = new Vector2[]
        {
            new Vector2(1,0),
            new Vector2(0,1),
            new Vector2(-1,0),
            new Vector2(0,-1)
        };

        [SerializeField]
        private Transform[] m_WayPoints = null;

        [SerializeField]
        private float m_Speed = 5f;

        [SerializeField]
        private float m_LookDelayMin = 0.5f;

        [SerializeField]
        private float m_LookDelayMax = 3f;

        [SerializeField]
        private State m_CurrentState = State.Moving;

        [SerializeField]
        private bool m_RandomizeTargetWaypoints = false;
        
        [SerializeField]
        private int m_TargetIdx = 0;

        private IMovementController m_MovementController;
        private int m_LookDirIdx = 0;
        private float m_LookDelayTimeout = 0.0f;
        private Dictionary<State, Action> m_StateFunctions = null;

        private void Awake()
        {
            m_MovementController = GetComponent<IMovementController>();
            m_StateFunctions = new Dictionary<State, Action>()
            {
                { State.Moving, Update_Moving },
                { State.Looking, Update_Looking }
            };
        }

        private void LateUpdate()
        {
            m_StateFunctions[m_CurrentState]();
        }

        private void Update_Moving()
        {
            var targetPoint = m_WayPoints[m_TargetIdx].position.ToVec2();
            var currentPoint = transform.position.ToVec2();

            var dir = targetPoint - currentPoint;
            var distanceSqr = dir.sqrMagnitude;
            if (distanceSqr > 0.1f)
            {
                m_MovementController.Move(dir.normalized * m_Speed);
            }
            else
            {
                m_TargetIdx++;
                if (m_RandomizeTargetWaypoints)
                {
                    m_TargetIdx = UnityEngine.Random.Range(0, m_WayPoints.Length);
                }
                if (m_TargetIdx >= m_WayPoints.Length)
                {
                    m_TargetIdx = 0;
                    m_LookDelayTimeout = 0.0f;
                }

                if (UnityEngine.Random.Range(0, 2) == 1)
                {
                    m_LookDirIdx = 0;
                    m_CurrentState = State.Looking;
                }
                else
                {
                    m_CurrentState = State.Moving;
                }
            }
        }

        private void Update_Looking()
        {
            if (m_LookDelayTimeout <= Time.timeSinceLevelLoad)
            {
                m_MovementController.PlayDirectionAnimation(LOOK_DIRS[m_LookDirIdx++]);
                m_MovementController.PlayDirectionAnimation(new Vector2(0.0f, 0.0f));
                if (m_LookDirIdx >= LOOK_DIRS.Length)
                {
                    m_CurrentState = State.Moving;
                    m_LookDirIdx = 0;
                    m_LookDelayTimeout = 0.0f;
                }
                else
                {
                    m_LookDelayTimeout = Time.timeSinceLevelLoad + UnityEngine.Random.Range(m_LookDelayMin, m_LookDelayMax);
                }
            }
        }
    }
}
