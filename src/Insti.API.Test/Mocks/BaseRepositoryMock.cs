using Insti.Data.Models;
using Insti.Data.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Insti.API.Test.Mocks
{
    public class BaseRepositoryMock<T> : IBaseRepository<T> where T : BaseEntity
    {
        public Dictionary<string,T> dataBase = new();

        public void Add(T entity) => dataBase.Add(entity.Id, entity);

        public void Delete(string entityId) => dataBase.Remove(entityId);

        public void Delete(T entity) => dataBase.Remove(entity.Id);

        public List<T> GetAll() => dataBase.Values.ToList();

        public T? GetById(string entityId) => dataBase.GetValueOrDefault(entityId);

        public void Update(T entity) => dataBase[entity.Id] = entity;
    }
}
