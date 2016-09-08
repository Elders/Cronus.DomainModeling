namespace Elders.Cronus.DomainModeling
{
    /// <summary>
    /// Port is the mechanizm to do communication between aggregates. Usually this involves one aggregate who
    /// triggered an event and one aggregate which needs to react.
    /// If you feel the need to do more complex interactions it is advised to use ISaga. The reason for this
    /// is that IPort does not provide transperant view of a business flow because they do not have persistent state.
    /// </summary>
    public interface IPort
    {
        IPublisher<ICommand> CommandPublisher { get; set; }
    }
}
