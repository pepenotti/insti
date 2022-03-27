using Insti.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Insti.Data
{
    public class InstiDbContext : DbContext
    {
        public InstiDbContext()
        {
        }

        public InstiDbContext(DbContextOptions<InstiDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}