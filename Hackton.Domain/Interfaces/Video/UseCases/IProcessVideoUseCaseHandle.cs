using Hackton.Domain.Interfaces.Abstractions.UseCaseAbstraction;
using Hackton.Shared.Dto.Video;

namespace Hackton.Domain.Interfaces.Video.UseCases
{
    public interface IProcessVideoUseCaseHandle : IUseCaseCommandHandler<VideoMessageDto>
    {
    }
}
