using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Character.Common
{
    public interface IMovementController
    {
        void Move(Vector2 force);
        void PlayDirectionAnimation(Vector2 dir);
    }
}
