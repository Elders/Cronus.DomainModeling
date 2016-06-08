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

        IList<string> Parts { get; }
    }

    public class Urn : IUrn
    {
        public const string Prefix = "urn";

        public const char DelimiterChar = ':';

        public const string Delimiter = ":";

        protected Urn() { }

        public Urn(string basePart, string valuePart)
        {
            Initialize(basePart.ToLowerInvariant(), valuePart.ToLowerInvariant());
        }

        public Urn(IUrn urn)
        {
            Initialize(urn.BasePart, urn.ValuePart);
        }

        protected void Initialize(string basePart, string valuePart)
        {
            BasePart = basePart;
            ValuePart = valuePart;

            if (string.IsNullOrEmpty(BasePart))
                Value = Prefix + Delimiter + valuePart;
            else
                Value = Prefix + Delimiter + basePart + Delimiter + valuePart;
        }

        public string BasePart { get; private set; }

        public string ValuePart { get; private set; }

        public string Value { get; private set; }

        public IList<string> Parts
        {
            get
            {
                return Value.Split(new[] { DelimiterChar });
            }
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(Urn urn)
        {
            return urn.Value;
        }
    }

    public class TenantUrn : Urn
    {
        public TenantUrn(string tenant, string value) : base(tenant, value) { }

        public TenantUrn(string urn)
        {
            var parts = urn.Split(new[] { DelimiterChar }).ToList();
            if (parts.Count < 3) throw new ArgumentException("Invalid Urn. Expected: urn:tenant:value", nameof(urn));

            var basePart = parts[1];
            var valuePart = string.Join(Delimiter, parts.Skip(2));
            Initialize(basePart, valuePart);
        }

        public TenantUrn(IUrn urn) : this(urn.Value) { }

        public string Tenant { get { return BasePart; } }

        public static implicit operator string(TenantUrn urn)
        {
            return urn.Value;
        }
    }
}