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
        public bool Restart { get; private set; }

        public void SetMovement(Vector2 value)
        {
            this.Movement = value;
        }
        
        public void SetLook(Vector2 value)
        {
            this.Look = value;
        }

        public void SetJump(bool value)
        {
            this.Jump = value;
        }

        public void SetSprint(bool value)
        {
            this.Sprint = value;
        }

        public void SetActivate(bool value)
        {
            this.Activate = value;
        }

        public void SetSpecial(bool value)
        {
            this.Special = value;
        }

        public void SetBack(bool value)
        {
            this.Back = value;
        }

        public void SetRestart(bool value)
        {
            this.Restart = value;
        }

        public InputState Combine(InputState inputState)
        {
            InputState toReturn = new InputState();
            toReturn.Movement = inputState.Movement;
            toReturn.Jump = this.Jump | inputState.Jump;
            toReturn.Sprint = this.Sprint | inputState.Sprint;

            toReturn.Activate = this.Activate | inputState.Activate;
            toReturn.Back = this.Back | inputState.Back;
            toReturn.Restart = this.Restart | inputState.Restart;

            return toReturn;
        }
    }
}