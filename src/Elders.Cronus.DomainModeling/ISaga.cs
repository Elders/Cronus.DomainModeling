using System;

namespace Elders.Cronus
{
    /// <summary>
    /// When we have a workflow which involves several aggregates it is recommended to have the whole process described 
    /// in a single place such as Saga/ProcessManager.
    /// </summary>
    public interface ISaga
    {
        IPublisher<ICommand> CommandPublisher { get; set; }

        IPublisher<IScheduledMessage> TimeoutRequestPublisher { get; set; }
    }

    /// <summary>
    /// Message which will be published in the future.
    /// </summary>
    public interface IScheduledMessage : IMessage
    {
        /// <summary>
        /// The date as 'date.ToFileTimeUtc()' when this message will be published.
        /// </summary>
        long PublishAt { get; }
    }

    public interface ISagaTimeoutHandler<in T> where T : IScheduledMessage
    {
        void Handle(T sagaTimeout);
    }

    public class Saga : ISaga
    {
        public IPublisher<ICommand> CommandPublisher { get; set; }

        public IPublisher<IScheduledMessage> TimeoutRequestPublisher { get; set; }

        public void RequestTimeout<T>(T timeoutMessage) where T : IScheduledMessage
        {
            TimeoutRequestPublisher.Publish(timeoutMessage, DateTime.FromFileTimeUtc(timeoutMessage.PublishAt));
        }

    }
}
