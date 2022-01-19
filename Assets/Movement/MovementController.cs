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
        bool SpecialCache { get; set; }
        bool BackCache { get; set; }
        bool RestartCache { get; set; }


        private void Awake()
        {
            this.InputObserver = new InputObserver();
            this.InputObserver.Enable();
            this.InputObserver.Player.Jump.started += this.JumpStarted;
            this.InputObserver.Player.Activate.started += this.ActivateStarted;
            this.InputObserver.Player.Special.started += this.SpecialStarted;
            this.InputObserver.Player.Back.started += this.BackStarted;
            this.InputObserver.Player.Restart.started += this.RestartStarted;
        }

        private void ActivateStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            this.ActivateCache = true;
        }

        private void SpecialStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            this.SpecialCache = true;
        }

        private void JumpStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            this.JumpCache = true;
        }

        private void BackStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            this.BackCache = true;
        }

        private void RestartStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            this.RestartCache = true;
        }

        public void CreateState()
        {
            InputState inputState = new InputState();
            inputState.SetMovement(this.InputObserver.Player.Movement.ReadValue<Vector2>());
            inputState.SetLook(this.InputObserver.Player.Look.ReadValue<Vector2>());
            inputState.SetSprint(this.InputObserver.Player.Sprint.ReadValue<float>() > 0);

            inputState.SetJump(this.JumpCache);
            inputState.SetActivate(this.ActivateCache);
            inputState.SetSpecial(this.SpecialCache);
            inputState.SetBack(this.BackCache);
            inputState.SetRestart(this.RestartCache);

            this.InputState = inputState;

            this.ActivateCache = false;
            this.SpecialCache = false;
            this.JumpCache = false;
            this.BackCache = false;
            this.RestartCache = false;
        }

        public void Update()
        {
            this.CreateState();
            this.InputStateChanged?.Invoke(this.InputState);
        }
    }

    public delegate void OnInputStateChanged(InputState inputState);
}