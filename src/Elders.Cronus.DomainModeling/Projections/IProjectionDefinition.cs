namespace Elders.Cronus.DomainModeling.Projections
{
    public interface IProjectionDefinition : IHaveState, IAmEventSourcedProjection
    {
        IBlobId GetProjectionId(IEvent @event);

        void Apply(IEvent @event);
    }
}
