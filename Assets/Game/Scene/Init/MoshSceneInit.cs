using Game.Mode;
using Game.Mode.MoshArea;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Scene.Init
{
    public sealed class MoshSceneInit : AbstractSceneInit
    {
        [SerializeField]
        private GameObject[] m_PauseableGameObjects = null;

        private void Awake()
        {
            GameModeManager.Instance.PushMode(new MoshMode(m_PauseableGameObjects));
        }
    }
}
