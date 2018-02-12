namespace Elders.Cronus.Projections
{
    public interface IProjectionLoader
    {
        IProjectionGetResult<T> Get<T>(IBlobId projectionId) where T : IProjectionDefinition;
    }
}
