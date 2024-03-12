using System;
using System.Threading.Tasks;

namespace Elders.Cronus.Projections;

public interface IProjectionReader
{
    Task<ReadResult<T>> GetAsync<T>(IBlobId projectionId) where T : IProjectionDefinition;
    Task<ReadResult<T>> GetAsOfAsync<T>(IBlobId projectionId, DateTimeOffset timestamp) where T : IProjectionDefinition;

    Task<ReadResult<IProjectionDefinition>> GetAsync(IBlobId projectionId, Type projectionType);
    Task<ReadResult<IProjectionDefinition>> GetAsOfAsync(IBlobId projectionId, Type projectionType, DateTimeOffset timestamp);
}
