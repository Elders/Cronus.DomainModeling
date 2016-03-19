using System;

namespace Elders.Cronus.DomainModeling
{
    /// <summary>
    /// We have ports that are throwing commands. Which is ok if we send commands to the same BC, but there is a need to have ports with 
    /// state(aka IGateway) for external BC like push notifications,emails etc. There is no need to event source this state and its perfectly 
    /// fine if this state is wiped. Example: iOS push notifications badge.This state should be used only for infrastructure need and never 
    /// for business cases.
    /// NM: Usually projections just track events and project their data. They are not allowed to send any commands at all. However generic 
    /// domains such as emailing, notification services etc. require explicit invocation of their APIs with some meta data attached. Basically 
    /// we need to store and track this metadata inside IGateway state. Also, IGateway are restricted and not touched when events are replayed.
    /// </summary>
    public interface IGateway { }

    /// <summary>
    /// When we have a workflow which involves several aggregates it is recommended to have the whole process described in a single place 
    /// such as Saga. Right now we do not have explicit support for Sagas and we use IPort for these communication between the aggregates. 
    /// This approach works well only if you have to issue a single ICommand to another aggregate but it becomes a hell to maintain more 
    /// complex scenarios.
    /// </summary>
    public interface ISaga
    {
        IPublisher<ICommand> CommandPublisher { get; set; }
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
