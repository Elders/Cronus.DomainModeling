using System;

namespace Elders.Cronus;

public class EntityUrn : Urn
{
    const string NSS_REGEX = @"\A(?i:(?<arname>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+):(?<arid>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+)\/(?<entityname>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+):(?<entityid>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+))\z";

    private string id;
    private string entityId;
    private string entityName;
    private IAggregateRootId aggregateRootId;
    private bool isFullyInitialized;

    protected EntityUrn()
    {
        entityId = string.Empty;
        entityName = string.Empty;
        aggregateRootId = default;
    }

    public EntityUrn(IAggregateRootId arUrn, string entityName, string entityId)
        : base(arUrn.Tenant, $"{arUrn.AggregateRootName}{PARTS_DELIMITER}{arUrn.Id}{HIERARCHICAL_DELIMITER}{entityName}{PARTS_DELIMITER}{entityId}")
    {
        this.aggregateRootId = arUrn ?? throw new ArgumentNullException(nameof(arUrn));
        this.entityName = entityName;
        this.entityId = entityId;
    }

    public IAggregateRootId AggregateRootId { get { DoFullInitialization(); return aggregateRootId; } }

    public string EntityName { get { DoFullInitialization(); return entityName; } }

    public string Id { get { DoFullInitialization(); return id; } }

    public string EntityId { get { DoFullInitialization(); return entityId; } }

    protected override void DoFullInitialization()
    {
        if (isFullyInitialized == false)
        {
            base.DoFullInitialization();

            var match = System.Text.RegularExpressions.Regex.Match(nss, NSS_REGEX, System.Text.RegularExpressions.RegexOptions.None);
            if (match.Success)
            {
                aggregateRootId = new AggregateUrn(nid, match.Groups["arname"].Value, match.Groups["arid"].Value);
                id = nss;
                entityName = match.Groups["entityname"].Value;
                entityId = match.Groups["entityid"].Value;
            }

            isFullyInitialized = true;
        }
    }

    new public static EntityUrn Parse(string urn)
    {
        Urn baseUrn = new Urn(urn);

        var match = System.Text.RegularExpressions.Regex.Match(baseUrn.NSS, NSS_REGEX, System.Text.RegularExpressions.RegexOptions.None);
        if (match.Success)
        {
            var rootUrn = new AggregateUrn(baseUrn.NID, match.Groups["arname"].Value, match.Groups["arid"].Value);
            return new EntityUrn(rootUrn, match.Groups["entityname"].Value, match.Groups["entityid"].Value);
        }

        throw new ArgumentException($"Invalid {nameof(EntityUrn)}: {urn}", nameof(urn));
    }

    new public static EntityUrn Parse(string urn, IUrnFormatProvider proviver)
    {
        string plain = proviver.Parse(urn);
        return Parse(plain);
    }
}
