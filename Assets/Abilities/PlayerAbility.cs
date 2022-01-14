using UnityEngine;

namespace Abilities
{
    // Provides Extra Stats for Movement
    [CreateAssetMenu(fileName = "New Player Ability", menuName = "Abilities/Player Ability")]
    public class PlayerAbility : Ability
    {
        [SerializeField] private bool disableBarrier;
        public bool DisableBarrier { get { return disableBarrier; } }
    }
}