namespace Abilities
{
    public interface IAbilityStore
    {
        Ability Stored { get; }

        OnAbilityChanged AbilityChanged { get; set; }
    }

    public delegate void OnAbilityChanged(Ability ability);
}