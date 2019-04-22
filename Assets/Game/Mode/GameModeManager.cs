using Game.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Mode
{
    public sealed class GameModeManager : AutoCreatedSingletonBehaviour<GameModeManager>
    {
        private readonly Stack<AbstractGameMode> m_ActiveModes = new Stack<AbstractGameMode>();
        private readonly Stack<int> m_Checkpoint = new Stack<int>();

        public void PushCheckpoint()
        {
            m_Checkpoint.Push(m_ActiveModes.Count);
        }

        public void PopCheckpoint()
        {
            int targetLength = m_Checkpoint.Pop();

            int numToPop = m_ActiveModes.Count - targetLength;
            PopMode(numToPop);
        }

        public void PushMode(AbstractGameMode mode)
        {
            if (m_ActiveModes.Count > 0)
            {
                m_ActiveModes.Peek().Pause();
            }
            m_ActiveModes.Push(mode);
            mode.Awake();
        }

        public void PushModes(params AbstractGameMode[] modes)
        {
            foreach (var mode in modes)
            {
                PushMode(mode);
            }
        }

        public void PopMode(int numToPop = 1)
        {
            for (int i = 0; i < numToPop && m_ActiveModes.Count > 0; ++i)
            {
                var previousMode = m_ActiveModes.Pop();
                previousMode.Destroy();

                if (m_ActiveModes.Count > 0)
                {
                    m_ActiveModes.Peek().Resume();
                }
            }
        }

        private void Update()
        {
            if (m_ActiveModes.Count > 0)
            {
                m_ActiveModes.Peek().Update();
            }
        }
    }
}
