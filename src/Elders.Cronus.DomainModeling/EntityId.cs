using System;
using System.Text.Json.Serialization;

namespace Elders.Cronus;

public abstract class EntityId<TAggregateRootId> : EntityUrn, IEntityId
    where TAggregateRootId : IAggregateRootId
{
    protected EntityId() { }

    public EntityId(string idBase, TAggregateRootId rootId, string entityName) : base(rootId, entityName, idBase)
    {

    }

    IAggregateRootId IEntityId.AggregateRootId { get { return base.AggregateRootId; } }

    TAggregateRootId aggregateRootId;

    [JsonIgnore]
    new public TAggregateRootId AggregateRootId
    {
        get
        {
            aggregateRootId = (TAggregateRootId)Activator.CreateInstance(typeof(TAggregateRootId), true);
            RawIdProperty.SetValue(aggregateRootId, base.AggregateRootId.RawId);

            return aggregateRootId;
        }
    }
}
