using System;
using System.Threading.Tasks;

namespace Elders.Cronus.Projections;

public interface IProjectionReader
{
    Task<ReadResult<T>> GetAsync<T>(IBlobId projectionId) where T : IProjectionDefinition;
    Task<ReadResult<IProjectionDefinition>> GetAsync(IBlobId projectionId, Type projectionType);
}
