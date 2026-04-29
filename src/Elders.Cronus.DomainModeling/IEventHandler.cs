using System.Threading;
using System.Threading.Tasks;

namespace Elders.Cronus;

/// <summary>
/// Defines a handler that processes an event of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The event type handled by this handler.</typeparam>
public interface IEventHandler<in T>
    where T : IEvent
{
    /// <summary>
    /// Handles the specified event asynchronously.
    /// </summary>
    /// <param name="event">The event to handle.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous handle operation.</returns>
    Task HandleAsync(T @event, CancellationToken cancellationToken = default);
}
