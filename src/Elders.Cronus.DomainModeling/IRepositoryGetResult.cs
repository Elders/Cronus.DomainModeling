namespace Elders.Cronus
{
    public interface IRepositoryGetResult<out T>
    {
        bool Success { get; }

        T Data { get; }
    }
}
