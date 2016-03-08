using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Elders.Cronus.DomainModeling
{
    public static class MessageInfo
    {
        private const string MissingBoundedContextAttribute = @"The assembly '{0}' is missing a BoundedContext attribute in AssemblyInfo.cs! Example: [BoundedContext(""Company.Product.BoundedContext"")]";

        private static readonly ConcurrentDictionary<string, BoundedContextAttribute> boundedContexts = new ConcurrentDictionary<string, BoundedContextAttribute>();

        private static readonly ConcurrentDictionary<Type, string> contractIds = new ConcurrentDictionary<Type, string>();

        public static BoundedContextAttribute GetBoundedContext(this Type contractType)
        {
            BoundedContextAttribute boundedContext;
            string contractId;
            if (contractIds.TryGetValue(contractType, out contractId))
            {
                if (boundedContexts.TryGetValue(contractId, out boundedContext))
                    return boundedContext;
            }

            contractId = contractType.GetContractId();
            return contractType.GetAndCacheBoundedContextFromAttribute(contractId);
        }

        public static BoundedContextAttribute GetBoundedContext(this Assembly contractAssembly)
        {
            BoundedContextAttribute boundedContext;
            if (boundedContexts.TryGetValue(contractAssembly.FullName, out boundedContext))
                return boundedContext;

            boundedContext = contractAssembly.GetAssemblyAttribute<BoundedContextAttribute>();
            if (boundedContext == default(BoundedContextAttribute))
                throw new Exception(String.Format(MissingBoundedContextAttribute, contractAssembly.FullName));

            boundedContexts.TryAdd(contractAssembly.FullName, boundedContext);
            return boundedContext;
        }

        public static string GetContractId(this Type messageType)
        {
            string messageId;
            if (!contractIds.TryGetValue(messageType, out messageId))
            {
                messageId = GetAndCacheContractIdFromAttribute(messageType);
            }
            return messageId;
        }

        public static string ToString(this IMessage message, string info, params object[] args)
        {
            var bcNamespace = message.GetType().GetBoundedContext().BoundedContextNamespace;
            var messageInfo = String.Format(info, args);
            return "[" + bcNamespace + "] " + messageInfo;
        }

        private static BoundedContextAttribute GetAndCacheBoundedContextFromAttribute(this Type contractType, string contractId)
        {
            var boundedContext = contractType.Assembly.GetAssemblyAttribute<BoundedContextAttribute>();

            if (boundedContext == default(BoundedContextAttribute))
                throw new Exception(String.Format(MissingBoundedContextAttribute, contractType.Assembly.FullName));

            boundedContexts.TryAdd(contractId, boundedContext);
            return boundedContext;
        }

        private static string GetAndCacheContractIdFromAttribute(Type contractType)
        {
            string contractId;
            DataContractAttribute contract = contractType
                .GetCustomAttributes(false).Where(attr => attr is DataContractAttribute)
                .SingleOrDefault() as DataContractAttribute;

            if (contract == null || String.IsNullOrEmpty(contract.Name))
            {
                if (typeof(IProjection).IsAssignableFrom(contractType) ||
                    typeof(IPort).IsAssignableFrom(contractType) ||
                    typeof(IAggregateRootApplicationService).IsAssignableFrom(contractType))
                    contractId = contractType.GetHashCode().ToString();
                else
                    throw new Exception(String.Format(@"The message type '{0}' is missing a DataContract attribute. Example: [DataContract(""00000000-0000-0000-0000-000000000000"")]", contractType.FullName));
            }
            else
            {
                contractId = contract.Name;
            }

            contractIds.TryAdd(contractType, contractId);
            return contractId;
        }

    }

    public static class ReflectionExtensions
    {
        public static T GetAssemblyAttribute<T>(this Type type)
        {
            return GetAssemblyAttribute<T>(type.Assembly);
        }

        public static T GetAssemblyAttribute<T>(this Assembly assembly)
        {
            var attributeType = typeof(T);
            var attribute = assembly
                .GetCustomAttributes(attributeType, false)
                .SingleOrDefault();

            return attribute == null
                ? default(T)
                : (T)attribute;
        }

        public static IEnumerable<MemberInfo> GetAllMembers(this Type t)
        {
            if (t == null)
                return Enumerable.Empty<MemberInfo>();

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                                 BindingFlags.Static | BindingFlags.Instance |
                                 BindingFlags.DeclaredOnly;
            return t.GetMembers(flags).Concat(GetAllMembers(t.BaseType));
        }

        public static bool HasAttribute<TAttribute>(this ICustomAttributeProvider self)
        {
            return self
                .GetCustomAttributes(false)
                .Where(x => x is TAttribute)
                .Any();
        }

        public static TResultType GetAttrubuteValue<TAttribute, TResultType>(this ICustomAttributeProvider self, Func<TAttribute, TResultType> get)
        {
            TAttribute attribute = (TAttribute)self.GetCustomAttributes(typeof(TAttribute), false).Single();
            return get(attribute);
        }

        private static object GetValue(object instance, MemberInfo member)
        {
            if (member is PropertyInfo)
            {
                return ((member as PropertyInfo)).GetValue(instance);
            }
            else if (member is FieldInfo)
            {
                return ((member as FieldInfo)).GetValue(instance);
            }
            else
                throw new NotImplementedException();
        }
    }
}
