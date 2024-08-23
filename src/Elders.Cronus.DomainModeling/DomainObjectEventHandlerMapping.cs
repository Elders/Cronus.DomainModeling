using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Elders.Cronus;

internal static class DomainObjectEventHandlerMapping
{
    public static MethodInfo[] GetEventHandlersMethodInfo(Type stateType)
    {
        var methodsToMatch = stateType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        return (from method in methodsToMatch
                let parameters = method.GetParameters()
                where
                   method.Name.Equals("when", StringComparison.OrdinalIgnoreCase) &&
                   parameters.Length == 1 &&
                   typeof(IEvent).IsAssignableFrom(parameters[0].ParameterType)
                select method).ToArray();
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
