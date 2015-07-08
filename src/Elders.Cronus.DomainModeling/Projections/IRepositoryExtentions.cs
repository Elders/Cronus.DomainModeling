using System.Collections.Generic;
using System.Linq;

namespace Elders.Cronus.DomainModeling.Projections
{
    public static class IRepositoryExtentions
    {
        public static T Get<T, V>(this Query<T> self, V id) where T : IDataTransferObject<V>
        {
            return self.Session.Get<T, V>(id);
        }

        public static IEnumerable<T> GetCollection<T, V>(this Query<T> self, V id) where T : ICollectionDataTransferObject<V>
        {
            return self.Session.GetAsCollectionItems<T, V>(id).ToList();
        }

        public static T GetCollectionItem<T, V, C>(this Query<T> self, V id, C collectionId) where T : ICollectionDataTransferObjectItem<V, C>
        {
            return (T)self.Session.GetAsCollectionItems<T, C>(collectionId).ToList().Where(x => x.Id.Equals(id)).SingleOrDefault();
        }

        public static void Save<T, V>(this Query<T> self, IDataTransferObject<V> obj) where T : IDataTransferObject<V>
        {
            self.Session.Save<T, V>((T)obj);
        }

        public static void Save<T, C, V>(this Query<T> self, ICollectionDataTransferObjectItem<C, V> obj) where T : ICollectionDataTransferObjectItem<C, V>
        {
            self.Session.Save<T, C, V>((T)obj);
        }

        public static void Delete<T, C, V>(this Query<T> self, ICollectionDataTransferObjectItem<C, V> obj) where T : ICollectionDataTransferObjectItem<C, V>
        {
            self.Session.DeleteCollectionItem<T, C, V>((T)obj);
        }
    }
}
