using BTM.Account.Application.Abstractions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTM.Account.Application.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<object> _passwordHasher;

        public PasswordService()
        {
            _passwordHasher = new PasswordHasher<object>();
        }

        // Hashing the password
        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(new object(), password);
        }

        // Verifying the password
        public bool VerifyPassword(string hashedPassword, string enteredPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(new object(), hashedPassword, enteredPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
