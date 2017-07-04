namespace Elders.Cronus.DomainModeling.Projections
{
    [System.Obsolete("Use event sourced projections instead.")]
    public interface IPersiter : IKeyValuePersister, IKeyValueCollectionPersister
    {
    }
}