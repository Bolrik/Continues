using UnityEngine;

namespace Abilities
{
    // Provides Extra Stats for Movement
    [CreateAssetMenu(fileName = "New Movement Ability", menuName = "Movement/Movement Ability")]
    public class MovementAbility : Ability
    {
        [SerializeField] private int jumpCount = 0;
        public int JumpCount { get { return jumpCount; } }

        [SerializeField] private float sprintSpeed = 0;
        public float SprintSpeed { get { return sprintSpeed; } }

        [SerializeField] private float walkSpeed = 0;
        public float WalkSpeed { get { return walkSpeed; } }

        [SerializeField] private int stableSlopeAngle = 0;
        public int StableSlopeAngle { get { return stableSlopeAngle; } }

    }
}