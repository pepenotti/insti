using Insti.Data.Models;

namespace Insti.Data.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(int entityId);
        T GetById(int entityId);
        List<T> GetAll();
    }
}