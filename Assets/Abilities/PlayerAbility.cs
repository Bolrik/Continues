using UnityEngine;

namespace Abilities
{
    // Provides Extra Stats for Movement
    [CreateAssetMenu(fileName = "New Player Ability", menuName = "Abilities/Player Ability")]
    public class PlayerAbility : Ability
    {
        [SerializeField] private bool allowBarrierPhasing;
        public bool AllowBarrierPhasing { get { return allowBarrierPhasing; } }

        [SerializeField] private bool allowGrabbing;
        public bool AllowGrabbing { get { return allowGrabbing; } }

        [SerializeField] private float interactionRange;
        public float InteractionRange { get { return interactionRange; } }


    }
}