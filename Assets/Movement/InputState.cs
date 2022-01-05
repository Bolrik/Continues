using UnityEngine;

namespace Movement
{
    public struct InputState
    {
        public Vector2 Movement { get; private set; }
        public Vector2 Look { get; private set; }
        public bool Jump { get; private set; }
        public bool Sprint { get; private set; }

        public void SetMovement(Vector2 movement)
        {
            this.Movement = movement;
        }
        
        public void SetLook(Vector2 look)
        {
            this.Look = look;
        }

        public void SetJump(bool jump)
        {
            this.Jump = jump;
        }

        public void SetSprint(bool sprint)
        {
            this.Sprint = sprint;
        }

        public InputState Combine(InputState inputState)
        {
            InputState toReturn = new InputState();
            toReturn.Movement = inputState.Movement;
            toReturn.Jump = this.Jump | inputState.Jump;
            toReturn.Sprint = this.Sprint | inputState.Sprint;

            return toReturn;
        }
    }
}