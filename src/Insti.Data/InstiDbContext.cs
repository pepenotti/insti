
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Insti.Data
{
    public class InstiDbContext : IdentityDbContext<IdentityUser>
    {
        public InstiDbContext()
        {
        }

        public InstiDbContext(DbContextOptions<InstiDbContext> options)
            : base(options)
        {
        }
    }
}