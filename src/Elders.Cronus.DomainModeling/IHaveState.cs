namespace Elders.Cronus
{
    public interface IHaveState<out TState>
    {
        TState State { get; }
    }
}