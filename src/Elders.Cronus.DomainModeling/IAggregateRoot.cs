namespace Elders.Cronus;

public interface IAggregateRoot : IAmEventSourced, IHaveState<IAggregateRootState>, IUnderstandPublishedLanguage
{
    int Revision { get; }
}
