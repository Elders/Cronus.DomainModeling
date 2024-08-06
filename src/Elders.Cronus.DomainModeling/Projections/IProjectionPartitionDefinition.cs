namespace Elders.Cronus.Projections;

/// <summary>
/// Defines how projection cassandra partitions will be calculated.
/// </summary>
public interface IProjectionPartitionDefinition
{
    long CalculatePartition(IEvent @event);
}

public record class PartitionByTimestamp : IProjectionPartitionDefinition
{
    public long CalculatePartition(IEvent @event)
    {
        int month = @event.Timestamp.Month;
        int day = @event.Timestamp.DayOfYear;
        int partitionId = @event.Timestamp.Year * 10000 + month *1000 + day *10; 

        return partitionId;
    }
}

