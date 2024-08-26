using System;
using System.Text;
using System.Text.Json.Serialization;

namespace Elders.Cronus;

public abstract class EntityId<TAggregateRootId> : EntityId
    where TAggregateRootId : AggregateRootId
{
    protected EntityId() { }

    public EntityId(ReadOnlySpan<char> idBase, TAggregateRootId rootId)
    {
        if (idBase.IsEmpty) throw new ArgumentException("Entity base id cannot be empty", nameof(idBase));
        if (rootId is null) throw new ArgumentNullException(nameof(rootId));
        if (EntityName.IsEmpty) throw new InvalidOperationException($"{nameof(EntityName)} cannot be null or empty.");

        Span<char> nss = stackalloc char[rootId.NSS.Length + 1 + EntityName.Length + 1 + idBase.Length];
        rootId.NSS.CopyTo(nss[..rootId.NSS.Length]);
        nss[rootId.NSS.Length] = HIERARCHICAL_DELIMITER;
        EntityName.CopyTo(nss[(rootId.NSS.Length + 1)..(rootId.NSS.Length + 1 + EntityName.Length)]);
        nss[rootId.NSS.Length + 1 + EntityName.Length] = PARTS_DELIMITER;
        idBase.CopyTo(nss[^idBase.Length..]);

        Span<char> urn = stackalloc char[5 + rootId.NID.Length + nss.Length];
        urn[0] = 'u'; urn[1] = 'r'; urn[2] = 'n'; urn[3] = PARTS_DELIMITER;
        rootId.NID.CopyTo(urn[4..(4 + rootId.NID.Length)]);
        urn[4 + rootId.NID.Length] = PARTS_DELIMITER;
        nss.CopyTo(urn[^nss.Length..]);

        if (IsUrn(urn) == false)
            throw new ArgumentException($"Invalid aggregate root id.");

        if (EntityRegex().IsMatch(nss) == false)
            throw new ArgumentException($"Invalid aggregate root id.");

        ConvertCaseIfNeeded(urn);

        rawId = new byte[urn.Length];
        Encoding.UTF8.GetBytes(urn, rawId.Span);
    }

    TAggregateRootId aggregateRootId;

    new protected abstract ReadOnlySpan<char> EntityName { get; }

    [JsonIgnore]
    new public TAggregateRootId AggregateRootId
    {
        get
        {
            aggregateRootId = (TAggregateRootId)Activator.CreateInstance(typeof(TAggregateRootId), true);
            aggregateRootId.SetRawId(base.AggregateRootId.RawId);

            return aggregateRootId;
        }
    }
}
