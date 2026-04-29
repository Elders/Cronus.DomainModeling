using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elders.Cronus;

/// <summary>
/// This is a handler where commands are received and delivered to the addressed AggregateRoot.
/// We call these handlers *ApplicationService*. This is the *write side* in CQRS.
/// </summary>
public interface IApplicationService : IMessageHandler { }

public abstract class ApplicationService<AR> : IApplicationService where AR : IAggregateRoot
{
    protected readonly IAggregateRepository repository;

    public ApplicationService(IAggregateRepository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// Executes an action against an existing aggregate root. Use this method only when the aggregate
    /// is guaranteed to exist; if it does not, the method throws.
    /// </summary>
    /// <param name="id">The identifier of the aggregate root to load and mutate.</param>
    /// <param name="update">The mutation to apply to the loaded aggregate root before saving.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous load-update-save sequence.</returns>
    /// <exception cref="Exception">Thrown when the aggregate root cannot be loaded.</exception>
    public virtual async Task UpdateAsync(AggregateRootId id, Action<AR> update, CancellationToken cancellationToken = default)
    {
        ReadResult<AR> result = await repository.LoadAsync<AR>(id).ConfigureAwait(false);
        if (result.IsSuccess)
        {
            update(result.Data);
            await repository.SaveAsync(result.Data).ConfigureAwait(false);
        }
        else
        {
            throw new Exception($"Failed to load an aggregate. {result}");
        }
    }
}
