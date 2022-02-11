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

        [SerializeField] private Transform screenShakeTarget;
        public Transform ScreenShakeTarget { get { return screenShakeTarget; } }

        [SerializeField] private Image interactionIconImage;
        public Image InteractionIconImage { get { return interactionIconImage; } }

        [Header("References / Audio")]
        [SerializeField] private AudioSource audioSource;
        public AudioSource AudioSource { get { return audioSource; } }

        [SerializeField] private AudioSource walkingAudioSource;
        public AudioSource WalkingAudioSource { get { return walkingAudioSource; } }



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


        #region Damage
        [Header("References / Damage")]
        [SerializeField] private HealthBarContainer health;
        public HealthBarContainer Health { get { return health; } }
        #endregion

        [Header("Settings")]
        [SerializeField] private PlayerConfig config;
        public PlayerConfig Config { get { return config; } }
        
        public Ability Ability { get; private set; }
        public OnAbilityChanged AbilityChanged { get; set; }

        float InteractionBufferTime { get; set; }

        IGrabObject GrabObject { get; set; }

        IInteractable Interactable { get; set; }


        private float LevelTime { get; set; }
        private bool IsGameOver { get; set; }
        private bool IsPaused { get; set; }


        #region Default Values
        float InteractionRangeDefault => this.CalculateInteractionRangeDefault();


        private float CalculateInteractionRangeDefault()
        {
            float toReturn = 0;
            if (this.Config != null) toReturn += this.Config.InteractionRange;
            if (this.Ability is PlayerAbility playerAbility) toReturn = Mathf.Max(toReturn, playerAbility.InteractionRange);
            return toReturn;
        }
        #endregion



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
            this.Health.Show(LevelManager.Instance.GetLevel());
            
            this.SetAbility(null);

            this.InputController.InputStateChanged += this.InputChanged;
        }

        private void Update()
        {
            this.UpdateScreenShake();

            if (this.IsGameOver)
                return;

            this.UpdateLevelTime();
            
            this.Health.Tick(Time.deltaTime);

            this.UpdateInteractionBuffer();
            

            this.CheckInteraction();
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
            float velocity = this.Movement.Body.velocity.magnitude;
            if (this.Movement.IsGrounded && velocity > 1)
            {
                this.WalkingAudioSource.UnPause();
                this.WalkingAudioSource.pitch = Mathf.Clamp(0.8f + (.4f * (velocity - 4.2f) / 5.8f), .9f, 1.2f);
            }
            else
            {
                this.WalkingAudioSource.Pause();
            }
        }


        private void UpdateLevelTime()
        {
            this.LevelTime += Time.deltaTime;
            this.CurrentTime.text = Assistance.FloatToTimeString(this.LevelTime);
        }

        private void UpdateInteractionBuffer()
        {
            this.InteractionBufferTime = Mathf.Clamp(this.InteractionBufferTime - Time.deltaTime, 0, this.Config.InteractionBuffer);
            if (this.InteractionBufferTime > 0)
                this.Interact();
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
            if (Physics.Raycast(ray, out RaycastHit hit, this.InteractionRangeDefault))
            {
                IInteractable interactable = hit.transform.GetComponentInParent<IInteractable>();

                if (interactable == null)
                {
                    this.SetInteractable(null);
                    return;
                }

                this.SetInteractable(interactable);
            }
            else
                this.SetInteractable(null);

            //Ray ray = new Ray(this.Head.position, this.Head.forward);
            //RaycastHit[] hits = Physics.RaycastAll(ray, this.InteractionRange);

            //for (int i = 0; i < hits.Length; i++)
            //{
            //    var hit = hits[i];
            //    IInteractable interactable = hit.transform.GetComponentInParent<IInteractable>();

            //    if (interactable == null)
            //        continue;

            //    this.Interactable = interactable;
            //    Debug.Log(i);

            //    return;
            //}
        }

        private void SetInteractable(IInteractable interactable)
        {
            this.Interactable = interactable;

            if (this.Interactable == null)
            {
                this.InteractionIconImage.enabled = false;
            }
            else
            {
                this.InteractionIconImage.enabled = true;
                this.InteractionIconImage.sprite = this.Interactable.Icon;
            }
        }

        private void Interact()
        {
            if (this.Interactable == null)
                return;

            this.Interactable.Activate();
            this.InteractionBufferTime = 0;

            if (this.Interactable.OnUseSound != null)
                this.AudioSource.PlayOneShot(this.Interactable.OnUseSound, 1);
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
                RaycastHit[] hits = Physics.RaycastAll(ray, this.InteractionRangeDefault);

                for (int i = 0; i < hits.Length; i++)
                {
                    var hit = hits[i];

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
                this.InteractionBufferTime = this.Config.InteractionBuffer;
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

        public void GameOver(bool isVictory)
        {
            if (this.IsGameOver)
                return;

            this.IsGameOver = true;

            GameSettings.Instance.ShowCursor = true;

            if (isVictory)
                LevelManager.Instance.UpdateLevelTime(this.LevelTime);

            this.LevelEndScreen.Show(this.LevelTime, isVictory);
        }


        public void Damage()
        {
            if (!this.Health.Damage())
            {
                return;
            }

            // Screen Shake
            this.ScreenShakeTarget.localEulerAngles = 
                new Vector3(this.ScreenShakeTarget.localEulerAngles.x, this.ScreenShakeTarget.localEulerAngles.y, 13);

            this.Movement.SetDamageFrame();

            if (this.Health.Health == 0)
                this.GameOver(false);
        }


        private void UpdateScreenShake()
        {
            if (this.ScreenShakeTarget.localEulerAngles.z != 0)
            {
                this.ScreenShakeTarget.localEulerAngles =
                    Vector3.Lerp(
                        this.ScreenShakeTarget.localEulerAngles,
                        new Vector3(this.ScreenShakeTarget.localEulerAngles.x, this.ScreenShakeTarget.localEulerAngles.y, 0),
                        Time.deltaTime * 8f);
            }
        }
    }
}
