using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game.TouchInput.UI
{
    public sealed class ScreenJoystick : AbstractUIInput, IInputAxis, 
        IPointerUpHandler, IPointerDownHandler, IDragHandler
    {
        [SerializeField]
        private Image m_Joystick = null;

        [SerializeField]
        private Image m_Background = null;

        [SerializeField]
        private RectTransform m_JoystickTransform = null;

        [SerializeField]
        private RectTransform m_BackgroundTransform = null;

        [SerializeField]
        private float m_MaxDistance = 5.0f;

        private bool m_TouchDown = false;

        private bool m_Dirty = false;
        private Vector2 m_Axis = Vector2.zero;

        public Vector2 GetAxis()
        {
            if (m_TouchDown)
            {
                if (m_Dirty)
                {
                    m_Dirty = false;
                    m_Axis = m_JoystickTransform.position - m_BackgroundTransform.position;

                    float length = m_Axis.magnitude;
                    length /= m_MaxDistance;

                    m_Axis = m_Axis.normalized * length;
                }
                return m_Axis;
            }
            return Vector2.zero;
        }

        private void SetColour(Color color)
        {
            m_Joystick.color = color;
            m_Background.color = color;
        }

        private void OnEnable()
        {
            SetColour(Color.clear);
            m_TouchDown = false;
        }

        private void UpdatePosition(Vector2 pos)
        {
            m_Dirty = true;

            if (m_TouchDown)
            {
                Vector2 backgroundPos = m_BackgroundTransform.position.ToVec2();
                Vector2 delta = pos - backgroundPos;
                float lenSqr = delta.sqrMagnitude;
                if (lenSqr > (m_MaxDistance * m_MaxDistance))
                {
                    pos = delta.normalized * m_MaxDistance;
                    pos += backgroundPos;
                }
                m_JoystickTransform.position = pos;
            }
            else
            {
                m_TouchDown = true;
                m_BackgroundTransform.position = pos;
                m_JoystickTransform.position = pos;
                SetColour(Color.white);
            }
        }

        private Vector2 ScreenToWorld(Vector2 pos)
        {
            return InputManager.Instance.UICamera.ScreenToWorldPoint(pos);
        }

        public void OnDrag(PointerEventData eventData)
        {
            UpdatePosition(ScreenToWorld(eventData.position));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            UpdatePosition(ScreenToWorld(eventData.position));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            m_TouchDown = false;
            SetColour(Color.clear);
        }
    }
}
