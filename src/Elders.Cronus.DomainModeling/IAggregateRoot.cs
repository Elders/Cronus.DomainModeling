namespace Elders.Cronus.DomainModeling
{
    public interface IAggregateRoot : IAmEventSourced, IHaveState<IAggregateRootState>
    {
        int Revision { get; }
    }
}