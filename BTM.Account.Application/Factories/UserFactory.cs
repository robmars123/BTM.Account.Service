using BTM.Account.Application.Abstractions;
using BTM.Account.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTM.Account.Application.Factories
{
    public class UserFactory : IUserFactory
    {
        private readonly IPasswordService _passwordService;

        public UserFactory(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        public User CreateUser(string email, string firstName, string lastName, string password)
        {
            // Hash the password before creating the user
            var hashedPassword = _passwordService.HashPassword(password);

            // Return the user with the hashed password
            return User.Create(email, firstName, lastName, hashedPassword);
        }
    }
}
