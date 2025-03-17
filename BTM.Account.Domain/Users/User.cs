using BTM.Account.Domain.Abstractions;

namespace BTM.Account.Domain.Users
{
    public class User: Entity
    {
        private User() { }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

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
