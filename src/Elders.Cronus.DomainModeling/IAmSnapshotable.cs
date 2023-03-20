namespace Elders.Cronus;

public interface IAmSnapshotable<T>
    where T : class, new()
{
    T CreateSnapshot();
    void RestoreFromSnapshot(T snapshot);
}
