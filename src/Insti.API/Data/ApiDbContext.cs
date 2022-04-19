using Insti.Data.Models;
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

        public DbSet<Module> Modules { get; set; }
        public DbSet<Assistance> Assistances { get; set; }
        public DbSet<AssistanceStatus> AssistanceStatuses { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Student> Students  { get; set; }
    }
}