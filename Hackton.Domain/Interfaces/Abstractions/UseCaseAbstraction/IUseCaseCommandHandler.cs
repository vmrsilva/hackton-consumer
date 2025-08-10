using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackton.Domain.Interfaces.Abstractions.UseCaseAbstraction
{
    public interface IUseCaseCommandHandler<TCommand>
    {
        Task Handle(TCommand command, CancellationToken cancellation = default);
    }
}
