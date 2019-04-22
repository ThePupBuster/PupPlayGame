using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Mode.CommonModes
{
    public class LoadSceneAdditiveMode : AbstractGameMode
    {
        private readonly string m_SceneToLoad;
        private readonly bool m_DisableCamera;

        private GameObject m_OldCamera;

        public LoadSceneAdditiveMode(string scene, bool disableCamera)
        {
            m_SceneToLoad = scene;
            m_DisableCamera = disableCamera;
        }

        public override void Awake()
        {
            base.Awake();

            if (m_DisableCamera && Camera.main && Camera.main.gameObject.activeSelf)
            {
                m_OldCamera = Camera.main.gameObject;
                m_OldCamera.SetActive(false);
            }

            SceneManager.LoadScene(m_SceneToLoad, LoadSceneMode.Additive);
        }

        public override void Destroy()
        {
            base.Destroy();

            if (m_DisableCamera && m_OldCamera)
            {
                if (Camera.main)
                {
                    Camera.main.gameObject.SetActive(false);
                }
                m_OldCamera.SetActive(true);
            }

            SceneManager.UnloadSceneAsync(m_SceneToLoad);
        }
    }
}
