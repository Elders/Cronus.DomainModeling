﻿using System;
using System.Buffers;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Elders.Cronus;

[DataContract(Name = "d3ff08b5-38e2-4aaf-b3a8-ccc423ed096d")]
public class Urn : IEquatable<Urn>, IBlobId
{
    public const char PARTS_DELIMITER = ':';
    public const char HIERARCHICAL_DELIMITER = '/';
    public const string UriSchemeUrn = "urn";
    public const string PREFIX_R_COMPONENT = "?+";
    public const string PREFIX_Q_COMPONENT = "?=";
    public const string PREFIX_F_COMPONENT = "#";

    /// <summary>
    /// Specifies if the URNs will follow strictly the rfc(https://tools.ietf.org/html/rfc8141)
    /// We do recommend to use case insensitive urns. Enable this feature if you really need it.
    /// </summary>
    public static bool UseCaseSensitiveUrns = false;

    private static readonly SearchValues<char> allowedNidSymbols = SearchValues.Create("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-");
    private static readonly SearchValues<char> allowedNssSymbols = SearchValues.Create("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-()+,.:=@;$_!*'&~/%");

    protected string nid;
    protected string nss;
    protected string r_Component;
    protected string q_Component;
    protected string f_Component;

    private bool isFullyInitialized;
    private ReadOnlyMemory<byte> rawId;

    protected Urn()
    {
        rawId = ReadOnlyMemory<byte>.Empty;
    }

    public Urn(Urn urn)
    {
        if (urn is null) throw new ArgumentNullException(nameof(urn));

        SetRawId(urn.rawId);
    }

    public Urn(ReadOnlySpan<char> urnSpan)
    {
        if (urnSpan.Length == 0) throw new ArgumentException("Empty string is not a valid URN!", nameof(urnSpan));
        if (IsUrn(urnSpan) == false) throw new ArgumentException("String is not a valid URN!", nameof(urnSpan));

        bool isRawIdSet = false;
        if (UseCaseSensitiveUrns == false)
        {
            foreach (var c in urnSpan)
            {
                if (char.IsUpper(c))
                {
                    Span<char> chars = stackalloc char[urnSpan.Length];
                    urnSpan.ToLower(chars, null);

                    SetRawId(chars);
                    isRawIdSet = true;

                    break;
                }
            }
        }

        if (isRawIdSet == false)
            SetRawId(urnSpan);
    }

    public Urn(ReadOnlySpan<char> nid, ReadOnlySpan<char> nss) : this(nid, nss, [], [], []) { }
    public Urn(ReadOnlySpan<char> nid, ReadOnlySpan<char> nss, ReadOnlySpan<char> rcomponent) : this(nid, nss, rcomponent, [], []) { }
    public Urn(ReadOnlySpan<char> nid, ReadOnlySpan<char> nss, ReadOnlySpan<char> rcomponent, ReadOnlySpan<char> qcomponent) : this(nid, nss, rcomponent, qcomponent, []) { }
    public Urn(ReadOnlySpan<char> nid, ReadOnlySpan<char> nss, ReadOnlySpan<char> rcomponent, ReadOnlySpan<char> qcomponent, ReadOnlySpan<char> fcomponent)
    {
        if (nid.Length < 2 || nid.Length > 32) throw new ArgumentOutOfRangeException(nameof(nid), "NID must be at least 2 and less than 32 symbols");
        if (nid[0] == '-' || nid[^1] == '-') throw new ArgumentException("NID cannot start or end with '-'", nameof(nid));
        if (nid.Contains("urn:", StringComparison.OrdinalIgnoreCase)) throw new ArgumentException("NID cannot contain the string 'urn'", nameof(nid));
        if (nid.ContainsAnyExcept(allowedNidSymbols)) throw new ArgumentException($"NID is not valid {nid}", nameof(nid));
        if (nss.ContainsAnyExcept(allowedNssSymbols)) throw new ArgumentException($"NSS is not valid {nss}", nameof(nss));

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

        ConvertCaseIfNeeded(urn);
        SetRawId(urn);

        bool isValid = IsUrn(urn); // Validate the URN against the regex
        if (isValid == false)
        {
            var exception = new FormatException($"Invalid URN format.");
            exception.Data.Add("urnValue", urn.ToString());
            throw exception;
        }

        DoFullInitialization();
    }

    internal void SetRawId(ReadOnlyMemory<byte> memory)
    {
        Memory<byte> buffer = new byte[memory.Length];
        memory.CopyTo(buffer);
        rawId = buffer;
        isFullyInitialized = false;
    }

    internal void SetRawId(ReadOnlySpan<char> span)
    {
        Memory<byte> buffer = new byte[span.Length];
        Encoding.UTF8.GetBytes(span, buffer.Span);
        rawId = buffer;
        isFullyInitialized = false;
    }

