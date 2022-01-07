namespace Movement
{
    public interface IInputStateProvider
    {
        OnInputStateChanged InputStateChanged { get; set; }
    }
}