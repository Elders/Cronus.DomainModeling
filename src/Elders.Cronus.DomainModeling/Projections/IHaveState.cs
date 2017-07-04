namespace Elders.Cronus.DomainModeling.Projections
{
    public interface IHaveState
    {
        object State { get; set; }
        void InitializeState(object state);
    }
}
