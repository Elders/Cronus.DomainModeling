namespace Elders.Cronus
{
    /// <summary>
    /// Compared to IPort, which can dispatch a command, an IGateway can do the same but it also has a persistent state. A scenario could be sending
    /// commands to external BC like push notifications, emails etc. There is no need to event source this state and its perfectly
    /// fine if this state is wiped. Example: iOS push notifications badge. This state should be used only for infrastructure needs and never
    /// for business cases.
    /// Compared to projections, which tracks events and project their data and are not allowed to send any commands at all, an IGateway store and track
    /// a metadata required by external systems. Also, IGateway are restricted and not touched when events are replayed.
    /// </summary>
    public interface IGateway : IMessageHandler { }
}
