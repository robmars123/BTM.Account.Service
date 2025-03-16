// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityModel;
using Duende.IdentityServer.Test;
using System.Security.Claims;

namespace btm.identityserver;

public static class TestUsers
{
    public static List<TestUser> Users
    {
        get
        {
            var address = new
            {
                street_address = "One Hacker Way",
                locality = "Heidelberg",
                postal_code = "69118",
                country = "Germany"
            };
                
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "admin",
                    Password = "admin",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Role, "admin"),
                        new Claim(JwtClaimTypes.Name, "Alice Smith")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "bob",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Role, "registered"),
                        new Claim(JwtClaimTypes.Name, "Bob Smith")
                    }
                }
            };
        }
    }
}