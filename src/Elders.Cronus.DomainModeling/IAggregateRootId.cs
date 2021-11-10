namespace Elders.Cronus;

public interface IAggregateRootId : IUrn, IBlobId
{
    string AggregateRootName { get; }
    string Id { get; }
    string Tenant { get; }
}
