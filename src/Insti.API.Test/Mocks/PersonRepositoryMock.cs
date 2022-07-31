using Insti.Data.Models;
using Insti.Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Insti.API.Test.Mocks
{
    public class PersonRepositoryMock : BaseRepositoryMock<Person>, IPersonRepository
    {
        public PersonRepositoryMock() { }

        public PersonRepositoryMock(Dictionary<string, Person> db) { dataBase = db; }

        public PersonRepositoryMock(List<Person> people) { dataBase = people.ToDictionary(p => p.Id); }

        public Person? GetByUserId(string userId)
            => dataBase.Values
                .SingleOrDefault(d => d.IdentityUserId == userId);

        public void DeleteByUserId(string id)
        {
            var user = GetByUserId(id)!;
            Delete(user);
        }
    }
}
