using System;

namespace Elders.Cronus
{
    public static class UrnRegex
    {
        public const string Pattern = @"\A(?i:urn:(?!urn:)(?<nid>[\w][\w-]{0,30}[\w]):(?<nss>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+)(?:\?\+(?<rcomponent>.*?))?(?:\?=(?<qcomponent>.*?))?(?:#(?<fcomponent>.*?))?)\z";

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
                switch ((value ?? string.Empty).ToLower())
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
                        throw new NotSupportedException($"The group {value} is not supported by UrnRegex");
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
            try
            {
                var match = System.Text.RegularExpressions.Regex.Match(urn, Pattern, System.Text.RegularExpressions.RegexOptions.None);
                if (match.Success == false)
                    return false;

                var uri = new Uri(urn);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
