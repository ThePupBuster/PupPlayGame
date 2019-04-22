using Game.Scene.CameraControllers;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Game.Mode;
using Game.Utils;

namespace Game.MiniGames.Stacking
{
    public sealed class StackingGameMode : AbstractGameMode
    {
        private const float OUT_OF_BOUNDS_OFFSET = 1f;
        private const float INITIAL_OFFSET = 7f;
        private const float SPEED = 5f;
        private const float MAX_SIDE_MOVEMENT = 7f;
        private const float MIN_WAIT_TIME_SECONDS = 1f;
        
        private enum State
        {
            Spawn,
            MovingNoodle,
            WaitNoodle,
            End
        }

        private readonly StackingSceneInit m_SceneInitData;
        private readonly TrackTargetsCamera m_CameraTracker;

        private readonly Dictionary<State, System.Action> m_StateUpdateFunctions;

        private readonly float m_MinHeight;
        private State m_CurrentState = State.Spawn;
        private GameObject m_CurrentNoodle = null;
        private float m_NoodleHeight = 0;
        private float m_Direction = -1;
        private readonly List<Transform> m_SpawnedNoodles = new List<Transform>();
        private float m_MinWaitTime;
        private readonly float m_CamInitialYOffset;

        public StackingGameMode(StackingSceneInit sceneInit)
        {
            m_SceneInitData = sceneInit;

            m_CameraTracker = m_SceneInitData.m_Camera.GetOrCreateComponent<TrackTargetsCamera>();

            m_CameraTracker.AddTarget(m_SceneInitData.m_Floor.transform);

            m_CamInitialYOffset = m_SceneInitData.m_Camera.transform.position.y;

            m_MinHeight = m_SceneInitData.m_Floor.transform.position.y - OUT_OF_BOUNDS_OFFSET;

            m_StateUpdateFunctions = new Dictionary<State, System.Action>()
            {
                { State.Spawn, Update_Spawn },
                { State.MovingNoodle, Update_Moving },
                { State.WaitNoodle, Update_WaitNoodle },
                { State.End, Update_End }
            };

            m_SceneInitData.m_ScoreText.text = "0";
        }

        public override void Destroy()
        {
            base.Destroy();

            foreach (var noodle in m_SpawnedNoodles)
            {
                if (m_CameraTracker)
                {
                    m_CameraTracker.RemoveTarget(noodle.transform);
                }
                GameObject.Destroy(noodle.gameObject);
            }
            m_SpawnedNoodles.Clear();
        }

        public override void Update()
        {
            base.Update();

            if (m_CurrentState != State.End)
            {
                foreach (var noodleTransform in m_SpawnedNoodles)
                {
                    if (noodleTransform.position.y < m_MinHeight)
                    {
                        m_CurrentState = State.End;
                        break;
                    }
                }
            }

            m_StateUpdateFunctions[m_CurrentState]();
        }

        private void Update_Spawn()
        {
            m_CurrentState = State.MovingNoodle;

            float nextYOffset = 0.0f;
           
            foreach (var noodle in m_SpawnedNoodles)
            {
                nextYOffset = Mathf.Max(nextYOffset, noodle.GetComponent<SpriteRenderer>().bounds.max.y);
            }

            m_CurrentNoodle = GameObject.Instantiate<GameObject>(m_SceneInitData.m_PoolNoodles.SelectRandom());

            nextYOffset += INITIAL_OFFSET + m_CurrentNoodle.GetComponent<SpriteRenderer>().bounds.extents.y;

            m_CurrentNoodle.GetComponent<Rigidbody2D>().simulated = false;
            m_NoodleHeight = m_CurrentNoodle.transform.GetChild(0).transform.position.y - m_CurrentNoodle.transform.position.y;
            m_CurrentNoodle.transform.position = m_SceneInitData.m_Floor.transform.position + new Vector3(0, nextYOffset, 0);
            m_SpawnedNoodles.Add(m_CurrentNoodle.transform);

            m_CameraTracker.AddTarget(m_CurrentNoodle.transform);
        }

        private void Update_Moving()
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_CurrentNoodle.GetComponent<Rigidbody2D>().simulated = true;
                m_CurrentState = State.WaitNoodle;

                m_MinWaitTime = Time.timeSinceLevelLoad + MIN_WAIT_TIME_SECONDS;
                return;
            }

            var pos = m_CurrentNoodle.transform.position;
            pos.x += m_Direction * SPEED * Time.deltaTime;
            var xDist = Mathf.Abs(pos.x - m_SceneInitData.m_Floor.transform.position.x);
            if (xDist >= MAX_SIDE_MOVEMENT)
            {
                pos.x = m_SceneInitData.m_Floor.transform.position.x + (MAX_SIDE_MOVEMENT * m_Direction);
                m_Direction *= -1;
            }

            m_CurrentNoodle.transform.position = pos;
        }

        private void Update_WaitNoodle()
        {
            if (Time.timeSinceLevelLoad < m_MinWaitTime)
                return;

            var velocity = m_CurrentNoodle.GetComponent<Rigidbody2D>().velocity.SqrMagnitude();
            if (velocity <= 0.000001f)
            {
                m_CurrentState = State.Spawn;

                m_SceneInitData.m_ScoreText.text = StringUtils.CommaSeperateNumber(m_SpawnedNoodles.Count);
            }
        }

        private void Update_End()
        {
            m_SceneInitData.ShowEndGameDialog(int.Parse(m_SceneInitData.m_ScoreText.text.Replace(",", "")));
            GameModeManager.Instance.PopMode();
        }
    }
}
