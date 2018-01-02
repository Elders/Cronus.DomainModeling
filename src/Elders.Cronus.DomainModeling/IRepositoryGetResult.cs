namespace Elders.Cronus.DomainModeling
{
    public interface IRepositoryGetResult<out T>
    {
        bool Success { get; }

        T Data { get; }
    }
}
