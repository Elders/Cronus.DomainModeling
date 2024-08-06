namespace Elders.Cronus.Projections;

public interface IAmPartionableProjection
{
    long GetPartition(IEvent @event);
}
