using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Elders.Cronus;

[DataContract(Name = "d3ff08b5-38e2-4aaf-b3a8-ccc423ed096d")]
public class Urn : IEquatable<Urn>, IBlobId
{
    /// <summary>
    /// Specifies if the URNs will follow strictly the rfc(https://tools.ietf.org/html/rfc8141)
    /// We do recommend to use case insensitive urns. Enable this feature if you really need it.
    /// </summary>
    public static bool UseCaseSensitiveUrns = false;

    internal static readonly PropertyInfo RawIdProperty = typeof(Urn).GetProperty(nameof(RawId), BindingFlags.Instance | BindingFlags.Public);

    public const char PARTS_DELIMITER = ':';
    public const char HIERARCHICAL_DELIMITER = '/';
    public const string UriSchemeUrn = "urn";
    public const string PREFIX_R_COMPONENT = "?+";
    public const string PREFIX_Q_COMPONENT = "?=";
    public const string PREFIX_F_COMPONENT = "#";

    private Uri uri;

    protected Urn()
    {
        RawId = [];
    }

    public Urn(Urn urn) : this(urn.Value) { }

    public Urn(string urnString)
    {
        if (IsUrn(urnString) == false) throw new ArgumentException("String is not a URN!", nameof(urnString));

        if (UseCaseSensitiveUrns == false)
            urnString = urnString.ToLower();

        uri = new Uri(urnString);
        RawId = Encoding.UTF8.GetBytes(uri.ToString());
    }

    /// <summary>
    /// Initializes a new URN
    /// </summary>
    /// <param name="nid">The Namespace Identifier</param>
    /// <param name="nss">The Namespace Specific String</param>
    public Urn(string nid, string nss, string rcomponent = null, string qcomponent = null, string fcomponent = null)
    {
        if (string.IsNullOrEmpty(nid)) throw new ArgumentException("NID is not valid", nameof(nid));
        if (string.IsNullOrEmpty(nss)) throw new ArgumentException("NSS is not valid", nameof(nss));

        string urn = BuildUrnString(nid, nss, rcomponent, qcomponent, fcomponent);

        if (UseCaseSensitiveUrns == false)
            urn = urn.ToLower();

        uri = new Uri(urn);
        RawId = Encoding.UTF8.GetBytes(uri.ToString());
    }

    protected Uri Uri
    {
        get
        {
            if (uri is null)
                uri = new Uri(Encoding.UTF8.GetString(RawId));

            return uri;
        }
    }

    protected string nid;
    protected string nss;
    protected string r_Component;
    protected string q_Component;
    protected string f_Component;

    private void SetUri(Uri uri)
    {
        System.Text.RegularExpressions.Match match = UrnRegex.Match(uri.AbsoluteUri);
        nid = match.Groups[UrnRegex.Group.NID.ToString()].Value;
        nss = match.Groups[UrnRegex.Group.NSS.ToString()].Value;
        r_Component = match.Groups[UrnRegex.Group.R_Component.ToString()].Value;
        q_Component = match.Groups[UrnRegex.Group.Q_Component.ToString()].Value;
        f_Component = match.Groups[UrnRegex.Group.F_Component.ToString()].Value;
    }

    [DataMember(Order = 10)]
    public byte[] RawId { get; private set; }

    private bool isFullyInitialized;
    protected virtual void DoFullInitialization()
    {
        if (isFullyInitialized == false)
        {
            SetUri(Uri);
            isFullyInitialized = true;
        }
    }

    private static string BuildUrnString(string nid, string nss, string rcomponent, string qcomponent, string fcomponent)
    {
        var urn = new StringBuilder($"urn{PARTS_DELIMITER}{nid}{PARTS_DELIMITER}{nss}");

        if (string.IsNullOrEmpty(rcomponent) == false)
        {
            if (rcomponent.Contains(PREFIX_Q_COMPONENT) || rcomponent.Contains(PREFIX_F_COMPONENT))
                throw new ArgumentException("rcomponent includes illegal characters!", nameof(rcomponent));

            urn.Append(rcomponent.StartsWith(PREFIX_R_COMPONENT) ? rcomponent : $"{PREFIX_R_COMPONENT}{rcomponent}");
        }

        if (string.IsNullOrEmpty(qcomponent) == false)
        {
            if (qcomponent.Contains(PREFIX_R_COMPONENT) || qcomponent.Contains(PREFIX_F_COMPONENT))
                throw new ArgumentException("qcomponent includes illegal characters!", nameof(qcomponent));

            urn.Append(rcomponent.StartsWith(PREFIX_Q_COMPONENT) ? qcomponent : $"{PREFIX_Q_COMPONENT}{qcomponent}");
        }


        if (string.IsNullOrEmpty(fcomponent) == false)
        {
            if (fcomponent.Contains(PREFIX_R_COMPONENT) || fcomponent.Contains(PREFIX_Q_COMPONENT))
                throw new ArgumentException("fcomponent includes illegal characters!", nameof(fcomponent));

            urn.Append(rcomponent.StartsWith(PREFIX_F_COMPONENT) ? fcomponent : $"{PREFIX_F_COMPONENT}{fcomponent}");
        }

        return urn.ToString();
    }

    public string NID { get { DoFullInitialization(); return nid; } }

    public string NSS { get { DoFullInitialization(); return nss; } }

    public string R_Component { get { DoFullInitialization(); return r_Component; } }

    public string Q_Component { get { DoFullInitialization(); return q_Component; } }

    public string F_Component { get { DoFullInitialization(); return f_Component; } }

    public string Value => Uri?.ToString();

    public override string ToString() => Value;

    public static implicit operator string(Urn urn) => urn?.Value;

    public static implicit operator byte[](Urn urn) => urn?.RawId;

    public static bool operator ==(Urn left, Urn right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(Urn left, Urn right)
    {
        return !(left == right);
    }

    public static bool IsUrn(string candidate)
    {
        try { return UrnRegex.Matches(candidate); }
        catch (Exception) { return false; }
    }

    public override bool Equals(object comparand)
    {
        if (comparand is Urn)
            return this.Equals(comparand as Urn);

        return base.Equals(comparand);
    }

    /// <summary>
    /// Scheme and NID are case insensitive, NSS is case sensitive (except for the percent-encoded transformation where needed)
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Urn other)
    {
        if (UseCaseSensitiveUrns)
            return NID.Equals(other.NID) && NSS.Equals(other.NSS);

        return NID.Equals(other.NID, StringComparison.OrdinalIgnoreCase) && NSS.Equals(other.NSS, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return UseCaseSensitiveUrns == true
                ? HashCode.Combine(14923, NID.ToLower(), NSS)
                : Uri.GetHashCode();
        }
    }
}

