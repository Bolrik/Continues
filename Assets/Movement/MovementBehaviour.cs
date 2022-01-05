using UnityEngine;

namespace Movement
{
    public class MovementBehaviour : MonoBehaviour, IInputStateReceiver
    {
        [Header("References")]
        [SerializeField] private IInputStateProvider inputStateProvider;
        private IInputStateProvider InputStateProvider { get { return this.inputStateProvider; } set { this.inputStateProvider = value; } }

        [Header("General")]
        [SerializeField] private Transform root;
        private Transform Root { get { return this.root; } set { this.root = value; } }

        [Header("Config")]
        [SerializeField] private MovementConfig config;
        public MovementConfig Config { get { return config; } private set { config = value; } }

        [Header("Ground Detection")]
        [SerializeField] private Transform groundTransform;
        public Transform GroundTransform { get { return groundTransform; } }


        float MoveSpeed { get; set; }

        Rigidbody Body { get; set; }
        Vector3 MoveDirection { get; set; }
        Vector3 SlopeMoveDirection { get; set; }
        RaycastHit SlopeHit { get; set; }
        InputState InputState { get; set; }


        bool DoPostUpdatePlayer { get; set; } = true;
        bool IsOnSlope { get; set; }
        bool IsSlopeMovement { get; set; }
        float LastJumpInput { get; set; } = 0;

        public bool IsGrounded { get; private set; }


        private void Start()
        {
            this.InputStateProvider = this.GetComponent<IInputStateProvider>();

            if (this.InputStateProvider == null)
            {
                this.enabled = false;
                return;
            }

            this.InputStateProvider.InputStateChanged += this.UpdateInputState;

            this.Body = this.GetComponent<Rigidbody>();
            this.Body.freezeRotation = true;
        }

        private void Update()
        {
            this.PostUpdatePlayer();
            this.UpdateSpeed();
            this.UpdateInput();
            this.SlopeMoveDirection = Vector3.ProjectOnPlane(this.MoveDirection, this.SlopeHit.normal);

            this.LastJumpInput += Time.deltaTime;
        }

        private void FixedUpdate()
        {
            this.UpdatePlayer();
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
                    this.IsSlopeMovement = angle <= this.Config.StableSlopeAngle;
                }
            }

            this.IsOnSlope = false;
        }

        // Use Input State
        void UpdateInput()
        {
            this.MoveDirection = this.Root.forward * this.InputState.Movement.y + this.Root.right * this.InputState.Movement.x;
        }

        // J-j-j-j-j-Jump
        void Jump()
        {
            this.Body.velocity = new Vector3(this.Body.velocity.x, 0, this.Body.velocity.z);
            this.Body.AddForce(this.transform.up * this.Config.JumpForce, ForceMode.Impulse);
        }

        void UpdateSpeed()
        {
            if (this.InputState.Sprint && this.IsGrounded)
            {
                this.MoveSpeed = Mathf.Lerp(this.MoveSpeed, this.Config.Sprint, this.Config.Acceleration * Time.deltaTime);
            }
            else
            {
                this.MoveSpeed = Mathf.Lerp(this.MoveSpeed, this.Config.Walk, this.Config.Acceleration * Time.deltaTime);
            }
        }

        void UpdateDrag()
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

        void UpdatePlayer()
        {
            Vector3 force = Vector3.zero;
            bool jump = false;
            // Do Jump
            if (this.CanJump())
            {
                this.Jump();
                jump = true;
            }

            // Ground not Slope
            if (this.IsGrounded && !this.IsSlopeMovement)
            {
                force = this.MoveDirection.normalized * this.MoveSpeed * this.Config.MovementMultiplier;
                // this.Body.AddForce(this.MoveDirection.normalized * this.MoveSpeed * this.Config.MovementMultiplier, ForceMode.Acceleration);
            }
            // Ground and Slope
            else if (this.IsGrounded && this.IsSlopeMovement)
            {
                force = this.SlopeMoveDirection.normalized * this.MoveSpeed * this.Config.MovementMultiplier;
                // this.Body.AddForce(this.SlopeMoveDirection.normalized * this.MoveSpeed * this.Config.MovementMultiplier, ForceMode.Acceleration);
            }
            // Jump or Fall
            else if (!this.IsGrounded)
            {
                force = this.MoveDirection.normalized * this.MoveSpeed * this.Config.MovementMultiplier * this.Config.JumpControl;

                force.y -= this.Config.Gravity;
                // this.Body.AddForce(this.MoveDirection.normalized * this.MoveSpeed * this.Config.MovementMultiplier * this.Config.JumpControl, ForceMode.Acceleration);
                if (this.Body.velocity.y < 0)
                {
                    // Falling? Add more Gravity!!!!!!!1!!eins
                    force.y -= this.Config.FallingForce;
                }
            }

            this.Body.AddForce(force);

            this.DoPostUpdatePlayer = true;
        }

        bool CanJump()
        {
            if ((this.InputState.Jump || this.LastJumpInput <= this.Config.JumpMemoryTime) && this.IsGrounded)
                return true;

            return false;
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

        private void UpdateGrounded()
        {
            Collider[] colliders = new Collider[3];
            Physics.OverlapSphereNonAlloc(this.GroundTransform.position, .1f, colliders);
            for (int i = 0; i < colliders.Length; i++)
            {
                Collider collider = colliders[i];
                if (collider == null ||
                    collider.transform.IsChildOf(this.transform))
                    continue;

                this.IsGrounded = true;
                return;
            }

            this.IsGrounded = false;
        }

        public void UpdateInputState(InputState inputState)
        {
            this.InputState = this.InputState.Combine(inputState);
            if (inputState.Jump)
                this.LastJumpInput = 0;
        }
    }
}
