using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Elders.Cronus;

public static class MessageInfo
{
    private static readonly ConcurrentDictionary<Type, string> typeToContract = new ConcurrentDictionary<Type, string>();
    private static readonly ConcurrentDictionary<string, Type> contractToType = new ConcurrentDictionary<string, Type>();
    private static readonly ConcurrentDictionary<Type, string> typeToBoundedContext = new ConcurrentDictionary<Type, string>();

    public static string GetContractId(this Type messageType)
    {
        string messageId;
        if (!typeToContract.TryGetValue(messageType, out messageId))
        {
            messageId = GetAndCacheContractIdFromAttribute(messageType);
        }
        return messageId;
    }

    public static string GetBoundedContext(this Type messageType, string defaultBoundedContext = "implicit")
    {
        string boundedContext;
        if (!typeToBoundedContext.TryGetValue(messageType, out boundedContext))
        {
            boundedContext = GetAndCacheBoundedContextFromAttribute(messageType, defaultBoundedContext);
        }
        return boundedContext;
    }

    public static Type GetTypeByContract(this string contractId)
    {
        Type theType;
        if (contractToType.TryGetValue(contractId, out theType) == false)
        {
            string uppercaseContractId = contractId.ToUpper(); // Keep in mind that contractId is case-sensitive. We are doing this check because Cronus is using lowercase contractIds everywhere,
                                                               // but the developer can set the contractId in uppercase inside the DataContract this causes issues, so we want to detect it and force the developer to use lowercase contracts.
            if (contractToType.TryGetValue(uppercaseContractId, out theType))
            {
                throw new Exception($"Unable to resolve type using contractId `{uppercaseContractId}`. The problem is that lowercase contract must be used, please fix it. The contract was found in '{theType.FullName}'.");
            }

            throw new Exception($"Unable to resolve type using contractId `{contractId}`. Most probably the type with this contractId is deleted from the source code and there is an existing record in the database which still uses it.");
        }
        return theType;
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

    private static string GetAndCacheBoundedContextFromAttribute(Type contractType, string defaultBoundedContext)
    {
        string boundedContext = defaultBoundedContext;
        DataContractAttribute contract = contractType
            .GetCustomAttributes(false).Where(attr => attr is DataContractAttribute)
            .SingleOrDefault() as DataContractAttribute;

        if (contract is null == false && contract.IsNamespaceSetExplicitly)
            boundedContext = contract.Namespace;

        boundedContext = boundedContext.ToLower(); // Keep in mind that rabbitmq exchanges and queues are case-sensitive

        typeToBoundedContext.TryAdd(contractType, boundedContext);

        return boundedContext;
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
