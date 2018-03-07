namespace Elders.Cronus.DomainModeling
{
    /// <summary>
    /// A command is used to dispatch domain model changes. It can be accepted or rejected depending on the domain model invariants.
    /// </summary>
    public interface ICommand : IMessage { }
}
