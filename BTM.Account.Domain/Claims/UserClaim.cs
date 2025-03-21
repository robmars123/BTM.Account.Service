using BTM.Account.Domain.Users;

namespace BTM.Account.Domain.Claims
{
    public class UserClaim
    {
        public int UserClaimID { get; set; }  // Primary Key
        public Guid UserId { get; set; }  // Foreign Key to User
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        // Navigation property
        public User User { get; set; }
    }
}
