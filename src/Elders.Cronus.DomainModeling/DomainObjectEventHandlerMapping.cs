using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Elders.Cronus.DomainModeling
{

    public class DomainObjectEventHandlerMapping
    {
        public Dictionary<Type, Action<IEvent>> GetEventHandlers(Func<object> target)
        {
            var targetType = target().GetType();
            var handlers = new Dictionary<Type, Action<IEvent>>();

            var methodsToMatch = targetType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            var matchedMethods = from method in methodsToMatch
                                 let parameters = method.GetParameters()
                                 where
                                    method.Name.Equals("when", StringComparison.InvariantCultureIgnoreCase) &&
                                    parameters.Length == 1 &&
                                    typeof(IEvent).IsAssignableFrom(parameters[0].ParameterType)
                                 select
                                    new { MethodInfo = method, FirstParameter = method.GetParameters()[0] };

            foreach (var method in matchedMethods)
            {
                var methodCopy = method.MethodInfo;
                Type eventType = methodCopy.GetParameters().First().ParameterType;

                Action<IEvent> handler = (e) => methodCopy.Invoke(target(), new[] { e });

                handlers.Add(eventType, handler);
            }

            return handlers;
        }
    }
}