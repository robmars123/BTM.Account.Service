using BTM.Account.Domain.Abstractions;
using BTM.Account.Domain.Claims;
using System.Data;
using System.Security.Claims;

namespace BTM.Account.Domain.Users
{
    public class User: Entity
    {
        public User() { }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public ICollection<UserClaim> Claims { get; set; }

        public User(Guid id, string email, string username, string password)
            :base(id)
        {
            Email = email;
            Username = username;
            Password = password;
        }

        public static User Create(Guid id, string email, string username, string password)
        {
            var newId = Guid.NewGuid();

            return new User(newId, email, username, password);
        }

  
    }
}
