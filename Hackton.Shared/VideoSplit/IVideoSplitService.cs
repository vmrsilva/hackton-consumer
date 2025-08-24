using Hackton.Shared.Dto.VideoSplit;

namespace Hackton.Shared.VideoSplit
{
    public interface IVideoSplitService
    {
        Task<VideoSplitResultDto> ProcessVideoSPlit(Stream fileStream, string extensionFile, CancellationToken cancellation = default);
    }
}
