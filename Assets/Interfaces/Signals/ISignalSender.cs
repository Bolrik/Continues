namespace Signals
{
    public interface ISignalBase
    {
        SignalChannel SignalChannel { get; }
    }

    public interface ISignalSender : ISignalBase
    {

    }
    
    public interface ISignalReceiver : ISignalBase
    {

    }
}