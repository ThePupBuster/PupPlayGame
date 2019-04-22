using Game.Core;
using Game.MiniGames.Catch.Data;
using Game.MiniGames.Catch.Data.Modifiers;
using Game.MiniGames.Catch.UI;
using Game.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Game.MiniGames.Catch
{
    public sealed class CatchGamePlayer : MonoBehaviour
    {
        private struct ActiveModifiers
        {
            internal object m_Data;
            internal AbstractCatchModifier m_Modifier;
            internal ActiveModifierUIElement m_UIElement;

            internal ActiveModifiers(AbstractCatchModifier modifier, ActiveModifierUIElement uiElement, Transform playerTransform)
            {
                m_Data = modifier.InitData(playerTransform);
                m_Modifier = modifier;
                m_UIElement = uiElement;
            }
        }

        [SerializeField]
        private SpriteRenderer m_Sprite = null;

        [SerializeField]
        private Camera m_Camera = null;

        [SerializeField]
        private float m_MaxSpeed = 5f;

        [SerializeField]
        private RectTransform m_ModifierUIContainer = null;

        [SerializeField]
        private ActiveModifierUIElement m_ActiveModifierUI = null;

        private readonly List<ActiveModifiers> m_ActiveModifiers = new List<ActiveModifiers>();
        private Pool<ActiveModifierUIElement> m_ActiveModiferUIPool = null;

        public delegate void OnClaimedItemDelegate(CatchSpawnableObject caughtObject);
        public event OnClaimedItemDelegate OnClaimedItem;

        private void Awake()
        {
            m_ActiveModiferUIPool = new Pool<ActiveModifierUIElement>(CreateNewModifierUIElement, 3);
        }

        private void OnDestroy()
        {
            m_ActiveModiferUIPool.DestroyPool(MonoBehaviourUtils.DestoryBehaviourGameObject);

            foreach (var modifier in m_ActiveModifiers)
            {
                modifier.m_Modifier.Destory(modifier.m_Data);
            }
            m_ActiveModifiers.Clear();
        }

        private ActiveModifierUIElement CreateNewModifierUIElement(Pool<ActiveModifierUIElement> pool)
        {
            GameObject newGameObject = Instantiate(m_ActiveModifierUI.gameObject);
            newGameObject.SetActive(false);
            newGameObject.GetComponent<RectTransform>().SetParent(m_ModifierUIContainer);
            return newGameObject.GetComponent<ActiveModifierUIElement>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 inputPos = m_Camera.ScreenToWorldPoint(Input.mousePosition);

                var currentPos = transform.position;

                float moveDistance = inputPos.x - currentPos.x;
                float moveSign = Mathf.Sign(moveDistance);
                float absMoveDistance = moveDistance * moveSign;

                if (absMoveDistance > MathUtils.EPSILON)
                {
                    absMoveDistance = Mathf.Min(m_MaxSpeed * Time.deltaTime, absMoveDistance);
                    moveDistance = absMoveDistance * moveSign;

                    foreach (var modifierData in m_ActiveModifiers)
                    {
                        moveDistance = modifierData.m_Modifier.UpdateMovement(modifierData.m_Data, moveDistance);
                    }

                    m_Sprite.flipX = moveDistance < 0.0f;
                    currentPos.x += moveDistance;
                    transform.position = currentPos;
                }
            }

            UpdateModifiers();
        }

        private void UpdateModifiers()
        {
            for (int i = 0; i < m_ActiveModifiers.Count; ++i)
            {
                var uiElement = m_ActiveModifiers[i].m_UIElement;
                if (m_ActiveModifiers[i].m_Modifier.IsDone(m_ActiveModifiers[i].m_Data))
                {
                    m_ActiveModifiers[i].m_Modifier.Destory(m_ActiveModifiers[i].m_Data);
                    uiElement.gameObject.SetActive(false);

                    m_ActiveModiferUIPool.Free(uiElement);

                    m_ActiveModifiers.RemoveAt(i);
                    i--;
                }
                else
                {
                    uiElement.transform.position = new Vector3(0.0f, i * uiElement.Height, 0.0f);
                    uiElement.SetText(m_ActiveModifiers[i].m_Modifier.GetActiveText(m_ActiveModifiers[i].m_Data));
                }
            }
        }

        private void AddModifier(ActiveModifiers modifierToAdd)
        {
            // remove any duplicates
            for (int i = 0; i < m_ActiveModifiers.Count; ++i)
            {
                if (m_ActiveModifiers[i].m_Modifier == modifierToAdd.m_Modifier)
                {
                    m_ActiveModifiers[i].m_Modifier.Destory(m_ActiveModifiers[i].m_Data);
                    var uiElement = m_ActiveModifiers[i].m_UIElement;
                    uiElement.gameObject.SetActive(false);
                    m_ActiveModiferUIPool.Free(uiElement);

                    m_ActiveModifiers.RemoveAt(i);
                    i--;
                }
            }

            m_ActiveModifiers.Add(modifierToAdd);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(CollisionTags.COLLECTABLE))
            {
                var itemCaught = collision.gameObject.GetComponent<CatchGameFallingItem>();
                var itemData = itemCaught.ItemData;

                if (itemData.ExplosionOnCapture)
                {
                    var go = Instantiate(itemData.ExplosionOnCapture);
                    go.transform.SetParent(itemCaught.transform.parent);
                    go.transform.position = itemCaught.transform.position;
                }

                itemCaught.Remove();

                OnClaimedItem?.Invoke(itemData);

                if (itemData.Modifier != null)
                {
                    var uiElement = m_ActiveModiferUIPool.New();
                    uiElement.gameObject.SetActive(true);
                    uiElement.SetSprite(itemData.Modifier.ActiveSprite);

                    AddModifier(new ActiveModifiers(itemData.Modifier, uiElement, transform));
                }
            }
        }
    }
}
