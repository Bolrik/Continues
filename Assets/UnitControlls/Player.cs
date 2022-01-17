using Abilities;
using GameManagement;
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


        [Header("References / Abilities")]
        [SerializeField] private Image abilityImage;
        public Image AbilityImage { get { return abilityImage; } }

        [SerializeField] private Text abilityDescription;
        public Text AbilityDescription { get { return abilityDescription; } }

        [SerializeField] private Collider barrierCollider;
        public Collider BarrierCollider { get { return barrierCollider; } }

        [SerializeField] private LayerMask barrierLayer;
        public LayerMask BarrierLayer { get { return barrierLayer; } }





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

            this.SetAbilityEffect();
        }

        private void SetAbilityEffect()
        {
            if (!(this.Stored is PlayerAbility playerAbility))
            {
                // Set Ability Defaults

                this.SetDisableBarrierState(false);
                return;
            }

            this.SetDisableBarrierState(playerAbility.DisableBarrier);
        }


        private void SetDisableBarrierState(bool value)
        {
            // Inverted logic...

            this.BarrierCollider.enabled = !value;

            if (value)
            {
                // Enable Barrier Is Ground
                this.Movement.GroundLayer &= ~this.BarrierLayer;
            }
            else
            {
                // Disable Barrier Is Ground
                this.Movement.GroundLayer |= this.BarrierLayer;
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

            if (inputState.Back)
            {
                LevelLoader.Instance.Start(GameScene.GameEntry);
            }
        }
    }
}
