using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elders.Cronus;

public interface IPublisher<in TMessage> where TMessage : IMessage
{
    /// <summary>
    /// Publishes the specified message.
    /// </summary>
    /// <param name="message">The message</param>
    /// <param name="messageHeaders">The message headers</param>
    /// <returns>Returns true if sending the message was successful.</returns>
    Task<bool> PublishAsync(TMessage message, Dictionary<string, string> messageHeaders = null);

    /// <summary>
    /// Publishes the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="publishAt">The specific point in time when the message will be published.</param>
    /// <returns>Returns true if sending the message was successful.</returns>
    Task<bool> PublishAsync(TMessage message, DateTime publishAt, Dictionary<string, string> messageHeaders = null);

    /// <summary>
    /// Publishes the specified message.
    /// </summary>
    /// <param name="message">The message</param>
    /// <param name="publishAfter">The timespan after current time which the message will be published.</param>
    /// <param name="messageHeaders">The message headers.</param>
    /// <returns>Returns true if sending the message was successful.</returns>
    Task<bool> PublishAsync(TMessage message, TimeSpan publishAfter, Dictionary<string, string> messageHeaders = null);

    /// <summary>
    /// Publishes the specified message bytes. You need to take care about the correct <paramref name="messageType"/> and <paramref name="tenant"/>
    /// </summary>
    /// <param name="messageRaw">The message bytes.</param>
    /// <param name="messageType">The type of the message.</param>
    /// <param name="tenant">The tenant of the message</param>
    /// <param name="messageHeaders">The message headers.</param>
    /// <returns>Returns true if sending the message was successful.</returns>
    Task<bool> PublishAsync(byte[] messageRaw, Type messageType, string tenant, Dictionary<string, string> messageHeaders);
}
