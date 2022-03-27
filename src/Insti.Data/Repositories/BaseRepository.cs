using Insti.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Insti.Data.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected InstiDbContext context;
        protected DbSet<T> dbSet;

        protected BaseRepository(InstiDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
        }

        public void Delete(int entityId) => Delete(GetById(entityId));

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
            context.SaveChanges();
        }

        public List<T> GetAll() => dbSet.ToList();

        public T GetById(int entityId) => dbSet.SingleOrDefault(e => e.Id == entityId)!;

        public void Update(T entity)
        {
            dbSet.Update(entity);
            context.SaveChanges();
        }
    }
}
