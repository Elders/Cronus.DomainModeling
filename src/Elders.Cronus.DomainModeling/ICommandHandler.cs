using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elders.Cronus.DomainModeling
{
    public interface ICommandHandler<in T>
        where T : ICommand
    {
        void Handle(T command);
    }
}
