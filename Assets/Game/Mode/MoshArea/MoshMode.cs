using Game.Character.Player;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Mode.MoshArea
{
    public sealed class MoshMode : AbstractGameMode
    {
        private readonly GameObject[] m_MoshGameObjects;
        private readonly Dictionary<GameObject, bool> m_PauseState = new Dictionary<GameObject, bool>();

        public MoshMode(GameObject[] moshElements)
        {
            m_MoshGameObjects = moshElements;
        }

        public override void Pause()
        {
            base.Pause();

            m_PauseState.Clear();
            foreach (var obj in m_MoshGameObjects)
            {
                if (obj && !m_PauseState.ContainsKey(obj))
                {
                    m_PauseState[obj] = obj.activeSelf;
                    obj.SetActive(false);
                }
            }
        }

        public override void Resume()
        {
            base.Resume();

            foreach (var kvp in m_PauseState)
            {
                kvp.Key.SetActive(kvp.Value);
            }
            m_PauseState.Clear();
        }
    }
}
