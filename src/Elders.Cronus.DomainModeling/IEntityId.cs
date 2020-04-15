namespace Elders.Cronus
{
    public interface IEntityId : IUrn, IBlobId
    {
        string Id { get; }
        string EntityName { get; }
        string EntityId { get; }
        IAggregateRootId AggregateRootId { get; }
    }
}
