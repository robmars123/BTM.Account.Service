using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTM.Account.Application.Results;

namespace BTM.Account.Application.Abstractions.UserIdentityManager
{
  public interface IUserIdentityManager
  {
    Task<Result> CreateUserAsync(string email, string username, string password);
  }
}
