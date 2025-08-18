using Hackton.Domain.VideoResult.Entity;

namespace Hackton.Domain.Interfaces.VideoResult
{
    public interface IVideoResultRepository
    {
        Task<VideoResultEntity> GetByVideoId(Guid VideoId);
        Task Create(VideoResultEntity videoResultEntity);
    }
}
