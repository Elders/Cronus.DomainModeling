namespace Elders.Cronus.DomainModeling.Projections
{
    [System.Obsolete("Use event sourced projections instead.")]
    public interface IKeyValuePersister
    {
        void Save(KeyValueData data);

        KeyValueData Get(string id, string keySpace);

        void Delete(string id, string keySpace);

        void Update(KeyValueData item, byte[] data);
    }
}
