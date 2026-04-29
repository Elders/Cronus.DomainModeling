using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Elders.Cronus;

public interface IPublisher<in TMessage> where TMessage : IMessage
{
    /// <summary>
    /// Publishes the specified message asynchronously.
    /// </summary>
    /// <param name="message">The message to publish.</param>
    /// <param name="messageHeaders">Optional headers to attach to the message.</param>
    /// <param name="cancellationToken">A token to cancel the in-flight publish.</param>
    /// <returns>Returns true if sending the message was successful.</returns>
    Task<bool> PublishAsync(TMessage message, Dictionary<string, string> messageHeaders = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Schedules the specified message to be published at a specific point in time.
    /// </summary>
    /// <param name="message">The message to publish.</param>
    /// <param name="publishAt">The UTC point in time when the message will be published.</param>
    /// <param name="messageHeaders">Optional headers to attach to the message.</param>
    /// <param name="cancellationToken">A token to cancel the in-flight publish (does not cancel the scheduled delivery itself).</param>
    /// <returns>Returns true if sending the message was successful.</returns>
    Task<bool> PublishAsync(TMessage message, DateTime publishAt, Dictionary<string, string> messageHeaders = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Schedules the specified message to be published after the given delay.
    /// </summary>
    /// <param name="message">The message to publish.</param>
    /// <param name="publishAfter">The timespan after the current time at which the message will be published.</param>
    /// <param name="messageHeaders">Optional headers to attach to the message.</param>
    /// <param name="cancellationToken">A token to cancel the in-flight publish (does not cancel the scheduled delivery itself).</param>
    /// <returns>Returns true if sending the message was successful.</returns>
    Task<bool> PublishAsync(TMessage message, TimeSpan publishAfter, Dictionary<string, string> messageHeaders = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a pre-serialized message asynchronously. The caller is responsible for supplying the correct
    /// <paramref name="messageType"/> and <paramref name="tenant"/>; no validation is performed against <paramref name="messageRaw"/>.
    /// </summary>
    /// <param name="messageRaw">The serialized message bytes.</param>
    /// <param name="messageType">The runtime <see cref="Type"/> of the message.</param>
    /// <param name="tenant">The tenant the message belongs to.</param>
    /// <param name="messageHeaders">Optional headers to attach to the message.</param>
    /// <param name="cancellationToken">A token to cancel the in-flight publish.</param>
    /// <returns>Returns true if sending the message was successful.</returns>
    Task<bool> PublishAsync(byte[] messageRaw, Type messageType, string tenant, Dictionary<string, string> messageHeaders = null, CancellationToken cancellationToken = default);
}
