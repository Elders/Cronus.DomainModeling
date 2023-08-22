using System.Threading.Tasks;

namespace Elders.Cronus;

/// <summary>
/// Indicates the ability to store and retreive a stream of events.
/// </summary>
/// <remarks>
/// Implementations of this interface must be designed to be thread safe such that they can be shared between threads and machines.
/// </remarks>
public interface IAggregateRepository
{
    /// <summary>
    /// Persists uncommitted events
    /// </summary>
    /// <typeparam name="AR"></typeparam>
    /// <param name="aggregateRoot"></param>
    Task SaveAsync<AR>(AR aggregateRoot) where AR : IAggregateRoot;

    /// <summary>
    /// Reconstructs the entire aggregate (including entities and VOs)
    /// </summary>
    /// <typeparam name="AR"></typeparam>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ReadResult<AR>> LoadAsync<AR>(AggregateRootId id) where AR : IAggregateRoot;
}
