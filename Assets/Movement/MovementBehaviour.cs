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


        [Header("Config")]
        [SerializeField] private MovementConfig config;
        public MovementConfig Config { get { return this.config; } private set { this.config = value; } }

        [SerializeField] private MovementAbility ability;
        public MovementAbility Ability { get { return this.ability; } private set { this.ability = value; } }


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
            this.UpdateSpeed();
            this.UpdateInput();

            this.SlopeMoveDirection = Vector3.ProjectOnPlane(this.MoveDirection, this.SlopeHit.normal);

            this.JumpMemoryTime = Mathf.Clamp(this.JumpMemoryTime += Time.deltaTime, 0, this.Config.JumpMemoryTime);
            this.CoyoteTime = Mathf.Clamp(this.CoyoteTime += Time.deltaTime, 0, this.Config.CoyoteTime);
        }

        private void FixedUpdate()
        {
            this.UpdatePlayer();
        }



        // Use Input State
        void UpdateInput()
        {
            this.MoveDirection = this.Root.forward * this.InputState.Movement.y + this.Root.right * this.InputState.Movement.x;
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

            Debug.Log(this.WalkSpeedDefault);
            Debug.Log(this.SprintSpeedDefault);
            Debug.Log(this.Config?.WalkSpeed);
            Debug.Log(this.Ability?.WalkSpeed);
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
                force = this.MoveDirection.normalized * this.MoveSpeed * this.Config.MovementMultiplier * this.Config.JumpControl;

                force.y -= this.Config.Gravity;
                
                if (this.Body.velocity.y < 0)
                {
                    // Falling? Add more Gravity!!!!!!!1!!eins
                    force.y -= this.Config.FallingForce;
                }
            }

            this.Body.AddForce(force);

            this.DoPostUpdatePlayer = true;
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

            if (jumpCountFirst)
            {
                if ((jumpInput || jumpMemory) && (isGrounded || coyoteGround))
                {
                    Debug.Log($"JI: {jumpInput}, JM: {jumpMemory}, IG: {isGrounded}, CG: {coyoteGround}");
                    return true;
                }
            }
            else
            {
                if (jumpCount && jumpInput)
                {
                    return true;
                }
            }

            //if ((jumpInput || jumpMemory) && (isGrounded || coyoteGround))
            //{
            //    Debug.Log($"JI: {jumpInput}, JM: {jumpMemory}, IG: {isGrounded}, CG: {coyoteGround}");
            //    return true;
            //}

            return false;
        }

        // J-j-j-j-j-Jump
        void Jump()
        {
            this.Body.velocity = new Vector3(this.Body.velocity.x, 0, this.Body.velocity.z);
            this.Body.AddForce(this.transform.up * this.Config.JumpForce, ForceMode.Impulse);

            this.CoyoteTime = this.Config.CoyoteTime;
            this.JumpMemoryTime = this.Config.JumpMemoryTime;
            this.JumpCount--;
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
            if (this.GroundCheck.CheckNonAloc(3))
            {
                this.IsGrounded = true;

                // Just recently jumped...
                if (this.Body.velocity.y > 0)
                {
                    // Prevent Coyote Time to reset midair
                    this.CoyoteTime = this.Config.CoyoteTime;
                }
                else
                {
                    this.CoyoteTime = 0;
                    this.JumpCount = this.JumpCountDefault;
                }

                return;
            }

            this.IsGrounded = false;
            return;

            //Collider[] colliders = new Collider[3];
            //Physics.OverlapSphereNonAlloc(this.GroundTransform.position, .1f, colliders);

            //for (int i = 0; i < colliders.Length; i++)
            //{
            //    Collider collider = colliders[i];
            //    if (collider == null ||
            //        collider.transform.IsChildOf(this.transform))
            //        continue;

            //    this.IsGrounded = true;
            //    this.CoyoteTime = 0;
            //    return;
            //}

            //this.IsGrounded = false;
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
            if (Physics.Raycast(this.transform.position, Vector3.down, out slopeHit, 1.5f))
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
        }

        public void UpdateInputState(InputState inputState)
        {
            this.InputState = this.InputState.Combine(inputState);
            if (inputState.Jump)
                this.JumpMemoryTime = 0;
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
                Debug.Log($"IS NOT MA>> {ability}");
        }
    }
}
