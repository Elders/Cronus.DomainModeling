using System;
using System.Collections.Generic;

namespace Elders.Cronus;

public interface IPublisher<in TMessage> where TMessage : IMessage
{
    /// <summary>
    /// Publishes the speicified message.
    /// </summary>
    /// <param name="message">The message</param>
    /// <param name="messageHeaders">The message headers</param>
    /// <returns>Returns true if sending the message is successful</returns>
    bool Publish(TMessage message, Dictionary<string, string> messageHeaders = null);

    /// <summary>
    /// Publishes the speicified message.
    /// </summary>
    /// <param name="message">The message</param>
    /// <param name="publishAt">The specific point in time when the message will be published</param>
    /// <returns>Returns true if sending the message is successful</returns>
    bool Publish(TMessage message, DateTime publishAt, Dictionary<string, string> messageHeaders = null);

    /// <summary>
    /// Publishes the speicified message.
    /// </summary>
    /// <param name="message">The message</param>
    /// <param name="publishAfter">The timespan after current time which the message will be published</param>
    /// <param name="messageHeaders">The message headers</param>
    /// <returns>Returns true if sending the message is successful</returns>
    bool Publish(TMessage message, TimeSpan publishAfter, Dictionary<string, string> messageHeaders = null);
}
