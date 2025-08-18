using Hackton.Domain.Interfaces.Base.Repository;
using Hackton.Domain.Interfaces.Video.Repository;
using Hackton.Domain.Video.Entity;

namespace Hackton.Infrastructure.Repository.Video
{
    public class VideoRepository : IVideoRepository
    {
        private readonly IBaseRepository<VideoEntity> _repository;

        public VideoRepository(IBaseRepository<VideoEntity> repository)
        {
            _repository = repository;
        }

        public async Task<VideoEntity> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id).ConfigureAwait(false);
        }

        public async Task UpdateAsync(VideoEntity videoEntity)
        {
            await _repository.UpdateAsync(videoEntity).ConfigureAwait(false);
        }
    }
}
