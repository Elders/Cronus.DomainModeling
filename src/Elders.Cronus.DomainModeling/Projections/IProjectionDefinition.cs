using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Elders.Cronus.Projections;

/// <summary>
/// Defines a projection that derives its state from one or more events. The framework calls
/// <see cref="GetProjectionIds"/> to fan an incoming event out to one or more projection identities,
/// then invokes <see cref="ApplyAsync"/> on each loaded instance to fold the event into state.
/// </summary>
public interface IProjectionDefinition : IHaveState
{
    /// <summary>
    /// Returns every projection identifier the supplied event should be applied to.
    /// </summary>
    /// <param name="event">The event being routed.</param>
    /// <returns>The projection identifiers affected by the event.</returns>
    IEnumerable<IBlobId> GetProjectionIds(IEvent @event);

    /// <summary>
    /// Applies the specified event to the projection state asynchronously.
    /// </summary>
    /// <param name="event">The event to apply.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous apply operation.</returns>
    Task ApplyAsync(IEvent @event, CancellationToken cancellationToken = default);
}
