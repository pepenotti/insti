using Bogus;
using Insti.API.Test.Mocks;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Person = Insti.Data.Models.Person;

namespace Insti.API.Test.Helpers
{
    public class RepositoryMocker
    {
        public List<IdentityUser> Users { get; set; }
        public PersonRepositoryMock PersonRepository { get; set; }

        public void Init(
            List<IdentityUser>? users = null,
            List<Person>? people = null)
        {
            Users = users ?? MockUsers(11);
            PersonRepository = new PersonRepositoryMock(people ?? MockPeople(10));
        }

        public List<IdentityUser> MockUsers(int amount)
        {
            var faker = new Faker<IdentityUser>()
                .RuleFor(u => u.Id, f => f.UniqueIndex.ToString())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.UserName, f => f.Internet.UserName())
                .RuleFor(u => u.EmailConfirmed, f => true);

            return faker.Generate(amount);
        }

        public List<Person> MockPeople(int amount)
        {
            var faker = new Faker<Person>()
                .RuleFor(p => p.Id, f => f.UniqueIndex.ToString())
                .RuleFor(p => p.IdentityUser, f => Users[f.IndexVariable])
                .RuleFor(p => p.IdentityUserId, (f,u) => u.IdentityUser.Id)
                .RuleFor(p => p.FirstName, f => f.Person.FirstName)
                .RuleFor(p => p.LastName, f => f.Person.LastName)
                .RuleFor(p => p.GenderId, f => f.Random.Int(0, 2).ToString());

            return faker.Generate(amount);
        }
    }
}