using UnityEngine;

namespace Movement
{
    public struct InputState
    {
        public Vector2 Movement { get; private set; }
        public Vector2 Look { get; private set; }
        public bool Jump { get; private set; }
        public bool Sprint { get; private set; }
        public bool Activate { get; private set; }
        public bool Special { get; private set; }
        public bool Back { get; private set; }

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

        public void SetActivate(bool activate)
        {
            this.Activate = activate;
        }

        public void SetSpecial(bool special)
        {
            this.Special = special;
        }

        public void SetBack(bool back)
        {
            this.Back = back;
        }

        public InputState Combine(InputState inputState)
        {
            InputState toReturn = new InputState();
            toReturn.Movement = inputState.Movement;
            toReturn.Jump = this.Jump | inputState.Jump;
            toReturn.Sprint = this.Sprint | inputState.Sprint;

            toReturn.Activate = this.Activate | inputState.Activate;
            toReturn.Back = this.Back | inputState.Back;

            return toReturn;
        }
    }
}