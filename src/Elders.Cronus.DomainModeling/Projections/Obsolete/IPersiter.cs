namespace Elders.Cronus.Projections
{
    [System.Obsolete("Use event sourced projections instead.")]
    public interface IPersiter : IKeyValuePersister, IKeyValueCollectionPersister
    {
    }
}