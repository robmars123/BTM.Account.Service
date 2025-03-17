using BTM.Account.Domain.Users;
using System;
using System.Linq;

namespace BTM.Account.Application.Abstractions
{
    public interface IUserFactory
    {
        User CreateUser(string email, string firstName, string lastName, string password);
    }
}
