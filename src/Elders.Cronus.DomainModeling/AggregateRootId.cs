using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Elders.Cronus;

public abstract class AggregateRootId<T> : AggregateRootId
    where T : AggregateRootId<T>
{
    protected AggregateRootId() { }

    protected AggregateRootId(string tenant, string rootName, string id) : base(tenant, rootName, id) { }

    public static T New(string tenant)
    {
        var instance = (T)System.Activator.CreateInstance(typeof(T), true);
        return instance.Construct(System.Guid.NewGuid().ToString(), tenant);
    }

    public static T New(string tenant, string id)
    {
        var instance = (T)System.Activator.CreateInstance(typeof(T), true);
        return instance.Construct(id, tenant);
    }

    protected abstract T Construct(string id, string tenant);

    new public static T Parse(string id)
    {
        var instance = (T)System.Activator.CreateInstance(typeof(T), true);

        var stringTenantUrn = AggregateRootId.Parse(id);
        var newId = instance.Construct(stringTenantUrn.Id, stringTenantUrn.Tenant);
        if (stringTenantUrn.AggregateRootName == newId.AggregateRootName)
            return newId;
        else
            throw new System.Exception("bum");
        //todo check if ar name mateches..
    }

    public static bool TryParse(string id, out T result)
    {
        try
        {
            var instance = (T)System.Activator.CreateInstance(typeof(T), true);

            var stringTenantUrn = AggregateRootId.Parse(id);
            var newId = instance.Construct(stringTenantUrn.Id, stringTenantUrn.Tenant);
            if (stringTenantUrn.AggregateRootName == newId.AggregateRootName)
                result = newId;
            else
                throw new System.Exception("bum");
            //todo check if ar name mateches..

            return true;
        }
        catch (Exception)
        {
            result = null;
            return false;
        }
    }
}

[DataContract(Name = "b78e63f3-1443-4e82-ba4c-9b12883518b9")]
public class AggregateRootId : Urn
{
    private const string NSS_REGEX = @"\A(?i:(?<arname>(?:[-a-z0-9()+,.=@;$_!*'&~\/]|%[0-9a-f]{2})+):(?<id>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+))\z";
    private static Regex NssRegex = new Regex(NSS_REGEX, RegexOptions.None);

    protected AggregateRootId()
    {
        this.id = string.Empty;
        this.tenant = string.Empty;
        this.aggregateRootName = string.Empty;
    }

    public AggregateRootId(string tenant, string arName, string id)
        : base(tenant, $"{arName}{PARTS_DELIMITER}{id}")
    {
        this.id = id;
        this.tenant = tenant;
        this.aggregateRootName = arName;
    }

    public AggregateRootId(string tenant, AggregateRootId urn)
        : base(tenant, $"{urn.AggregateRootName}{PARTS_DELIMITER}{urn.Id}")
    {
        this.id = urn.Id;
        this.tenant = tenant;
        this.aggregateRootName = urn.AggregateRootName;
    }

    public AggregateRootId(AggregateRootId urn, string aggregateRootName)
        : this(urn.tenant, urn)
    {
        if (aggregateRootName.Equals(urn.AggregateRootName, StringComparison.OrdinalIgnoreCase) == false)
            throw new ArgumentException("AggregateRootName missmatch");
    }

    string id;
    string tenant;
    string aggregateRootName;
    private bool isFullyInitialized;

    protected override void DoFullInitialization()
    {
        if (isFullyInitialized == false)
        {
            base.DoFullInitialization();

            var match = NssRegex.Match(nss);
            if (match.Success)
            {
                id = match.Groups["id"].Value;
                aggregateRootName = match.Groups["arname"].Value;
                tenant = nid;
            }

            isFullyInitialized = true;
        }
    }

    public string Id { get { DoFullInitialization(); return id; } }

    public string Tenant { get { DoFullInitialization(); return tenant; } }

    public string AggregateRootName { get { DoFullInitialization(); return aggregateRootName; } }

    public static bool TryParse(string urn, out AggregateRootId parsedUrn)
    {
        parsedUrn = null;

        if (IsUrn(urn) == false)
            return false;

        Urn baseUrn = new Urn(urn);

        Match match = NssRegex.Match(baseUrn.NSS);
        if (match.Success)
        {
            parsedUrn = new AggregateRootId(baseUrn.NID, match.Groups["arname"].Value, match.Groups["id"].Value);
            return true;
        }

        return false;
    }

    new public static AggregateRootId Parse(string urn)
    {
        if (TryParse(urn, out AggregateRootId parsedUrn))
        {
            return parsedUrn;
        }
        else
        {
            throw new ArgumentException($"Invalid {nameof(AggregateRootId)}: {urn}", nameof(urn));
        }
    }
}
