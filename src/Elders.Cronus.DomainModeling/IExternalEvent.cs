namespace Elders.Cronus
{
    /// <summary>
    /// External domain events represent business changes which already happened in another bounded context
    /// </summary>
    public interface IExternalEvent : IMessage { }
}
