namespace Elders.Cronus.Projections
{
    public interface IProjectionGetResult<out T>
    {
        bool Success { get; }

        T Projection { get; }
    }
}