    private protected static void ConvertCaseIfNeeded(Span<char> urn)
    {
        if (UseCaseSensitiveUrns == false)
        {
            for (int i = 0; i < urn.Length; i++)
            {
                if (char.IsUpper(urn[i]))
                    urn[i] = char.ToLower(urn[i]);
            }
        }
    }

    [DataMember(Order = 10)]
    public ReadOnlyMemory<byte> RawId { get => rawId; protected set { rawId = value; isFullyInitialized = false; } }

    public string NID { get { DoFullInitialization(); return nid; } }

    public string NSS { get { DoFullInitialization(); return nss; } }

    public string R_Component { get { DoFullInitialization(); return r_Component; } }

    public string Q_Component { get { DoFullInitialization(); return q_Component; } }

    public string F_Component { get { DoFullInitialization(); return f_Component; } }

    public string Value => Encoding.UTF8.GetString(RawId.Span);

    protected virtual void DoFullInitialization()
    {
        if (isFullyInitialized == false)
        {
            Span<char> rawChars = stackalloc char[RawId.Length];
            Encoding.UTF8.GetChars(RawId.Span, rawChars);

            int rCompIndex = -1, qCompIndex = -1, fCompIndex = -1;
            for (int i = 4; i < rawChars.Length; i++) // skipping "urn:"
            {
                if (string.IsNullOrEmpty(nid) && rawChars[i] == PARTS_DELIMITER)
                {
                    nid = rawChars[4..i].ToString();
                    continue;
                }

                if (string.IsNullOrEmpty(nss) && i == rawChars.Length - 1)
                {
                    nss = rawChars[(4 + nid.Length + 1)..].ToString();
                    break;
                }

                if (i + 1 < rawChars.Length && rawChars[i] == PREFIX_R_COMPONENT[0] && rawChars[i + 1] == PREFIX_R_COMPONENT[1])
                {
                    if (string.IsNullOrEmpty(nss))
                        nss = rawChars[(4 + nid.Length + 1)..i].ToString();
                    rCompIndex = i;
                    continue;
                }

                if (i + 1 < rawChars.Length && rawChars[i] == PREFIX_Q_COMPONENT[0] && rawChars[i + 1] == PREFIX_Q_COMPONENT[1])
                {
                    if (string.IsNullOrEmpty(nss))
                        nss = rawChars[(4 + nid.Length + 1)..i].ToString();
                    qCompIndex = i;
                    continue;
                }

                if (rawChars[i] == PREFIX_F_COMPONENT[0])
                {
                    if (string.IsNullOrEmpty(nss))
                        nss = rawChars[(4 + nid.Length + 1)..i].ToString();
                    fCompIndex = i;
                    continue;
                }
            }

            if (string.IsNullOrEmpty(nid) || string.IsNullOrEmpty(nss))
                throw new UnreachableException("How did I get here?!");

            if (rCompIndex > -1)
            {
                if (qCompIndex > -1)
                    r_Component = rawChars[(4 + nid.Length + nss.Length + 3)..qCompIndex].ToString();
                else if (fCompIndex > -1)
                    r_Component = rawChars[(4 + nid.Length + nss.Length + 3)..fCompIndex].ToString();
                else
                    r_Component = rawChars[(4 + nid.Length + nss.Length + 3)..].ToString();
            }

            if (qCompIndex > -1)
            {
                if (fCompIndex > -1)
                    q_Component = rawChars[(4 + nid.Length + nss.Length + (r_Component?.Length > 0 ? r_Component.Length + 2 : 0) + 3)..fCompIndex].ToString();
                else
                    q_Component = rawChars[(4 + nid.Length + nss.Length + (r_Component?.Length > 0 ? r_Component.Length + 2 : 0) + 3)..].ToString();
            }

            if (fCompIndex > -1)
            {
                f_Component = rawChars[(4 + nid.Length + nss.Length + (r_Component?.Length > 0 ? r_Component.Length + 2 : 0) + (q_Component.Length > 0 ? q_Component.Length + 2 : q_Component.Length) + 2)..].ToString();
            }

            isFullyInitialized = true;
        }
    }

    public override string ToString() => Value;

    public static implicit operator string(Urn urn) => urn?.Value;
    public static implicit operator ReadOnlySpan<byte>(Urn urn) => urn is null ? [] : urn.RawId.Span;

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

    public static bool IsUrn(ReadOnlySpan<char> candidate)
    {
        if (candidate.Length == 0)
            return false;

        return UrnRegex.Matches(candidate);
    }

    public override bool Equals(object comparand) => Equals(comparand as Urn);

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
            return NID.Equals(other.NID, StringComparison.Ordinal) && NSS.Equals(other.NSS, StringComparison.Ordinal);

        return MemoryExtensions.Equals(NID, other.NID, StringComparison.OrdinalIgnoreCase)
            && MemoryExtensions.Equals(NSS, other.NSS, StringComparison.OrdinalIgnoreCase);
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
                hash = (hash ^ RawId.Span[i]) * p;

            return hash;
        }
    }
}

