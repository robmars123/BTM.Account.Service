using BTM.Account.Domain.Claims;
using Duende.IdentityModel;
using System.Security.Claims;

namespace BTM.IdentityServer.Models
{
    public class UserDTO
    {
        public Guid SubjectId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public ICollection<Claim> Claims { get; set; } = new HashSet<Claim>(new ClaimComparer());
    }
}
