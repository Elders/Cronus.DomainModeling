namespace Elders.Cronus.Projections
{
    public class ProjectionGetResult<T> : IProjectionGetResult<T>
    {
        public ProjectionGetResult(T projection)
        {
            Projection = projection;
        }

        public bool Success { get { return ReferenceEquals(null, Projection) == false; } }

        public T Projection { get; private set; }

        public static IProjectionGetResult<T> NoResult = new ProjectionGetResult<T>(default(T));
    }
}
