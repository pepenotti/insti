using Insti.Data.Models;

namespace Insti.Data.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(string entityId);
        void Delete(T entity);
        T? GetById(string entityId);
        List<T> GetAll();
    }
}