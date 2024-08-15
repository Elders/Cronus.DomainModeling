using System;
using System.Linq;
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

    protected Urn()
    {
        RawId = [];
    }

    public Urn(Urn urn)
    {
        if (urn is null) throw new ArgumentNullException(nameof(urn));

        RawId = new byte[urn.RawId.Length];
        urn.RawId.AsSpan().CopyTo(RawId);
    }

    public Urn(string urnString)
    {
        if (IsUrn(urnString) == false) throw new ArgumentException("String is not a URN!", nameof(urnString));

        if (UseCaseSensitiveUrns == false && urnString.Any(char.IsUpper))
            urnString = urnString.ToLower();

        RawId = Encoding.UTF8.GetBytes(urnString);
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

        if (UseCaseSensitiveUrns == false && urn.Any(char.IsUpper))
            urn = urn.ToLower();

        RawId = Encoding.UTF8.GetBytes(urn);
    }

    public Urn(ReadOnlySpan<char> urnSpan)
    {
        if (urnSpan.Length == 0) throw new ArgumentException("String is not a URN!", nameof(urnSpan));
        if (IsUrn(urnSpan) == false) throw new ArgumentException("String is not a URN!", nameof(urnSpan));

        if (UseCaseSensitiveUrns == false)
        {
            foreach (var c in urnSpan)
            {
                if (char.IsUpper(c))
                {
                    Span<char> chars = new char[urnSpan.Length];
                    urnSpan.ToLowerInvariant(chars);
                    urnSpan = chars;
                    break;
                }
            }
        }

        RawId = new byte[urnSpan.Length];
        Encoding.UTF8.GetBytes(urnSpan, RawId);
    }

    public Urn(ReadOnlySpan<char> nid, ReadOnlySpan<char> nss) : this(nid, nss, "", "", "") { }
    public Urn(ReadOnlySpan<char> nid, ReadOnlySpan<char> nss, ReadOnlySpan<char> rcomponent) : this(nid, nss, rcomponent, "", "") { }
    public Urn(ReadOnlySpan<char> nid, ReadOnlySpan<char> nss, ReadOnlySpan<char> rcomponent, ReadOnlySpan<char> qcomponent) : this(nid, nss, rcomponent, qcomponent, "") { }
    public Urn(ReadOnlySpan<char> nid, ReadOnlySpan<char> nss, ReadOnlySpan<char> rcomponent, ReadOnlySpan<char> qcomponent, ReadOnlySpan<char> fcomponent)
    {
        if (nid.Length == 0) throw new ArgumentException("NID is not valid", nameof(nid));
        if (nss.Length == 0) throw new ArgumentException("NSS is not valid", nameof(nss));

        var rcomponentLength = 0;
        var qcomponentLength = 0;
        var fcomponentLength = 0;

        if (rcomponent.Length > 0)
        {
            for (int i = 0; i < rcomponent.Length; i++)
            {
                if (i + 1 < rcomponent.Length && rcomponent[i] == PREFIX_Q_COMPONENT[0] && rcomponent[i + 1] == PREFIX_Q_COMPONENT[1])
                    throw new ArgumentException("rcomponent includes illegal characters!", nameof(rcomponent));

                if (rcomponent[i] == PREFIX_F_COMPONENT[0])
                    throw new ArgumentException("rcomponent includes illegal characters!", nameof(rcomponent));
            }

            rcomponentLength = rcomponent.StartsWith(PREFIX_R_COMPONENT) ? rcomponent.Length : rcomponent.Length + PREFIX_R_COMPONENT.Length;
        }

        if (qcomponent.Length > 0)
        {
            for (int i = 0; i < qcomponent.Length; i++)
            {
                if (i + 1 < qcomponent.Length && qcomponent[i] == PREFIX_R_COMPONENT[0] && qcomponent[i + 1] == PREFIX_R_COMPONENT[1])
                    throw new ArgumentException("qcomponent includes illegal characters!", nameof(qcomponent));

                if (qcomponent[i] == PREFIX_F_COMPONENT[0])
                    throw new ArgumentException("qcomponent includes illegal characters!", nameof(qcomponent));
            }

            qcomponentLength = qcomponent.StartsWith(PREFIX_Q_COMPONENT) ? qcomponent.Length : qcomponent.Length + PREFIX_Q_COMPONENT.Length;
        }

        if (fcomponent.Length > 0)
        {
            for (int i = 0; i < fcomponent.Length; i++)
            {
                if (i + 1 < fcomponent.Length && fcomponent[i] == PREFIX_R_COMPONENT[0] && fcomponent[i + 1] == PREFIX_R_COMPONENT[1])
                    throw new ArgumentException("fcomponent includes illegal characters!", nameof(fcomponent));

                if (i + 1 < fcomponent.Length && fcomponent[i] == PREFIX_Q_COMPONENT[0] && fcomponent[i + 1] == PREFIX_Q_COMPONENT[1])
                    throw new ArgumentException("fcomponent includes illegal characters!", nameof(fcomponent));
            }

            fcomponentLength = fcomponent.StartsWith(PREFIX_F_COMPONENT) ? fcomponent.Length : fcomponent.Length + PREFIX_F_COMPONENT.Length;
        }

        Span<char> urn = stackalloc char[5 + nid.Length + nss.Length + rcomponentLength + qcomponentLength + fcomponentLength];
        urn[0] = 'u';
        urn[1] = 'r';
        urn[2] = 'n';
        urn[3] = PARTS_DELIMITER;
        var index = 4;
        nid.CopyTo(urn.Slice(index, nid.Length));
        index += nid.Length;
        urn[index++] = PARTS_DELIMITER;
        nss.CopyTo(urn.Slice(index, nss.Length));
        index += nss.Length;
        if (rcomponent.Length > 0)
        {
            if (rcomponent.StartsWith(PREFIX_R_COMPONENT) == false)
            {
                PREFIX_R_COMPONENT.CopyTo(urn.Slice(index, PREFIX_R_COMPONENT.Length));
                index += PREFIX_R_COMPONENT.Length;
            }

            rcomponent.CopyTo(urn.Slice(index, rcomponent.Length));
            index += rcomponent.Length;
        }

        if (qcomponent.Length > 0)
        {
            if (qcomponent.StartsWith(PREFIX_Q_COMPONENT) == false)
            {
                PREFIX_Q_COMPONENT.CopyTo(urn.Slice(index, PREFIX_Q_COMPONENT.Length));
                index += PREFIX_Q_COMPONENT.Length;
            }

            qcomponent.CopyTo(urn.Slice(index, qcomponent.Length));
            index += qcomponent.Length;
        }


        if (fcomponent.Length > 0)
        {
            if (fcomponent.StartsWith(PREFIX_F_COMPONENT) == false)
            {
                PREFIX_F_COMPONENT.CopyTo(urn.Slice(index, PREFIX_F_COMPONENT.Length));
                index += PREFIX_F_COMPONENT.Length;
            }

            fcomponent.CopyTo(urn.Slice(index, fcomponent.Length));
        }

        if (UseCaseSensitiveUrns == false)
        {
            for (int i = 0; i < urn.Length; i++)
            {
                if (char.IsUpper(urn[i]))
                    urn[i] = char.ToLower(urn[i]);
            }
        }

        RawId = new byte[urn.Length];
        Encoding.UTF8.GetBytes(urn, RawId);
    }

    protected string nid;
    protected string nss;
    protected string r_Component;
    protected string q_Component;
    protected string f_Component;

    private void SetUri(string uri)
    {
        System.Text.RegularExpressions.Match match = UrnRegex.Match(uri);
        nid = match.Groups[UrnRegex.Group.NID.ToString()].Value;
        nss = match.Groups[UrnRegex.Group.NSS.ToString()].Value;
        r_Component = match.Groups[UrnRegex.Group.R_Component.ToString()].Value;
        q_Component = match.Groups[UrnRegex.Group.Q_Component.ToString()].Value;
        f_Component = match.Groups[UrnRegex.Group.F_Component.ToString()].Value;
    }

    [DataMember(Order = 10)]
    public byte[] RawId { get; protected set; }

    private bool isFullyInitialized;
    protected virtual void DoFullInitialization()
    {
        if (isFullyInitialized == false)
        {
            SetUri(Encoding.UTF8.GetString(RawId)); // TODO: do not allocate
            isFullyInitialized = true;
        }
    }

    private static string BuildUrnString(ReadOnlySpan<char> nid, ReadOnlySpan<char> nss, ReadOnlySpan<char> rcomponent, ReadOnlySpan<char> qcomponent, ReadOnlySpan<char> fcomponent)
    {
        var urn = new StringBuilder($"urn{PARTS_DELIMITER}{nid}{PARTS_DELIMITER}{nss}");

        if (rcomponent.Length > 0)
        {
            if (rcomponent.IndexOf(PREFIX_Q_COMPONENT) > -1 || rcomponent.IndexOf(PREFIX_F_COMPONENT) > -1)
                throw new ArgumentException("rcomponent includes illegal characters!", nameof(rcomponent));

            urn.Append(rcomponent.StartsWith(PREFIX_R_COMPONENT) ? rcomponent : $"{PREFIX_R_COMPONENT}{rcomponent}");
        }

        if (qcomponent.Length > 0)
        {
            if (qcomponent.IndexOf(PREFIX_R_COMPONENT) > -1 || qcomponent.IndexOf(PREFIX_F_COMPONENT) > -1)
                throw new ArgumentException("qcomponent includes illegal characters!", nameof(qcomponent));

            urn.Append(rcomponent.StartsWith(PREFIX_Q_COMPONENT) ? qcomponent : $"{PREFIX_Q_COMPONENT}{qcomponent}");
        }

        if (fcomponent.Length > 0)
        {
            if (fcomponent.IndexOf(PREFIX_R_COMPONENT) > -1 || fcomponent.IndexOf(PREFIX_Q_COMPONENT) > -1)
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

    public string Value => Encoding.UTF8.GetString(RawId);

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

    public static bool IsUrn(ReadOnlySpan<char> candidate)
    {
        if (candidate.Length == 0)
            return false;

        return UrnRegex.Matches(candidate);
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
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (UseCaseSensitiveUrns)
            return NID.Equals(other.NID) && NSS.Equals(other.NSS);

        return NID.Equals(other.NID, StringComparison.OrdinalIgnoreCase) && NSS.Equals(other.NSS, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            if (UseCaseSensitiveUrns)
                return HashCode.Combine(14923, NID.ToLower(), NSS);

            const int p = 16777619;
            int hash = (int)2166136261;

            for (int i = 0; i < RawId.Length; i++)
                hash = (hash ^ RawId[i]) * p;

            return hash;
        }
    }
}

