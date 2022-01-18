namespace Abilities
{
    public interface IAbilityStore
    {
        Ability Ability { get; }

        OnAbilityChanged AbilityChanged { get; set; }
    }

    public delegate void OnAbilityChanged(Ability ability);
}