using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Elders.Cronus;

internal static class DomainObjectEventHandlerMapping
{
    private static readonly Type eventType = typeof(IEvent);

    public static List<MethodInfo> GetEventHandlersMethodInfo(Type stateType)
    {
        var methodsToMatch = stateType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        List<MethodInfo> result = [];
        foreach (var method in methodsToMatch)
        {
            var parameters = method.GetParameters();
            if (method.Name.Equals("when", StringComparison.OrdinalIgnoreCase)
                && parameters.Length == 1
                && eventType.IsAssignableFrom(parameters[0].ParameterType))
            {
                result.Add(method);
            }
        }

        return result;
    }

    public static Dictionary<Type, Action<IEvent>> GetEventHandlers(IEnumerable<MethodInfo> whenMethods, Func<object> target)
    {
        var targetType = target().GetType();
        var handlers = new Dictionary<Type, Action<IEvent>>();

        foreach (var method in whenMethods)
        {
            Type eventType = method.GetParameters().First().ParameterType;

            Action<IEvent> handler = (e) => method.Invoke(target(), [e]);

            handlers.Add(eventType, handler);
        }

        return handlers;
    }
}
