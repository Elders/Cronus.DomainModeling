using System;
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
    /// Executes an action to an existing AR. Use this method only if you are 100% sure that the AR must exist.
    /// If the AR does not exists the method throws an exception.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="update"></param>
    public virtual async Task UpdateAsync(IAggregateRootId id, Action<AR> update)
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
