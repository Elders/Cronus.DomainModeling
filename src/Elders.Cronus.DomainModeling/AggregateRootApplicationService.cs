using System;

namespace Elders.Cronus
{
    /// <summary>
    /// This is a handler where commands are received and delivered to the addressed AggregateRoot.
    /// We call these handlers *ApplicationService*. This is the *write side* in CQRS.
    /// </summary>
    public interface IAggregateRootApplicationService { }

    public abstract class AggregateRootApplicationService<AR> : IAggregateRootApplicationService where AR : IAggregateRoot
    {
        protected readonly IAggregateRepository repository;

        public AggregateRootApplicationService(IAggregateRepository repository)
        {
            if (repository is null) throw new ArgumentNullException(nameof(repository));

            this.repository = repository;
        }

        public virtual void Update(IAggregateRootId id, Action<AR> update)
        {
            var ar = repository.Load<AR>(id);
            update(ar);
            repository.Save(ar);
        }
    }
}
