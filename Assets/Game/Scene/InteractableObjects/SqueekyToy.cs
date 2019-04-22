using UnityEngine;
using UnityEngine.UI;
using Game.Utils;

namespace Game.Scene.InteractableObjects
{
    public class SqueekyToy : MonoBehaviour
    {
        [SerializeField]
        private AudioClip[] m_ClipsToPlay = null;

        [SerializeField]
        private Button m_InteractButton = null;

        private void RegisterInteractEvents()
        {
            UnregisterInteractEvents();
            m_InteractButton.onClick.AddListener(TriggerSound);
        }

        private void UnregisterInteractEvents()
        {
            m_InteractButton.onClick.RemoveListener(TriggerSound);
        }

        private void TriggerSound()
        {
            // TODO: Make some kind of sound manager
            var soundEffect = m_ClipsToPlay.SelectRandom();

            GameObject audioEmitterContainer = new GameObject("SqueekSFX");

            AudioSource audioSource = audioEmitterContainer.AddComponent<AudioSource>();
            audioSource.clip = soundEffect;
            audioSource.volume = 1.0f;
            audioSource.Play();

            Destroy(audioEmitterContainer, soundEffect.length);
        }
        
        private void SetButtonActive(bool active)
        {
            m_InteractButton.gameObject.SetActive(active);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(CollisionTags.PLAYER))
            {
                SetButtonActive(true);
                RegisterInteractEvents();
                TriggerSound();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(CollisionTags.PLAYER))
            {
                SetButtonActive(false);
                UnregisterInteractEvents();
            }
        }
    }
}
