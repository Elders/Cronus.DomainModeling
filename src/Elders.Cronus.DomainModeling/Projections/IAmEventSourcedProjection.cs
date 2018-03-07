using System.Collections.Generic;

namespace Elders.Cronus.DomainModeling.Projections
{
    public interface IAmEventSourcedProjection
    {
        void ReplayEvents(IEnumerable<IEvent> events);
    }
}
