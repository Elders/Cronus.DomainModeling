using System.Threading.Tasks;

namespace Elders.Cronus;

public interface IEventHandler<in T>
    where T : IEvent
{
    Task HandleAsync(T @event);
}
