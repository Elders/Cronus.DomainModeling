using System.Threading;
using System.Threading.Tasks;

namespace Elders.Cronus;

/// <summary>
/// Defines a handler that processes a public event of type <typeparamref name="T"/>. Public events
/// are events visible to consumers outside the publishing bounded context.
/// </summary>
/// <typeparam name="T">The public event type handled by this handler.</typeparam>
public interface IPublicEventHandler<in T>
    where T : IPublicEvent
{
    /// <summary>
    /// Handles the specified public event asynchronously.
    /// </summary>
    /// <param name="event">The public event to handle.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous handle operation.</returns>
    Task HandleAsync(T @event, CancellationToken cancellationToken = default);
}
