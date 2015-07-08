using System;

namespace Elders.Cronus.DomainModeling.Projections
{
    public class KeyValueCollectionItem
    {
        public KeyValueCollectionItem()
        {

        }

        public KeyValueCollectionItem(string collectionId, string id, string table, byte[] blob)
        {
            CollectionId = collectionId;
            Id = Guid.NewGuid();
            ItemId = id;
            Table = table;
            Blob = blob;
        }
        public virtual string CollectionId { get; set; }

        public virtual Guid Id { get; set; }//RecordId

        public virtual string ItemId { get; set; }

        public virtual string Table { get; set; }

        public virtual byte[] Blob { get; set; }
    }
}
