using BTM.Account.Domain.Claims;
using BTM.Account.Domain.Users;
using BTM.Account.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace BTM.Account.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) // This passes the options to the base constructor of DbContext
        {
        }
        //public DbSet<User> Users { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
        }
    }
}
