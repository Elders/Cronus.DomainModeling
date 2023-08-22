using System;
using System.Text.Json.Serialization;

namespace Elders.Cronus;

public abstract class EntityId<TAggregateRootId> : EntityId
    where TAggregateRootId : AggregateRootId
{
    protected EntityId() { }

    public EntityId(string idBase, TAggregateRootId rootId, string entityName) : base(rootId, entityName, idBase)
    {

    }

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
