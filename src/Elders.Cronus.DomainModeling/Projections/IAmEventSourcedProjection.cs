using System.Collections.Generic;

namespace Elders.Cronus.Projections
{
    public interface IAmEventSourcedProjection
    {
        void ReplayEvents(IEnumerable<IEvent> events);
    }
}
