namespace Elders.Cronus.DomainModeling.Projections
{
    [System.Obsolete("Use event sourced projections instead.")]
    public class Query<T>
    {
        public Query(IRepository session)
        {
            Session = session;
        }

        public IRepository Session { get; set; }
    }
}
