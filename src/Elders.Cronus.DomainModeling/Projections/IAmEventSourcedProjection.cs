using System.Threading.Tasks;

namespace Elders.Cronus.Projections;

/// <summary>
/// Specifies that a projection supports event sourcing.
/// </summary>
public interface IAmEventSourcedProjection
{
    Task ReplayEventAsync(IEvent @event);
}
