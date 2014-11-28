using System;

namespace Elders.Cronus.DomainModeling
{
    /// <summary>
    /// Indicates the ability to store and retreive a stream of events.
    /// </summary>
    /// <remarks>
    /// Instances of this class must be designed to be thread safe such that they can be shared between threads.
    /// </remarks>
    public interface IAggregateRepository
    {
        void Save<AR>(AR aggregateRoot) where AR : IAggregateRoot;

        AR Load<AR>(IAggregateRootId id) where AR : IAggregateRoot;
    }
}