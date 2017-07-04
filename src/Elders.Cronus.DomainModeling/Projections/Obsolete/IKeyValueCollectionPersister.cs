using System.Collections.Generic;

namespace Elders.Cronus.DomainModeling.Projections
{
    [System.Obsolete("Use event sourced projections instead.")]
    public interface IKeyValueCollectionPersister
    {
        IEnumerable<KeyValueCollectionItem> GetCollection(string collectionId, string keySpace);

        KeyValueCollectionItem GetCollectionItem(string collectionId, string itemId, string columnFamily);

        void AddToCollection(KeyValueCollectionItem collectionItem);

        void DeleteCollectionItem(KeyValueCollectionItem collectionItem);

        void Update(KeyValueCollectionItem collectionItem, byte[] data);
    }
}