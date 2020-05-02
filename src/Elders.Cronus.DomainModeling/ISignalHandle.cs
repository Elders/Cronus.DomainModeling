namespace Elders.Cronus
{
    public interface ISignalHandle<in T>
        where T : ISignal
    {
        void Handle(T signal);
    }
}
