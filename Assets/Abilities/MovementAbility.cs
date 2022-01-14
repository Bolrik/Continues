using UnityEngine;

namespace Abilities
{
    // Provides Extra Stats for Movement
    [CreateAssetMenu(fileName = "New Movement Ability", menuName = "Abilities/Movement Ability")]
    public class MovementAbility : Ability
    {
        [SerializeField] private int jumpCount = 0;
        public int JumpCount { get { return jumpCount; } }

        [SerializeField] private float sprintSpeed = 0f;
        public float SprintSpeed { get { return sprintSpeed; } }

        [SerializeField] private float walkSpeed = 0f;
        public float WalkSpeed { get { return walkSpeed; } }

        [SerializeField] private int stableSlopeAngle = 0;
        public int StableSlopeAngle { get { return stableSlopeAngle; } }

        [SerializeField] private float jumpForce = 0f;
        public float JumpForce { get { return jumpForce; } }

        [SerializeField] private float jumpControl = 0f;
        public float JumpControl { get { return jumpControl; } }

        [SerializeField] private float fallingForce = 0f;
        public float FallingForce { get { return fallingForce; } }

    }
}