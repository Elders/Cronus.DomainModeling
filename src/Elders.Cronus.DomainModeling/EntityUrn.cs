using System;
using System.Text.RegularExpressions;

namespace Elders.Cronus;

public class EntityId : Urn
{
    const string NSS_REGEX = @"\A(?i:(?<arname>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+):(?<arid>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+)\/(?<entityname>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+?):(?<entityid>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+))\z";
    private static readonly Regex EntityRegex = new Regex(NSS_REGEX, System.Text.RegularExpressions.RegexOptions.None);

    private string id;
    private string entityId;
    private string entityName;
    private AggregateRootId aggregateRootId;
    private bool isFullyInitialized;

    protected EntityId()
    {
        entityId = string.Empty;
        entityName = string.Empty;
        aggregateRootId = default;
    }

    public EntityId(AggregateRootId arUrn, string entityName, string entityId)
        : base(arUrn.Tenant, $"{arUrn.AggregateRootName}{PARTS_DELIMITER}{arUrn.Id}{HIERARCHICAL_DELIMITER}{entityName}{PARTS_DELIMITER}{entityId}")
    {
        this.aggregateRootId = arUrn ?? throw new ArgumentNullException(nameof(arUrn));
        this.entityName = entityName;
        this.entityId = entityId;
    }

    public AggregateRootId AggregateRootId { get { DoFullInitialization(); return aggregateRootId; } }

    public string EntityName { get { DoFullInitialization(); return entityName; } }

    public string Id { get { DoFullInitialization(); return id; } }

    public string EntityID { get { DoFullInitialization(); return entityId; } }

    protected override void DoFullInitialization()
    {
        if (isFullyInitialized == false)
        {
            base.DoFullInitialization();

            var match = EntityRegex.Match(nss);
            if (match.Success)
            {
                aggregateRootId = new AggregateRootId(nid, match.Groups["arname"].Value, match.Groups["arid"].Value);
                id = nss;
                entityName = match.Groups["entityname"].Value;
                entityId = match.Groups["entityid"].Value;
            }

            isFullyInitialized = true;
        }
    }

    new public static EntityId Parse(string urn)
    {
        Urn baseUrn = new Urn(urn);

        var match = EntityRegex.Match(baseUrn.NSS);
        if (match.Success)
        {
            var rootUrn = new AggregateRootId(baseUrn.NID, match.Groups["arname"].Value, match.Groups["arid"].Value);
            return new EntityId(rootUrn, match.Groups["entityname"].Value, match.Groups["entityid"].Value);
        }

        throw new ArgumentException($"Invalid {nameof(Cronus.EntityId)}: {urn}", nameof(urn));
    }
}
