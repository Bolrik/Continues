using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    [CreateAssetMenu(fileName = "New Movement Config", menuName = "Movement/Movement Config")]
    public class MovementConfig : ScriptableObject
    {
        [SerializeField] private float gravity = 9.81f;
        public float Gravity { get { return gravity; } }


        [Header("Movement")]
        [SerializeField] private float walkSpeed = 4f;
        public float WalkSpeed { get { return walkSpeed; } }

        [SerializeField] private float sprintSpeed = 6f;
        public float SprintSpeed { get { return sprintSpeed; } }

        [SerializeField] private float acceleration = 10f;
        public float Acceleration { get { return acceleration; } }

        [SerializeField] private float movementMultiplier = 10f;
        public float MovementMultiplier { get { return movementMultiplier; } }


        [Header("Drag")]
        [SerializeField] private float drag = 6f;
        public float Drag { get { return drag; } }

        [SerializeField] private float jumpDrag = 2f;
        public float JumpDrag { get { return jumpDrag; } }


        [Header("Jumping")]
        [SerializeField] private float jumpForce = 30f;
        public float JumpForce { get { return jumpForce; } }

        [SerializeField] private float jumpControl = 0.4f;
        public float JumpControl { get { return jumpControl; } }

        [SerializeField] private float fallingForce = 5;
        public float FallingForce { get { return fallingForce; } }

        [SerializeField] private float jumpMemoryTime = .2f;
        public float JumpMemoryTime { get { return jumpMemoryTime; } }

        [SerializeField] private float coyoteTime = .1f;
        public float CoyoteTime { get { return coyoteTime; } }

        [SerializeField] private int jumpCount = 1;
        public int JumpCount { get { return jumpCount; } }




        [Header("Slopes")]
        [SerializeField] private float stableSlopeAngle = 30f;
        public float StableSlopeAngle { get { return stableSlopeAngle; } }

    }
}