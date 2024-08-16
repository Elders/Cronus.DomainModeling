using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Elders.Cronus;

public abstract class AggregateRootId<T> : AggregateRootId
    where T : AggregateRootId<T>
{
    protected AggregateRootId() { }

    protected AggregateRootId(string tenant, string rootName, string id) : base(tenant, rootName, id) { }

    protected AggregateRootId(ReadOnlySpan<char> tenant, ReadOnlySpan<char> rootName, ReadOnlySpan<char> id) : base(tenant, rootName, id) { }

    new public abstract string AggregateRootName { get; }

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

    public static T New(ReadOnlySpan<char> tenant)
    {
        var instance = (T)System.Activator.CreateInstance(typeof(T), true);
        return instance.Construct(Guid.NewGuid().ToString(), tenant);
    }

    public static T New(ReadOnlySpan<char> tenant, ReadOnlySpan<char> id)
    {
        var instance = (T)System.Activator.CreateInstance(typeof(T), true);
        return instance.Construct(id, tenant);
    }

    protected abstract T Construct(string id, string tenant);
    protected abstract T Construct(ReadOnlySpan<char> id, ReadOnlySpan<char> tenant);
    protected virtual T Construct(AggregateRootId from)
    {
        var comparisoin = UseCaseSensitiveUrns ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        if (from.AggregateRootName.Equals(AggregateRootName, comparisoin) == false)
            throw new System.Exception($"Failed to construct aggregate root id of type {typeof(T).Name}. Aggregate root name mismatch.");

        RawId = new byte[from.RawId.Length];
        from.RawId.AsSpan().CopyTo(RawId);
        return (T)this;
    }

    new public static T Parse(ReadOnlySpan<char> candidate)
    {
        if (TryParse(candidate, out var instance))
        {
            return instance;
        }

        throw new ArgumentException("Invalid aggregate root id.", nameof(candidate));
    }

    public static bool TryParse(ReadOnlySpan<char> candidate, out T aggregateRootId)
    {
        try
        {
            var instance = (T)System.Activator.CreateInstance(typeof(T), true);
            var arId = new AggregateRootId(candidate);
            aggregateRootId = instance.Construct(arId);

            return true;
        }
        catch (Exception)
        {
            aggregateRootId = null;
            return false;
        }
    }

    new public static T Parse(string candidate)
    {
        if (TryParse(candidate, out var instance))
        {
            return instance;
        }

        throw new ArgumentException("Invalid aggregate root id.", nameof(candidate));
    }

    public static bool TryParse(string candidate, out T aggregateRootId)
    {
        try
        {
            aggregateRootId = (T)System.Activator.CreateInstance(typeof(T), true);
            var stringTenantUrn = AggregateRootId.Parse(candidate);
            aggregateRootId = aggregateRootId.Construct(stringTenantUrn.Id, stringTenantUrn.Tenant);
            if (aggregateRootId.AggregateRootName != stringTenantUrn.AggregateRootName)
                return false;

            return true;
        }
        catch (Exception)
        {
            aggregateRootId = null;
            return false;
        }
    }
}

[DataContract(Name = "b78e63f3-1443-4e82-ba4c-9b12883518b9")]
public partial class AggregateRootId : Urn
{
    private const string NSS_REGEX = @"\A(?i:(?<arname>(?:[-a-z0-9()+,.=@;$_!*'&~\/]|%[0-9a-f]{2})+):(?<id>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+))\z";

    [GeneratedRegex(NSS_REGEX, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.NonBacktracking, 500)]
    internal static partial Regex NssRegex();

    protected AggregateRootId() { }

    public AggregateRootId(string tenant, string arName, string id)
        : base(tenant, $"{arName}{PARTS_DELIMITER}{id}") { }

    public AggregateRootId(ReadOnlySpan<char> tenant, ReadOnlySpan<char> arName, ReadOnlySpan<char> id)
        : base(tenant, $"{arName}{PARTS_DELIMITER}{id}") { }

    public AggregateRootId(string tenant, AggregateRootId arId)
        : base(tenant, $"{arId.AggregateRootName}{PARTS_DELIMITER}{arId.Id}") { }

    public AggregateRootId(ReadOnlySpan<char> tenant, AggregateRootId arId)
        : base(tenant, $"{arId.AggregateRootName}{PARTS_DELIMITER}{arId.Id}") { }

    public AggregateRootId(AggregateRootId arId, string aggregateRootName)
        : this(arId.Tenant.AsSpan(), arId)
    {
        if (aggregateRootName.Equals(arId.AggregateRootName, StringComparison.OrdinalIgnoreCase) == false)
            throw new ArgumentException("AggregateRootName mismatch");
    }

    internal AggregateRootId(ReadOnlySpan<char> urn) : base(urn)
    {
        var match = UrnRegex.Match(urn.ToString());
        var nss = match.Groups[UrnRegex.Group.NSS.ToString()].Value; // not using the NSS property to prevent full initialization and more GC
        if (NssRegex().IsMatch(nss.AsSpan()) == false) throw new ArgumentException("Invalid aggregate root id");
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

            var match = NssRegex().Match(nss);
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

    public static bool TryParse(string candicate, out AggregateRootId parsedUrn)
    {
        try
        {
            parsedUrn = null;
            if (IsUrn(candicate) == false)
            {
                return false;
            }

            var urn = new Urn(candicate);
            Match nssMatch = NssRegex().Match(urn.NSS);
            if (nssMatch.Success)
            {
                parsedUrn = new AggregateRootId(urn.NID, nssMatch.Groups["arname"].Value, nssMatch.Groups["id"].Value);
                return true;
            }

            return false;
        }
        catch (Exception)
        {
            parsedUrn = null;
            return false;
        }
    }

    public static bool TryParse(ReadOnlySpan<char> candicate, out AggregateRootId parsedUrn)
    {
        try
        {
            parsedUrn = new AggregateRootId(candicate);
            return true;
        }
        catch (Exception)
        {
            parsedUrn = null;
            return false;
        }
    }

    public static AggregateRootId Parse(string urn)
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

    public static AggregateRootId Parse(ReadOnlySpan<char> urn)
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
