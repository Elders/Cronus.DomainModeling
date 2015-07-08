namespace Elders.Cronus.DomainModeling.Projections
{
    public class Query<T>
    {
        public Query(IRepository session)
        {
            Session = session;
        }

        public IRepository Session { get; set; }
    }
}
