using System;
using System.Text;
using System.Collections.Generic;
using Game.Core;
using Game.TouchInput.UI;
using UnityEngine;

namespace Game.TouchInput
{
    public sealed class InputManager : SingletonBehaviour<InputManager>
    {
        private const string KEYBOARD_HORIZONTAL = "Horizontal";
        private const string KEYBOARD_VERTICAL = "Vertical";

        [SerializeField]
        private Camera m_UICamera;

        private readonly Dictionary<string, IInputAxis> m_Axies = new Dictionary<string, IInputAxis>();
        private readonly Dictionary<string, IInputButton> m_Buttons = new Dictionary<string, IInputButton>();
        private readonly Dictionary<string, AbstractUIInput> m_AllInputs = new Dictionary<string, AbstractUIInput>();

        public event OnInputButtonPressedDelegate OnInputButtonPressed; 

        public Camera UICamera
        {
            get { return m_UICamera; }
        }

        internal void Register(AbstractUIInput inputHandler)
        {
            AbstractUIInput existingRegistered;
            if (m_AllInputs.TryGetValue(inputHandler.InputNameKey, out existingRegistered))
            {
                if (existingRegistered != inputHandler)
                {
                    StringBuilder pathBuilder = new StringBuilder();
                    string input1Path = inputHandler.PrintDebugHierarchy(pathBuilder).ToString();

                    pathBuilder.Length = 0;
                    string input2Path = existingRegistered.PrintDebugHierarchy(pathBuilder).ToString();

                    throw new InvalidOperationException(String.Format("Duplicate inputs registered for key: {0}. Input 1: {1} Input 2: {2}", inputHandler.InputNameKey, input1Path, input2Path));
                }
                return;
            }

            m_AllInputs.Add(inputHandler.InputNameKey, inputHandler);

            m_Axies.AddIfImplements(inputHandler.InputNameKey, inputHandler);
            m_Buttons.AddIfImplements(inputHandler.InputNameKey, inputHandler);

            IInputButton btn = inputHandler as IInputButton;
            if (btn != null)
            {
                btn.OnPress -= HandleOnInputButtonPressed;
                btn.OnPress += HandleOnInputButtonPressed;
            }
        }

        internal void Unregister(AbstractUIInput inputHandler)
        {
            m_AllInputs.RemoveIfExists(inputHandler.InputNameKey, inputHandler);

            m_Axies.RemoveIfExistsAndImplements(inputHandler.InputNameKey, inputHandler);
            m_Buttons.RemoveIfExistsAndImplements(inputHandler.InputNameKey, inputHandler);

            IInputButton btn = inputHandler as IInputButton;
            if (btn != null)
            {
                btn.OnPress -= HandleOnInputButtonPressed;
                btn.OnPress += HandleOnInputButtonPressed;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            this.ResolveOptionalAssignment(ref m_UICamera);

            foreach (var comp in GetComponentsInChildren<AbstractUIInput>())
            {
                Register(comp);
            }
        }

        private void HandleOnInputButtonPressed(string buttonName)
        {
            OnInputButtonPressed?.Invoke(buttonName);
        }

        public Vector2 GetAxis(string name)
        {
            IInputAxis axis;
            if (m_Axies.TryGetValue(name, out axis))
            {
                return axis.GetAxis();
            }

            Debug.LogError("Unable to find axis: " + name);
            return Vector2.zero;
        }

        public Vector2 GetCombinedMovementDirection(string axis)
        {
            Vector2 input = GetAxis(axis);

            float moveHorizontal = Input.GetAxisRaw(KEYBOARD_HORIZONTAL);
            float moveVertical = Input.GetAxisRaw(KEYBOARD_VERTICAL);
            if (moveHorizontal != 0 || moveVertical != 0)
            {
                input.x = moveHorizontal;
                input.y = moveVertical;
            }

            return input;
        }
    }
}
