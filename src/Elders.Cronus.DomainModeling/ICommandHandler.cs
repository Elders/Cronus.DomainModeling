using System.Threading;
using System.Threading.Tasks;

namespace Elders.Cronus;

/// <summary>
/// Defines a handler that processes a command of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The command type handled by this handler.</typeparam>
public interface ICommandHandler<in T>
    where T : ICommand
{
    /// <summary>
    /// Handles the specified command asynchronously.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous handle operation.</returns>
    Task HandleAsync(T command, CancellationToken cancellationToken = default);
}
