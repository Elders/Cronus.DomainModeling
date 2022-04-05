namespace Elders.Cronus.Projections;

public interface IHaveState
{
    /// <summary>
    /// The ID of the specific projection instance.
    /// </summary>
    IBlobId Id { get; set; }

    /// <summary>
    /// The state/snapshot of the specific projection instance.
    /// </summary>
    object State { get; set; }

    /// <summary>
    /// Initializes the projection.
    /// </summary>
    /// <param name="id">The ID of the specific projection instance.</param>
    /// <param name="state">The state/snapshot of the specific projection instance.</param>
    void InitializeState(IBlobId id, object state);
}
