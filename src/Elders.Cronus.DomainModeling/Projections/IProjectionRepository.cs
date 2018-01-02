namespace Elders.Cronus.Projections
{
    public interface IProjectionRepository
    {
        IProjectionGetResult<T> Get<T>(IBlobId projectionId) where T : IProjectionDefinition;
    }
}
