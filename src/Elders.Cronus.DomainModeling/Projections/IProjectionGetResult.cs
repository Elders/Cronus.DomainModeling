namespace Elders.Cronus.DomainModeling.Projections
{
    public interface IProjectionGetResult<out T>
    {
        bool Success { get; }

        T Projection { get; }
    }
}
