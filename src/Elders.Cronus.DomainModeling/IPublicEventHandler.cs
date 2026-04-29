using System.Threading.Tasks;

namespace Elders.Cronus;

public interface IPublicEventHandler<in T>
    where T : IPublicEvent
{
    Task HandleAsync(T @event);
}
