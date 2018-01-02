namespace Elders.Cronus
{
    public interface IAggregateRoot : IAmEventSourced, IHaveState<IAggregateRootState>
    {
        int Revision { get; }
    }
}