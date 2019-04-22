using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Character.Common
{
    public class FourDirectionMovementController : MonoBehaviour, IMovementController
    {
        private const string PARAM_MOVE_ACTIVE = "IsMoving";
        private const string PARAM_MOVE_DIRECTION = "Direction";

        [SerializeField]
        private Animator m_Animator;

        [SerializeField]
        private Rigidbody2D m_RigidBody;

        private void Awake()
        {
            this.ResolveOptionalAssignment(ref m_Animator);
            this.ResolveOptionalAssignment(ref m_RigidBody);
        }

        public void Move(Vector2 force)
        {
            PlayDirectionAnimation(force);
            m_RigidBody.AddForce(force);
        }

        public void PlayDirectionAnimation(Vector2 dir)
        {
            float moveHorizontal = dir.x;
            float moveVertical = dir.y;

            // IF we are moving we set the animation IsMoving to true,
            // ELSE we are not moving.
            if (moveHorizontal != 0 || moveVertical != 0)
            {
                m_Animator.SetBool(PARAM_MOVE_ACTIVE, true);
            }
            else
            {
                m_Animator.SetBool(PARAM_MOVE_ACTIVE, false);
                return;
            }

            // We are wanting to go in the positive X direction.
            if (moveHorizontal > 0 && Mathf.Abs(moveVertical) <= Mathf.Abs(moveHorizontal))
            {
                m_Animator.SetInteger(PARAM_MOVE_DIRECTION, 4);
                // We are wanting to move in the negative X direction.
            }
            else if (moveHorizontal < 0 && Mathf.Abs(moveVertical) <= Mathf.Abs(moveHorizontal))
            {
                m_Animator.SetInteger(PARAM_MOVE_DIRECTION, 2);
                // We are wanting to move in the negative Y direction.
            }
            else if (moveVertical < 0 && Mathf.Abs(moveVertical) > Mathf.Abs(moveHorizontal))
            {
                m_Animator.SetInteger(PARAM_MOVE_DIRECTION, 3);
                // We are wanting to move in the positive Y direction.
            }
            else if (moveVertical > 0 && Mathf.Abs(moveVertical) > Mathf.Abs(moveHorizontal))
            {
                m_Animator.SetInteger(PARAM_MOVE_DIRECTION, 1);
            }
        }
    }
}
