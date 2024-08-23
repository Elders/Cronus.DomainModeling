using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Elders.Cronus;

public partial class EntityId : Urn
{
    [StringSyntax(StringSyntaxAttribute.Regex)]
    const string NSS_REGEX = @"\A(?i:(?<arname>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+):(?<arid>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+)\/(?<entityname>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+?):(?<entityid>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+))\z";

    [GeneratedRegex(NSS_REGEX, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.NonBacktracking, 500)]
    internal static partial Regex EntityRegex();

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

    public EntityId(AggregateRootId arUrn, ReadOnlySpan<char> entityName, ReadOnlySpan<char> entityId)
        : base(arUrn.Tenant.AsSpan(), $"{arUrn.AggregateRootName}{PARTS_DELIMITER}{arUrn.Id}{HIERARCHICAL_DELIMITER}{entityName}{PARTS_DELIMITER}{entityId}") { }

    private EntityId(ReadOnlySpan<char> urn) : base(urn)
    {
        base.DoFullInitialization();

        if (EntityRegex().IsMatch(nss.AsSpan()) == false) throw new ArgumentException("Invalid entity id");
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

            var nssSpan = nss.AsSpan();
            if (EntityRegex().IsMatch(nssSpan))
            {
                var lastSlash = nssSpan.LastIndexOf(HIERARCHICAL_DELIMITER);
                var arNss = nssSpan[0..lastSlash];
                aggregateRootId = new AggregateRootId(nid, arNss);

                var entityPart = nssSpan[(lastSlash + 1)..];
                var firstDelimiter = entityPart.IndexOf(PARTS_DELIMITER);
                id = nss;
                entityName = entityPart[0..firstDelimiter].ToString();
                entityId = entityPart[(firstDelimiter + 1)..].ToString();
            }

            isFullyInitialized = true;
        }
    }

    public static EntityId Parse(string urn)
    {
        Urn baseUrn = new Urn(urn);

        var match = EntityRegex().Match(baseUrn.NSS);
        if (match.Success)
        {
            var rootUrn = new AggregateRootId(baseUrn.NID, match.Groups["arname"].Value, match.Groups["arid"].Value);
            return new EntityId(rootUrn, match.Groups["entityname"].Value, match.Groups["entityid"].Value);
        }

        throw new ArgumentException($"Invalid {nameof(Cronus.EntityId)}: {urn}", nameof(urn));
    }

    public static EntityId Parse(ReadOnlySpan<char> candidate)
    {
        if (TryParse(candidate, out var entityId))
        {
            return entityId;
        }

        throw new ArgumentException($"Invalid entity id {candidate}", nameof(candidate));
    }

    public static bool TryParse(ReadOnlySpan<char> candidate, out EntityId entityId)
    {
        try
        {
            entityId = new EntityId(candidate);
            return true;
        }
        catch (Exception)
        {
            entityId = null;
            return false;
        }
    }

    public static T Parse<T>(ReadOnlySpan<char> candidate)
        where T : EntityId
    {
        if (TryParse<T>(candidate, out var entityId))
        {
            return entityId;
        }

        throw new ArgumentException("Invalid entity id");
    }

    public static bool TryParse<T>(ReadOnlySpan<char> candidate, out T entityId)
        where T : EntityId
    {
        try
        {
            entityId = (T)Activator.CreateInstance(typeof(T), true);
            var parsed = new EntityId(candidate);
            RawIdProperty.SetValue(entityId, parsed.RawId);

            var comparisoin = UseCaseSensitiveUrns ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            if (entityId.EntityName.Equals(parsed.EntityName, comparisoin) == false)
                return false;

            return true;
        }
        catch (Exception)
        {
            entityId = null;
            return false;
        }
    }
}
