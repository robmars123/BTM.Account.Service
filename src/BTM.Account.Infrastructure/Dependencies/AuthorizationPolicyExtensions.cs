using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTM.Account.Shared.Common;
using Microsoft.Extensions.DependencyInjection;

namespace BTM.Account.Infrastructure.Dependencies;
public static class AuthorizationPolicyExtensions
{
  public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
  {
    services.AddAuthorization(options =>
    {
      options.AddPolicy(GlobalConstants.Roles.Admin, policy =>
          policy.RequireClaim("role", GlobalConstants.Roles.Admin));
    });

    return services;
  }
}

