namespace Elders.Cronus
{
    /// <summary>
    /// Port is the mechanizm to do communication between aggregates. Usually this involves one aggregate who
    /// triggers an event and one aggregate which needs to react.
    /// If you feel the need to do more complex interactions it is advised to use <see cref="ISaga"/>. The reason for this
    /// is that <see cref="IPort"/> does not provide transperant view of a business flow.
    /// </summary>
    public interface IPort
    {
        IPublisher<ICommand> CommandPublisher { get; set; }
    }
}
