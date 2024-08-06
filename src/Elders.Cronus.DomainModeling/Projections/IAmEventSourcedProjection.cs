using System.Threading.Tasks;

namespace Elders.Cronus.Projections;

/// <summary>
/// Specifies that a projection supports event sourcing.
/// </summary>
public interface IAmEventSourcedProjection : IAmPartionableProjection
{
    /// <summary>
    /// Every event is sequentially passed to the projection handler and guarantees the order. If you do not care about this just use <see cref="IAmEventSourcedProjectionFast"/>
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    Task ReplayEventAsync(IEvent @event);

    /// <summary>
    /// Called after all events are iterated. It allows you to clean up after the process is finished.
    /// </summary>
    /// <returns></returns>
    Task OnReplayCompletedAsync() => Task.CompletedTask;
}

public interface IAmEventSourcedProjectionFast : IAmEventSourcedProjection
{

}
