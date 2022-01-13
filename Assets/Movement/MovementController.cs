using Inputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class MovementController : MonoBehaviour, IInputStateProvider
    {
        private InputObserver InputObserver { get; set; }
        InputState InputState { get; set; }

        public OnInputStateChanged InputStateChanged { get; set; }

        bool JumpCache { get; set; }
        bool ActivateCache { get; set; }


        private void Awake()
        {
            this.InputObserver = new InputObserver();
            this.InputObserver.Enable();
            this.InputObserver.Player.Jump.started += this.Jump_started;
            this.InputObserver.Player.Activate.started += this.Activate_started;
        }

        private void Activate_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            Debug.Log("Act");
            this.ActivateCache = true;
        }

        private void Jump_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            Debug.Log("Jump");
            this.JumpCache = true;
        }

        public void CreateState()
        {
            InputState inputState = new InputState();
            inputState.SetMovement(this.InputObserver.Player.Movement.ReadValue<Vector2>());
            inputState.SetLook(this.InputObserver.Player.Look.ReadValue<Vector2>());
            inputState.SetSprint(this.InputObserver.Player.Sprint.ReadValue<float>() > 0);

            inputState.SetJump(this.JumpCache);
            inputState.SetActivate(this.ActivateCache);

            this.InputState = inputState;

            this.ActivateCache = false;
            this.JumpCache = false;
        }

        public void Update()
        {
            this.CreateState();
            this.InputStateChanged?.Invoke(this.InputState);
        }
    }

    public delegate void OnInputStateChanged(InputState inputState);
}