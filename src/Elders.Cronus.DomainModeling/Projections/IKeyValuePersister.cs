namespace Elders.Cronus.DomainModeling.Projections
{
    public interface IKeyValuePersister
    {
        void Save(KeyValueData data);

        KeyValueData Get(string id, string keySpace);

        void Delete(string id, string keySpace);

        void Update(KeyValueData item, byte[] data);
    }
}
