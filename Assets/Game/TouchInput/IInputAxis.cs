using UnityEngine;

namespace Game.TouchInput
{
    public interface IInputAxis
    {
        bool Active { get; set; }

        Vector2 GetAxis();
    }
}
