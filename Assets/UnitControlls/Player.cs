using Abilities;
using Interaction;
using Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

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


        [Header("References / Ability Display")]
        [SerializeField] private Image abilityImage;
        public Image AbilityImage { get { return abilityImage; } }

        [SerializeField] private Text abilityDescription;
        public Text AbilityDescription { get { return abilityDescription; } }






        public Ability Stored { get; private set; }
        public OnAbilityChanged AbilityChanged { get; set; }


        private void Start()
        {
            this.SetAbility(null);
            this.InputController.InputStateChanged += this.InputChanged;
        }


        public void SetAbility(Ability ability)
        {
            this.Stored = ability;
            this.AbilityChanged?.Invoke(this.Stored);

            if (this.Stored == null)
            {
                this.AbilityDescription.gameObject.SetActive(false);
                this.AbilityImage.gameObject.SetActive(false);
            }
            else
            {
                this.AbilityDescription.gameObject.SetActive(true);
                this.AbilityImage.gameObject.SetActive(true);

                this.AbilityDescription.text = this.Stored.Description.Aggregate((a, b) => $"{a}{Environment.NewLine}{b}");
                this.AbilityImage.sprite = this.Stored.PreviewSprite.Sprite;
                this.AbilityImage.color = this.Stored.PreviewSprite.Color;
            }
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
