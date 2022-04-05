namespace Elders.Cronus;

public interface IEventHandler<in T>
    where T : IEvent
{
    void Handle(T @event);
}
