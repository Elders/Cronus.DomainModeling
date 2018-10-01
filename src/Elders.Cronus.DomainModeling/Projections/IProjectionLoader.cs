using System;
using System.Threading.Tasks;

namespace Elders.Cronus.Projections
{
    public interface IProjectionLoader
    {
        IProjectionGetResult<T> Get<T>(IBlobId projectionId) where T : IProjectionDefinition;
        IProjectionGetResult<IProjectionDefinition> Get(IBlobId projectionId, Type projectionType);

        Task<IProjectionGetResult<T>> GetAsync<T>(IBlobId projectionId) where T : IProjectionDefinition;
        Task<IProjectionGetResult<IProjectionDefinition>> GetAsync(IBlobId projectionId, Type projectionType);
    }
}
