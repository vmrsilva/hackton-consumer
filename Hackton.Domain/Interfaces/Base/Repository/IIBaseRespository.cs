using Hackton.Domain.Base.Entity;

namespace Hackton.Domain.Interfaces.Base.Repository
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(Guid id);
        Task UpdateAsync(T entity);

    }
}
