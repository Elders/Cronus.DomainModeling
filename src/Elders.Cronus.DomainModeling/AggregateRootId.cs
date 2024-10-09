using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Elders.Cronus;

//public abstract class AggregateRootId<T> : AggregateRootId
//    where T : AggregateRootId<T>
//{
//    protected AggregateRootId() { }

//    protected AggregateRootId(ReadOnlySpan<char> tenant, ReadOnlySpan<char> id)
//    {

//    }

//    public abstract ReadOnlySpan<char> AggregateRootName { get; }
//}

//public class TestId : AggregateRootId
//{
//    TestId() { }
//    public TestId(ReadOnlySpan<char> tenant, ReadOnlySpan<char> id) : base(tenant, id) { }


//}

[DataContract(Name = "b78e63f3-1443-4e82-ba4c-9b12883518b9")]
public partial class AggregateRootId : Urn
{
    [StringSyntax(StringSyntaxAttribute.Regex)]
    private const string NSS_REGEX = @"\A(?i:(?<arname>(?:[-a-z0-9()+,.=@;$_!*'&~\/]|%[0-9a-f]{2})+):(?<id>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+))\z";

    [GeneratedRegex(NSS_REGEX, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.NonBacktracking, 500)]
    internal static partial Regex NssRegex();

    private protected string id;
    private protected string tenant;
    private protected string aggregateRootName;
    private protected bool isFullyInitialized;

    protected AggregateRootId() { }

    public AggregateRootId(AggregateRootId urn)
        : this(urn.Tenant, urn.AggregateRootName, urn.Id) { }

    public AggregateRootId(ReadOnlySpan<char> tenant, ReadOnlySpan<char> arName, ReadOnlySpan<char> id) : this(tenant.ToString(), arName.ToString(), id.ToString()) { }

    public AggregateRootId(string tenant, string arName, string id) : base(tenant, $"{arName}{PARTS_DELIMITER}{id}")
    {
        this.tenant = tenant.ToString();
        this.aggregateRootName = arName.ToString();
        this.id = id;

        isFullyInitialized = true;
    }

    internal AggregateRootId(ReadOnlySpan<char> urn) : base(urn)
    {
        base.DoFullInitialization();

        if (NssRegex().IsMatch(nss.AsSpan()) == false)
            throw new ArgumentException("Invalid aggregate root id", nameof(urn));
    }

    internal AggregateRootId(ReadOnlySpan<char> tenant, ReadOnlySpan<char> nss) : base(tenant, nss)
    {
        if (NssRegex().IsMatch(nss) == false)
            throw new ArgumentException("Invalid aggregate root NSS", nameof(nss));
    }

    protected override void DoFullInitialization()
    {
        if (isFullyInitialized == false)
        {
            base.DoFullInitialization();

            var nssSpan = nss.AsSpan();
            if (NssRegex().IsMatch(nssSpan))
            {
                Span<Range> ranges = stackalloc Range[2];
                if (nssSpan.Split(ranges, PARTS_DELIMITER) != 2)
                    throw new InvalidOperationException($"Unable to initialize aggregate root id from NSS {nss}");

                aggregateRootName = nssSpan[ranges[0]].ToString();
                id = nssSpan[ranges[1]].ToString();
                tenant = nid;
            }

            isFullyInitialized = true;
        }
    }

    public string Id { get { DoFullInitialization(); return id; } }

    public string Tenant { get { DoFullInitialization(); return tenant; } }

    public string AggregateRootName { get { DoFullInitialization(); return aggregateRootName; } }

    protected string AggregateRootIdentifier { get; set; }

    public static bool TryParse(ReadOnlySpan<char> candidate, out AggregateRootId parsedUrn)
    {
        try
        {
            parsedUrn = new AggregateRootId(candidate);
            return true;
        }
        catch (Exception)
        {
            parsedUrn = null;
            return false;
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

    public static implicit operator ReadOnlySpan<byte>(AggregateRootId urn) => urn is null ? ReadOnlyMemory<byte>.Empty.Span : urn.RawId.Span;
    public static implicit operator ReadOnlyMemory<byte>(AggregateRootId urn) => urn is null ? ReadOnlyMemory<byte>.Empty : urn.RawId;
}
