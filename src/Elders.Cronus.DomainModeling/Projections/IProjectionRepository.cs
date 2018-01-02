namespace Elders.Cronus.DomainModeling.Projections
{
    public interface IProjectionRepository : IRepository<IProjectionDefinition>
    {
        IProjectionGetResult<T> Get<T>(IBlobId projectionId) where T : IProjectionDefinition;
    }
}
