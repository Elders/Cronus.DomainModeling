using System.Collections.Generic;

namespace Elders.Cronus.DomainModeling
{
    public interface IPublisher<in TMessage>
        where TMessage : IMessage
    {
        /// <summary>
        /// Publishes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        bool Publish(TMessage message, Dictionary<string, string> messageHeaders = null);
    }
}
