using System;

namespace Game.TouchInput
{
    public interface IInputButton
    {
        bool Active { get; set; }

        event OnInputButtonPressedDelegate OnPress;
    }
}
