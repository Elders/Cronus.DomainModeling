using System.Collections.Generic;

namespace Elders.Cronus.Projections;

/// <summary>
/// Specifies that a projection supports event sourcing.
/// </summary>
public interface IAmEventSourcedProjection
{
    void ReplayEvents(IEnumerable<IEvent> events);
}
