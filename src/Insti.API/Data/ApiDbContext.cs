using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Insti.API.Data
{
    public class ApiDbContext : IdentityDbContext<IdentityUser>
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }
    }
}