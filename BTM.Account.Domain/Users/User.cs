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
        public string Password { get; set; }

        public ICollection<UserClaim> Claims { get; set; }

        public User(Guid id, string email, string firstName, string lastName, string password)
            :base(id)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
        }

        public static User Create(string email, string firstName, string lastName, string password)
        {
            var id = Guid.NewGuid();

            return new User(id, email, firstName, lastName, password);
        }

  
    }
}
