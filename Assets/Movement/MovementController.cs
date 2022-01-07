using Inputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class MovementController : MonoBehaviour, IInputStateProvider
    {
        private InputObserver InputObserver { get; set; }
        public InputState InputState { get; private set; }

        public OnInputStateChanged InputStateChanged { get; set; }


        private void Awake()
        {
            this.InputObserver = new InputObserver();
            this.InputObserver.Enable();
        }

        public void CreateState()
        {
            InputState inputState = new InputState();
            inputState.SetMovement(this.InputObserver.Player.Movement.ReadValue<Vector2>());
            inputState.SetLook(this.InputObserver.Player.Look.ReadValue<Vector2>());
            inputState.SetJump(this.InputObserver.Player.Jump.WasPressedThisFrame());
            inputState.SetSprint(this.InputObserver.Player.Sprint.ReadValue<float>() > 0);

            this.InputState = inputState;
        }

        public void Update()
        {
            this.CreateState();
            this.InputStateChanged?.Invoke(this.InputState);
        }
    }

    public delegate void OnInputStateChanged(InputState inputState);
}