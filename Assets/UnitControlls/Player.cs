using Abilities;
using GameManagement;
using Interaction;
using Levels;
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


        [Header("References / Level Info")]
        [SerializeField] private LevelEndScreen levelEndScreen;
        public LevelEndScreen LevelEndScreen { get { return levelEndScreen; } }

        [SerializeField] private Text currentTime;
        public Text CurrentTime { get { return currentTime; } }

        [SerializeField] private Text bestTime;
        public Text BestTime { get { return bestTime; } }



        public Ability Ability { get; private set; }
        public OnAbilityChanged AbilityChanged { get; set; }

        IGrabObject GrabObject { get; set; }


        private float LevelTime { get; set; }
        private bool IsGameOver { get; set; }
        private bool IsPaused { get; set; }



        private void Awake()
        {
            LevelManager.Instance.SetActivePlayer(this);
            GameSettings.Instance.ShowCursor = false;
            float levelBest = LevelManager.Instance.GetLevelTime();
            this.BestTime.text = Assistance.FloatToTimeString(levelBest);
        }

        private void Start()
        {
            this.LevelEndScreen.Hide();
            this.SetAbility(null);

            this.InputController.InputStateChanged += this.InputChanged;
        }

        private void Update()
        {
            if (this.IsGameOver)
                return;

            this.LevelTime += Time.deltaTime;

            
            this.CurrentTime.text = Assistance.FloatToTimeString(this.LevelTime);
        }

        private void LateUpdate()
        {
            if (this.GrabObject != null)
            {
                this.GrabObject.Rigidbody.angularVelocity = Vector3.Lerp(this.GrabObject.Rigidbody.angularVelocity, Vector3.zero, .6f);
                this.GrabObject.Rigidbody.velocity = Vector3.Lerp(this.GrabObject.Rigidbody.velocity, Vector3.zero, .6f);


                Vector3 target = this.Head.position + this.Head.forward * 2.5f;

                Vector3 curGrabPoint = this.GrabObject.GrabPoint.position;
                // Vector3 newGrabPoint = Vector3.Lerp(target, curGrabPoint, .1f);

                Vector3 grabPointDelta = target - curGrabPoint;

                //this.GrabObject.Rigidbody.transform.position += grabPointDelta;
                this.GrabObject.Rigidbody.AddForce(grabPointDelta * 2f, ForceMode.Impulse);
            }
        }


        public void SetAbility(Ability ability)
        {
            this.Ability = ability;
            this.AbilityChanged?.Invoke(this.Ability);

            this.SetAbilityText();
            this.SetAbilityEffect();
        }

        private void SetAbilityText()
        {
            if (this.Ability == null)
            {
                this.AbilityDescription.gameObject.SetActive(false);
                this.AbilityImage.gameObject.SetActive(false);
            }
            else
            {
                this.AbilityDescription.gameObject.SetActive(true);
                this.AbilityImage.gameObject.SetActive(true);

                this.AbilityDescription.text = this.Ability.Description.Aggregate((a, b) => $"{a}{Environment.NewLine}{b}");
                this.AbilityImage.sprite = this.Ability.PreviewSprite.Sprite;
                this.AbilityImage.color = this.Ability.PreviewSprite.Color;
            }
        }

        private void SetAbilityEffect()
        {
            if (!(this.Ability is PlayerAbility playerAbility))
            {
                // Set Ability Defaults

                this.SetAllowBarrierPhasingState(false);
                return;
            }

            this.SetAllowBarrierPhasingState(playerAbility.AllowBarrierPhasing);
        }


        private void SetAllowBarrierPhasingState(bool value)
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

        private void SetAllowGrabbingState(bool value)
        {

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

        private void CheckSpecial()
        {
            if (!(this.Ability is PlayerAbility playerAbility))
            {
                return;
            }


            this.TryGrab(playerAbility);
        }


        private bool TryGrab(PlayerAbility playerAbility)
        {
            if (this.GrabObject == null && playerAbility.AllowGrabbing)
            {
                Ray ray = new Ray(this.Head.position, this.Head.forward);
                RaycastHit[] hits = Physics.RaycastAll(ray, this.InteractionRange);

                foreach (var hit in hits)
                {
                    IGrabObject grabObject = hit.transform.GetComponentInParent<IGrabObject>();

                    if (grabObject == null || grabObject.Rigidbody.mass > this.Movement.Body.mass) continue;

                    this.GrabObject = grabObject;
                    this.GrabObject.IsGrab(true);

                    return true;
                }
            }
            else
            {
                this.GrabObject.IsGrab(false);
                this.GrabObject = null;
            }

            return false;
        }


        void InputChanged(InputState inputState)
        {
            if (inputState.Activate)
            {
                this.CheckInteraction();
            }

            if (inputState.Special)
            {
                this.CheckSpecial();
            }

            if (inputState.Back)
            {
                if ((Application.isEditor && inputState.Sprint) || !Application.isEditor)
                    LevelManager.Instance.Start(null);
            }

            if (inputState.Restart)
            {
                LevelManager.Instance.Restart();
            }
        }

        public void GameOver()
        {
            if (this.IsGameOver)
                return;

            this.IsGameOver = true;

            GameSettings.Instance.ShowCursor = true;
            LevelManager.Instance.UpdateLevelTime(this.LevelTime);

            this.LevelEndScreen.Show(this.LevelTime);
        }
    }
}
