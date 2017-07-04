using System;
using System.Collections.Generic;

namespace Elders.Cronus.DomainModeling.Projections
{
    [System.Obsolete("Use event sourced projections instead.")]
    public interface IRepository
    {
        Func<byte[], object> Desirealizer { get; }
        Func<object, byte[]> Serializer { get; }

        void CommitChanges();

        void Delete<T, V>(T obj) where T : IDataTransferObject<V>;

        void DeleteCollectionItem<T, V, C>(T obj) where T : ICollectionDataTransferObjectItem<V, C>;

        T Get<T, V>(V ids) where T : IDataTransferObject<V>;

        IEnumerable<T> GetAsCollectionItems<T, C>(C collectionIds) where T : ICollectionDataTransferObject<C>;

        Query<T> Query<T>();

        void Save<T, V>(T obj) where T : IDataTransferObject<V>;

        void Save<T, V, C>(params T[] items) where T : ICollectionDataTransferObjectItem<V, C>;

        void ClearTrack();
    }
}
