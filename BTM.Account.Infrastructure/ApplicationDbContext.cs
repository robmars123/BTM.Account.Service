using BTM.Account.Domain.Claims;
using BTM.Account.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace BTM.Account.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) // This passes the options to the base constructor of DbContext
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                        .HasKey(u => u.Id);

            // You can explicitly configure the relationship between User and UserClaim
            modelBuilder.Entity<UserClaim>()
                .HasKey(uc => uc.UserClaimID);  // Define primary key for UserClaim

            modelBuilder.Entity<UserClaim>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.Claims)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();


            // Seed data for Users
            //modelBuilder.Entity<User>().HasData(
            //    new User
            //    {
            //        Email = "test@test.com",
            //        FirstName = "test",
            //        LastName = "test",
            //        Password = "Y3EoMt0q9GXRcGZ/JYY9YTdRvZAxDxVPzg3T4cOulklnpZbE"
            //    }
            //);

            // Seed data for UserClaims
            modelBuilder.Entity<UserClaim>().HasData(
                new UserClaim
                {
                    UserClaimID = 1,
                    UserId = new Guid("14802186-7D6A-41E1-A209-F61FBA883837"),  // Replace with the actual User GUID generated
                    ClaimType = "Role",
                    ClaimValue = "Admin"
                }
            );
        }
    }
}
