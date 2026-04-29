using System.Threading;
using System.Threading.Tasks;

namespace Elders.Cronus.Projections;

/// <summary>
/// Specifies that a projection supports event sourcing.
/// </summary>
public interface IAmEventSourcedProjection
{
    /// <summary>
    /// Every event is sequentially passed to the projection handler and guarantees the order.
    /// If you do not care about ordering, use <see cref="IAmEventSourcedProjectionFast"/> instead.
    /// </summary>
    /// <param name="event">The event being replayed.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous replay operation.</returns>
    Task ReplayEventAsync(IEvent @event, CancellationToken cancellationToken = default);

    /// <summary>
    /// Called after all events are iterated. It allows you to clean up after the process is finished.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous completion callback.</returns>
    Task OnReplayCompletedAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
}

/// <summary>
/// Specifies that a projection supports event-sourced replay without requiring strict per-event ordering,
/// allowing the framework to apply events in parallel for higher throughput.
/// </summary>
public interface IAmEventSourcedProjectionFast : IAmEventSourcedProjection
{

}
