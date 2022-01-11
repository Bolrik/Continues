using Abilities;
using Interaction;
using Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnitControlls
{
    public class Player : MonoBehaviour, IAbilityReceiver
    {
        [Header("References")]
        [SerializeField] private MovementBehaviour movement;
        public MovementBehaviour Movement { get { return movement; } }

        [SerializeField] private MovementController inputController;
        public MovementController InputController { get { return inputController; } }

        [SerializeField] private Transform head;
        public Transform Head { get { return head; } }

        [SerializeField] private float interactionRange = 1.337f;
        public float InteractionRange { get { return interactionRange; } }






        public Ability Stored { get; private set; }
        public OnAbilityChanged AbilityChanged { get; set; }


        private void Start()
        {
            this.InputController.InputStateChanged += this.InputChanged;
        }


        public void SetAbility(Ability ability)
        {
            this.Stored = ability;
            this.AbilityChanged?.Invoke(this.Stored);
        }

        Ability IAbilityReceiver.SwapAbility(Ability swapTo)
        {
            return this.SwapAbility(swapTo);
        }



        private void CheckInteraction()
        {
            Ray ray = new Ray(this.Head.position, this.Head.forward);
            RaycastHit[] hits = Physics.RaycastAll(ray, this.InteractionRange);

            foreach (var hit in hits)
            {
                IInteractable interactable = hit.transform.GetComponentInParent<IInteractable>();

                if (interactable == null)
                    continue;

                interactable.Activate();

                return;
            }
        }

        void InputChanged(InputState inputState)
        {
            if (inputState.Activate)
            {
                this.CheckInteraction();
            }
        }
    }
}
