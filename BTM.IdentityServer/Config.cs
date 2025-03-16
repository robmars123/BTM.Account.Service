using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace BTM.IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("scope1"),
            new ApiScope("scope2"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client()
                {
                    ClientName = "Account Service Web Client",
                    //this is the client id that will be used by the client application to identify itself
                    //BTM.Account.MVC.Client
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowOfflineAccess = true,
                    ClientId = "Account.MVC.Client",
                    AllowedGrantTypes = GrantTypes.Code,
                                        RedirectUris = new List<string>()
                    {
                        "https://localhost:7282/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:7282/signout-callback-oidc"
                    } ,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "btmaccountapi.fullaccess",//clients should match these scopes
                        "btmaccountapi.read",//clients should match these scopes
                        "btmaccountapi.write",//clients should match these scopes
                    },
                    ClientSecrets =
                    {
                        new Secret("mysecret".Sha256())
                    },
            }
        };
}
