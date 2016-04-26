using System;
using System.Collections.Generic;
using System.Linq;

namespace Elders.Cronus.DomainModeling
{
    public interface IUrn
    {
        string BasePart { get; }

        string ValuePart { get; }

        string Value { get; }
    }

    public class Urn : IUrn
    {
        public const string Prefix = "urn";

        public const char DelimiterChar = ':';

        public const string Delimiter = ":";

        protected Urn() { }

        public Urn(string basePart, string valuePart)
        {
            BasePart = basePart;
            ValuePart = valuePart;
            if (string.IsNullOrEmpty(BasePart))
                Value = Prefix + Delimiter + valuePart;
            else
                Value = Prefix + Delimiter + basePart + Delimiter + valuePart;
        }

        public Urn(string value)
        {
            BasePart = string.Empty;
            ValuePart = value.Remove(0, 4);
            Value = value;
        }

        public Urn(IUrn urn)
        {
            BasePart = urn.BasePart;
            ValuePart = urn.ValuePart;
            Value = urn.Value;
        }

        public string BasePart { get; protected set; }

        public string ValuePart { get; protected set; }

        public string Value { get; protected set; }

        public override string ToString()
        {
            return Value;
        }
    }

    public class TenantUrn : Urn
    {
        readonly List<string> parts;

        public TenantUrn(string tenant, string value) : base(tenant, value) { }

        public TenantUrn(string urn)
        {
            parts = urn.Split(new[] { DelimiterChar }).ToList();
            if (parts.Count < 3) throw new ArgumentException("Invalid Urn. Expected: urn:tenant:value:etc", nameof(urn));

            BasePart = parts[1];
            ValuePart = string.Join(Delimiter, parts.Skip(2));
            Value = urn;
        }

        public TenantUrn(IUrn urn) : this(urn.Value)
        {
        }

        public string Tenant { get { return BasePart; } }

        public IList<string> Parts { get { return parts.AsReadOnly(); } }
    }
}