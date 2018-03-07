namespace Elders.Cronus.DomainModeling
{
    public interface IEventHandler<in T>
        where T : IEvent
    {
        void Handle(T @event);
    }
}
