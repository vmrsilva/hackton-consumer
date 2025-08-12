using Hackton.Domain.Interfaces.Abstractions.UseCaseAbstraction;
using Hackton.Shared.Dto.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackton.Domain.Interfaces.Video.UseCases
{
    public interface IProcessVideoUseCaseHandle : IUseCaseCommandHandler<VideoMessageDto>
    {
    }
}
