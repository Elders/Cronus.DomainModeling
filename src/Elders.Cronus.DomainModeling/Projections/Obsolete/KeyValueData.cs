using System;

namespace Elders.Cronus.DomainModeling.Projections
{
    [System.Obsolete("Use event sourced projections instead.")]
    public class KeyValueData
    {
        public KeyValueData()
        {

        }

        public KeyValueData(string id, string table, byte[] blob)
        {
            Id = Guid.NewGuid();
            ItemId = id;
            Table = table;
            Blob = blob;
        }
        public virtual Guid Id { get; set; }//RecordId

        public virtual string ItemId { get; set; }

        public virtual string Table { get; set; }

        public virtual byte[] Blob { get; set; }
    }
}
