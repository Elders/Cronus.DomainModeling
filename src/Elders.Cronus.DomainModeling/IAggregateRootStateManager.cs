using System.Collections.Generic;

namespace Elders.Cronus.DomainModeling
{
    public interface IAggregateRootStateManager
    {
        IAggregateRootState State { get; }
        IAggregateRootState BuildStateFromHistory(List<IEvent> events, int revision);
    }
}