using System.Collections.Generic;

namespace Elders.Cronus.DomainModeling.Projections
{
    public interface IKeyValueCollectionPersister
    {
        IEnumerable<KeyValueCollectionItem> GetCollection(string collectionId, string keySpace);

        void AddToCollection(KeyValueCollectionItem collectionItem);

        void DeleteCollectionItem(KeyValueCollectionItem collectionItem);

        void Update(KeyValueCollectionItem collectionItem, byte[] data);
    }
}