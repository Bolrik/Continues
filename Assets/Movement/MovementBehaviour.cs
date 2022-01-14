using Abilities;
using UnityEngine;
using Utils;

namespace Movement
{
    public class MovementBehaviour : MonoBehaviour, IInputStateReceiver
    {
        [Header("References")]
        [SerializeField] private IInputStateProvider inputStateProvider;
        private IInputStateProvider InputStateProvider { get { return this.inputStateProvider; } set { this.inputStateProvider = value; } }

        [SerializeField] private IAbilityStore abilityStore;
        private IAbilityStore AbilityStore { get { return this.abilityStore; } set { this.abilityStore = value; } }

        [SerializeField] private Transform root;
        private Transform Root { get { return this.root; } set { this.root = value; } }


        [Header("Ground Detection")]
        [SerializeField] private SphereCollisionCheck groundCheck;
        public SphereCollisionCheck GroundCheck { get { return this.groundCheck; } }

        [SerializeField] private LayerMask groundLayer;
        public LayerMask GroundLayer { get { return groundLayer; } set { groundLayer = value; } }



        [Header("Config")]
        [SerializeField] private MovementConfig config;
        public MovementConfig Config { get { return this.config; } private set { this.config = value; } }

        [SerializeField] private MovementAbility ability;
        public MovementAbility Ability { get { return this.ability; } private set { this.ability = value; } }


        #region Debug
        //[Header("Temp Debug")]
        //[SerializeField] private bool isGrounded;
        //public bool IsGrounded { get { return isGrounded; } set { isGrounded = value; } }

        //[SerializeField] private float jumpMemoryTime;
        //public float JumpMemoryTime { get { return jumpMemoryTime; } set { jumpMemoryTime = value; } }

        //[SerializeField] private float coyoteTime;
        //public float CoyoteTime { get { return coyoteTime; } set { coyoteTime = value; } }

        //[SerializeField] private int jumpCount;
        //public int JumpCount { get { return jumpCount; } set { jumpCount = value; } }

        //[SerializeField] private bool isOnSlope;
        //public bool IsOnSlope { get { return isOnSlope; } set { isOnSlope = value; } }

        //[SerializeField] private bool isSlopeMovement;
        //public bool IsSlopeMovement { get { return isSlopeMovement; } set { isSlopeMovement = value; } }
        #endregion

        // Jumping
        float JumpMemoryTime { get; set; } = 1; // Jump Memory
        float CoyoteTime { get; set; } = 1; // Coyote Time
        int JumpCount { get; set; } // Multi Jump


        InputState InputState { get; set; }
        Rigidbody Body { get; set; }
        bool DoPostUpdatePlayer { get; set; } = true;

        // Movement
        Vector3 MoveDirection { get; set; }
        Vector3 SlopeMoveDirection { get; set; }
        float MoveSpeed { get; set; }
        public bool IsGrounded { get; private set; }



        // Slopes
        RaycastHit SlopeHit { get; set; }
        bool IsOnSlope { get; set; }
        bool IsSlopeMovement { get; set; }

        // Default Values
        int JumpCountDefault => this.CalculateJumpCountDefault();
        float WalkSpeedDefault => this.CalculateWalkSpeedDefault();
        float SprintSpeedDefault => this.CalculateSprintSpeedDefault();
        float StableSlopeAngleDefault => this.CalculateStableSlopeAngleDefault();
        float JumpForceDefault => this.CalculateJumpForceDefault();
        float JumpControlDefault => this.CalculateJumpControlDefault();
        float FallingForceDefault => this.CalculateFallingForceDefault();

        #region Auto Property Calculations

