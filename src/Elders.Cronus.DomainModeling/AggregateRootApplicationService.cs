using System;

namespace Elders.Cronus
{
    /// <summary>
    /// This is a handler where commands are received and delivered to the addressed AggregateRoot.
    /// We call these handlers *ApplicationService*. This is the *write side* in CQRS.
    /// </summary>
    public interface IAggregateRootApplicationService
    {
        IAggregateRepository Repository { get; set; }
    }

    public class AggregateRootApplicationService<AR> : IAggregateRootApplicationService where AR : IAggregateRoot
    {
        public IAggregateRepository Repository { get; set; }

        public void Update(IAggregateRootId id, Action<AR> update)
        {
            var ar = Repository.Load<AR>(id);
            update(ar);
            Repository.Save(ar);
        }
    }
}
