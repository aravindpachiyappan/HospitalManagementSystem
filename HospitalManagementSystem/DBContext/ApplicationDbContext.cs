using Hospital_ManagementSystem_Api.Entity;
using Microsoft.EntityFrameworkCore;

namespace Hospital_ManagementSystem_Api.DBContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> userRoles { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
