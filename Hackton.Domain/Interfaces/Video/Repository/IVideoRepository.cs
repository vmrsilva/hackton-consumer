using Hackton.Domain.Interfaces.Video.Entity;

namespace Hackton.Domain.Interfaces.Video.Repository
{
    public interface IVideoRepository
    {
        Task UpdateAsync(VideoEntity videoEntity);

        Task<VideoEntity> GetByIdAsync(Guid id);
    }
}
