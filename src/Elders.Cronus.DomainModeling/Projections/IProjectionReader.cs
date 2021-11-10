using System;
using System.Threading.Tasks;

namespace Elders.Cronus.Projections;

public interface IProjectionReader
{
    ReadResult<T> Get<T>(IBlobId projectionId) where T : IProjectionDefinition;
    ReadResult<IProjectionDefinition> Get(IBlobId projectionId, Type projectionType);

    Task<ReadResult<T>> GetAsync<T>(IBlobId projectionId) where T : IProjectionDefinition;
    Task<ReadResult<IProjectionDefinition>> GetAsync(IBlobId projectionId, Type projectionType);
}