        private float CalculateWalkSpeedDefault()
        {
            float toReturn = 0;
            if (this.Config != null) toReturn += this.Config.WalkSpeed;
            if (this.Ability != null) toReturn += this.Ability.WalkSpeed;
            return toReturn;
        }
        private float CalculateSprintSpeedDefault()
        {
            float toReturn = 0;
            if (this.Config != null) toReturn += this.Config.SprintSpeed;
            if (this.Ability != null) toReturn += this.Ability.SprintSpeed;
            return toReturn;
        }
        private int CalculateJumpCountDefault()
        {
            int toReturn = 0;
            if (this.Config != null) toReturn += this.Config.JumpCount;
            if (this.Ability != null) toReturn += this.Ability.JumpCount;
            return toReturn;
        }
        private float CalculateStableSlopeAngleDefault()
        {
            float toReturn = 0;
            if (this.Config != null) toReturn = Mathf.Max(toReturn, this.Config.StableSlopeAngle);
            if (this.Ability != null) toReturn = Mathf.Max(toReturn, this.Ability.StableSlopeAngle);
            return toReturn;
        }
        private float CalculateJumpForceDefault()
        {
            float toReturn = 0;
            if (this.Config != null) toReturn = Mathf.Max(toReturn, this.Config.JumpForce);
            if (this.Ability != null) toReturn = Mathf.Max(toReturn, this.Ability.JumpForce);
            return toReturn;
        }
        private float CalculateJumpControlDefault()
        {
            float toReturn = 0;
            if (this.Config != null) toReturn += this.Config.JumpControl;
            if (this.Ability != null) toReturn += this.Ability.JumpControl;
            return toReturn;
        }
        private float CalculateFallingForceDefault()
        {
            float toReturn = 0;
            if (this.Config != null) toReturn += this.Config.FallingForce;
            if (this.Ability != null) toReturn += this.Ability.FallingForce;
            return toReturn;
        }
        #endregion

        private void Start()
        {
            this.InputStateProvider = this.GetComponent<IInputStateProvider>();
            this.AbilityStore = this.GetComponent<IAbilityStore>();

            if (this.InputStateProvider == null)
            {
                this.enabled = false;
                return;
            }

            this.InputStateProvider.InputStateChanged += this.UpdateInputState;

            if (this.AbilityStore != null)
                this.AbilityStore.AbilityChanged += this.UpdateAbility;

            this.Body = this.GetComponent<Rigidbody>();
            this.Body.freezeRotation = true;
        }

        private void Update()
        {
            this.PostUpdatePlayer();
            //this.UpdateInput();
            //this.UpdateSpeed();

            //this.JumpMemoryTime = Mathf.Clamp(this.JumpMemoryTime += Time.deltaTime, 0, this.Config.JumpMemoryTime);
            //this.CoyoteTime = Mathf.Clamp(this.CoyoteTime += Time.deltaTime, 0, this.Config.CoyoteTime);
        }

        private void FixedUpdate()
        {
            this.UpdatePlayer();
        }


        // This should only be called once after every 'UpdatePlayer' (Physics Update) call
        private void PostUpdatePlayer()
        {
            if (!this.DoPostUpdatePlayer)
                return;

            this.UpdateGrounded();
            this.UpdateDrag();
            this.UpdateSlope();

            this.InputState = new InputState();
            this.DoPostUpdatePlayer = false;
        }

        // Check for Grounds
        private void UpdateGrounded()
        {
            if (this.GroundCheck.CheckNonAloc(5, this.GroundLayer))
            {
                this.IsGrounded = true;

                // Just recently jumped...
                if (this.Body.velocity.y <= 0.25f)
                {
                    this.CoyoteTime = 0;
                    this.JumpCount = this.JumpCountDefault;
                }

                return;
            }

            this.IsGrounded = false;
            return;
        }

        // Update Drag
        private void UpdateDrag()
        {
            if (this.IsGrounded)
            {
                this.Body.drag = this.Config.Drag;
            }
            else
            {
                this.Body.drag = this.Config.JumpDrag;
            }
        }

        // Check for Slopes
        private void UpdateSlope()
        {
            RaycastHit slopeHit;
            if (Physics.Raycast(this.transform.position, Vector3.down, out slopeHit, 1.5f, this.GroundLayer))
            {
                this.SlopeHit = slopeHit;
                this.IsOnSlope = this.SlopeHit.normal != Vector3.up;

                if (this.IsOnSlope)
                {
                    float angle = Vector3.Angle(Vector3.up, this.SlopeHit.normal);
                    this.IsSlopeMovement = angle <= this.StableSlopeAngleDefault;
                    return;
                }
            }

            this.IsOnSlope = false;
            this.IsSlopeMovement = false;
        }



