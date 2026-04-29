using System.Threading;
using System.Threading.Tasks;

namespace Elders.Cronus;

/// <summary>
/// Defines a handler that processes a signal of type <typeparamref name="T"/>. Signals are
/// short-lived notifications that do not change aggregate state.
/// </summary>
/// <typeparam name="T">The signal type handled by this handler.</typeparam>
public interface ISignalHandle<in T>
    where T : ISignal
{
    /// <summary>
    /// Handles the specified signal asynchronously.
    /// </summary>
    /// <param name="signal">The signal to handle.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous handle operation.</returns>
    Task HandleAsync(T signal, CancellationToken cancellationToken = default);
}
