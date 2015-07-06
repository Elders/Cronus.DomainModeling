namespace Elders.Cronus.DomainModeling
{
    public interface IHaveState<out TState>
    {
        TState State { get; }
    }
}