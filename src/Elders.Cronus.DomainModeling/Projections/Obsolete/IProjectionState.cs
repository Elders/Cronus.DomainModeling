namespace Elders.Cronus.DomainModeling.Projections
{
    [System.Obsolete("Use event sourced projections instead.")]
    public interface IProjectionState
    {
        object Id { get; }
    }
}