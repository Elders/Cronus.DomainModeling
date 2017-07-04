namespace Elders.Cronus.DomainModeling.Projections
{
    public interface IProjectionRepository
    {
        IProjectionGetResult<T> Get<T>(IBlobId projectionId) where T : IProjectionDefinition;
    }
}
