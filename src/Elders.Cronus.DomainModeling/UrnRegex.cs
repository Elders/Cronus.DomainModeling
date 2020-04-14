using System;

namespace Elders.Cronus
{
    public static class UrnRegex
    {
        public static string Pattern => @"\A(?i:urn:(?!urn:)(?<nid>[a-z0-9][a-z0-9-]{1,31}[^-]):(?<nss>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+)(?:\?\+(?<rcomponent>.*?))?(?:\?=(?<qcomponent>.*?))?(?:#(?<fcomponent>.*?))?)\z";

        public class Group
        {
            private string groupName;

            public Group(string groupName)
            {
                if (string.IsNullOrEmpty(groupName))
                    throw new ArgumentException(nameof(groupName));

                this.groupName = groupName;
            }

            public static Group NID => new Group("nid");
            public static Group NSS => new Group("nss");
            public static Group R_Component => new Group("rcomponent");
            public static Group Q_Component => new Group("qcomponent");
            public static Group F_Component => new Group("fcomponent");

            public override string ToString()
            {
                return groupName;
            }

            public static Group Create(string value)
            {
                switch (value.ToLowerInvariant())
                {
                    case "nid":
                        return NID;
                    case "nss":
                        return NSS;
                    case "rcomponent":
                        return R_Component;
                    case "qcomponent":
                        return Q_Component;
                    case "fcomponent":
                        return F_Component;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public static bool Matches(Uri urn)
        {
            var match = System.Text.RegularExpressions.Regex.Match(urn.AbsoluteUri, Pattern, System.Text.RegularExpressions.RegexOptions.None);

            return match.Success;
        }

        public static bool Matches(string urn)
        {
            var uri = new Uri(urn);
            return Matches(uri);
        }

        public static string GetGroup(string urnString, Group group)
        {
            var urn = new Uri(urnString);
            return GetGroup(urn, group);
        }

        public static string GetGroup(Uri urn, Group group)
        {
            var match = System.Text.RegularExpressions.Regex.Match(urn.AbsoluteUri, UrnRegex.Pattern, System.Text.RegularExpressions.RegexOptions.None);
            return match.Groups[group.ToString()].Value;
        }
    }
}
