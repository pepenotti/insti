using Insti.Data.Models;

namespace Insti.Data.Repositories.Interfaces
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        Person? GetByUserId(string userId);
        void DeleteByUserId(string id);
    }
}
