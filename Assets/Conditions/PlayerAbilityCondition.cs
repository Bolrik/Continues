using UnityEngine;

namespace Conditions
{
    [System.Serializable]
    public class PlayerAbilityCondition : Condition
    {
        [SerializeField] private PlayerAbilityConditionType playerProgressConditionType;
        public PlayerAbilityConditionType PlayerProgressConditionType { get { return playerProgressConditionType; } }


        public override bool Check()
        {
            return false;
        }
    }

    public enum PlayerAbilityConditionType
    {
        AbilityHaste
    }
}