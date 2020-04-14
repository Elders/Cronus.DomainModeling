namespace Elders.Cronus
{
    public abstract class EntityId<TAggregateRootId> : EntityUrn, IEntityId
        where TAggregateRootId : IAggregateRootId
    {
        protected EntityId() { }

        public EntityId(string idBase, TAggregateRootId rootId, string entityName) : base(rootId, entityName, idBase)
        {

        }

        IAggregateRootId IEntityId.AggregateRootId { get { return base.AggregateRootId; } }
    }
}
