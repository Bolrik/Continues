namespace Abilities
{
    public interface IAbilityReceiver : IAbilityStore
    {
        Ability SwapAbility(Ability swapTo);
        void SetAbility(Ability ability);
    }
}