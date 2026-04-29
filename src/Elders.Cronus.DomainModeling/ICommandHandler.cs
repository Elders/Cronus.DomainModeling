using System.Threading.Tasks;

namespace Elders.Cronus;

public interface ICommandHandler<in T>
    where T : ICommand
{
    Task HandleAsync(T command);
}