        public void UpdateInputState(InputState inputState)
        {
            this.InputState = this.InputState.Combine(inputState);

            if (inputState.Jump)
                this.JumpMemoryTime = 0;

            this.UpdateInput();
            this.UpdateSpeed();
        }

        // Use Input State
        void UpdateInput()
        {
            this.MoveDirection = this.Root.forward * this.InputState.Movement.y + this.Root.right * this.InputState.Movement.x;
            this.SlopeMoveDirection = Vector3.ProjectOnPlane(this.MoveDirection, this.SlopeHit.normal);
        }

        // Update Speed depending on Grounded and Sprinting
        void UpdateSpeed()
        {
            if (this.InputState.Sprint && this.IsGrounded)
            {
                this.MoveSpeed = Mathf.Lerp(this.MoveSpeed, this.SprintSpeedDefault, this.Config.Acceleration * Time.deltaTime);
            }
            else
            {
                this.MoveSpeed = Mathf.Lerp(this.MoveSpeed, this.WalkSpeedDefault, this.Config.Acceleration * Time.deltaTime);
            }
        }


        private void UpdateAbility(Ability ability)
        {
            Debug.Log("Update Ability");

            if (ability == null)
            {
                Debug.Log($"NULL >> {ability}");
                this.Ability = null;
                return;
            }

            if (ability is MovementAbility movementAbility)
            {
                this.Ability = movementAbility;
                Debug.Log($"IS MA >> {ability} ->> {this.Ability}");
            }
            else
            {
                this.Ability = null;
                Debug.Log($"IS NOT MA>> {ability}");
            }
        }



        // Do Physics Stuff
        void UpdatePlayer()
        {
            Vector3 force = Vector3.zero;
            // Do Jump
            if (this.CanJump())
            {
                this.Jump();
            }

            // Ground not Slope
            if (this.IsGrounded && !this.IsSlopeMovement)
            {
                force = this.MoveDirection.normalized * this.MoveSpeed * this.Config.MovementMultiplier;
                force.y -= this.Config.Gravity * .01f;

                // On to steep Slope
                if (this.IsOnSlope)
                {
                    force.y -= this.Config.Gravity;
                }
            }
            // Ground and Slope
            else if (this.IsGrounded && this.IsSlopeMovement)
            {
                force = this.SlopeMoveDirection.normalized * this.MoveSpeed * this.Config.MovementMultiplier;
            }
            // Jump or Fall
            else if (!this.IsGrounded)
            {
                force = this.MoveDirection.normalized * this.MoveSpeed * this.Config.MovementMultiplier * this.JumpControlDefault;

                force.y -= this.Config.Gravity;

                if (this.Body.velocity.y < 0)
                {
                    // Falling? Add more Gravity!!!!!!!1!!eins
                    force.y -= this.FallingForceDefault;
                }
            }

            this.Body.AddForce(force);

            this.DoPostUpdatePlayer = true;
            this.JumpMemoryTime = Mathf.Clamp(this.JumpMemoryTime += Time.fixedDeltaTime, 0, this.Config.JumpMemoryTime);
            this.CoyoteTime = Mathf.Clamp(this.CoyoteTime += Time.fixedDeltaTime, 0, this.Config.CoyoteTime);
        }


        // Check whether the Player can jump or not (Coyote Time, Jump Memory...)
        bool CanJump()
        {
            bool jumpInput = this.InputState.Jump;
            bool jumpMemory = this.JumpMemoryTime < this.Config.JumpMemoryTime;
            bool isGrounded = this.IsGrounded;
            bool coyoteGround = this.CoyoteTime < this.Config.CoyoteTime;
            bool jumpCount = this.JumpCount > 0;
            bool jumpCountFirst = this.JumpCount == this.JumpCountDefault;

            if (jumpCount && (jumpInput || jumpMemory))
            {
                return true;
            }

            return false;
        }

        // J-j-j-j-j-Jump
        void Jump()
        {
            this.Body.velocity = new Vector3(this.Body.velocity.x, 0, this.Body.velocity.z);
            this.Body.AddForce(this.transform.up * this.JumpForceDefault, ForceMode.Impulse);

            this.CoyoteTime = this.Config.CoyoteTime;
            this.JumpMemoryTime = this.Config.JumpMemoryTime;
            this.JumpCount--;
        }

    }
}
