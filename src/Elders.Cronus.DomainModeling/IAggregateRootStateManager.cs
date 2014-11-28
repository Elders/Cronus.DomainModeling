using System.Collections.Generic;

namespace Elders.Cronus.DomainModeling
{
    public interface IAggregateRootStateManager
    {
        IAggregateRootState State { get; }
        void BuildStateFromHistory(List<IEvent> events, int revision);
    }
}