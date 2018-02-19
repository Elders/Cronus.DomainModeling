using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Elders.Cronus
{
    public static class MessageInfo
    {
        private const string MissingBoundedContextAttribute = @"The assembly '{0}' is missing a BoundedContext attribute in AssemblyInfo.cs! Example: [BoundedContext(""Company.Product.BoundedContext"")]";

        private static readonly ConcurrentDictionary<string, BoundedContextAttribute> boundedContexts = new ConcurrentDictionary<string, BoundedContextAttribute>();

        private static readonly ConcurrentDictionary<Type, string> typeToContract = new ConcurrentDictionary<Type, string>();
        private static readonly ConcurrentDictionary<string, Type> contractToType = new ConcurrentDictionary<string, Type>();

        public static BoundedContextAttribute GetBoundedContext(this Type contractType)
        {
            BoundedContextAttribute boundedContext;
            string contractId;
            if (typeToContract.TryGetValue(contractType, out contractId))
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
            if (!typeToContract.TryGetValue(messageType, out messageId))
            {
                messageId = GetAndCacheContractIdFromAttribute(messageType);
            }
            return messageId;
        }

        public static Type GetTypeByContract(this string contractId)
        {
            Type theType;
            if (contractToType.TryGetValue(contractId, out theType) == false)
            {
                throw new Exception("I knew this will not gonna work... :(");
            }
            return theType;
        }

        public static string ToString(this IMessage message, string messageInfo)
        {
            var bcNamespace = message.GetType().GetBoundedContext().BoundedContextNamespace;
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
                throw new Exception(String.Format(@"The message type '{0}' is missing a DataContract attribute. Example: [DataContract(""00000000-0000-0000-0000-000000000000"")]", contractType.FullName));
            }
            else
            {
                contractId = contract.Name;
            }

            typeToContract.TryAdd(contractType, contractId);
            contractToType.TryAdd(contractId, contractType);
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
    }
}
