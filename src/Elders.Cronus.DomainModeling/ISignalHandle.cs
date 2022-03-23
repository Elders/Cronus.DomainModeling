using System.Threading.Tasks;

namespace Elders.Cronus;

public interface ISignalHandle<in T>
    where T : ISignal
{
    Task HandleAsync(T signal);
}
