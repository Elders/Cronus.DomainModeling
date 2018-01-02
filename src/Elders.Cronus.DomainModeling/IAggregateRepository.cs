namespace Elders.Cronus
{
    public interface IRepository<T>
    {
        IRepositoryGetResult<X> Load<X>(IBlobId id) where X : T;
        void Persist<X>(X data) where X : T;
    }

    /// <summary>
    /// Indicates the ability to store and retreive a stream of events.
    /// </summary>
    /// <remarks>
    /// Implementations of this interface must be designed to be thread safe such that they can be shared between threads and
    /// machines.
    /// </remarks>
    public interface IAggregateRepository : IRepository<IAggregateRoot>
    {
        void Save<AR>(AR aggregateRoot) where AR : IAggregateRoot;

        AR Load<AR>(IAggregateRootId id) where AR : IAggregateRoot;

        bool TryLoad<AR>(IAggregateRootId id, out AR aggregateRoot) where AR : IAggregateRoot;
    }
}
