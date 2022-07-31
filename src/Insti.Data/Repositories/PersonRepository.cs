using Insti.Core.Services.Interfaces;
using Insti.Data.Models;
using Insti.Data.Repositories.Interfaces;
using static Insti.Core.Constants.ErrorCodes;

namespace Insti.Data.Repositories
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        private IEncryptionService encryptionService;

        public PersonRepository(InstiDbContext context, IEncryptionService encryptionService) : base(context)
        {
            this.encryptionService = encryptionService;
        }

        public Person? GetByUserId(string userId) => dbSet.SingleOrDefault(p => p.IdentityUserId == encryptionService.EncryptString(userId));

        public void DeleteByUserId(string userId)
        {
            var person = GetByUserId(userId);

            if (person == null)
                throw new Exception(BadRequest(Code.InvalidUserId));

            person.FirstName = String.Empty;
            person.LastName = String.Empty;
            person.GenderId = "0";

            Update(person);
        }
    }
}
