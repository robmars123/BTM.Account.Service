using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace BTM.IdentityServer.BTM.Duende.ASPIdentity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiResource> ApiResources =>
 new ApiResource[]
     {
             new ApiResource("AccountAPI",
                 "BTM Account API",
                 new [] { "role" })
             {
                 Scopes = { "AccountApi.fullaccess",
                     "AccountAPI.read",
                     "AccountAPI.write"},
                ApiSecrets = { new Secret("mysecret".Sha256()) }
             }
     };

    public static IEnumerable<ApiScope> ApiScopes =>
    new ApiScope[]
        {
                //clients should match these scopes
                new ApiScope("AccountAPI.fullaccess"),
                new ApiScope("AccountAPI.read"),
                new ApiScope("AccountAPI.write")};

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
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AccessTokenLifetime = 120,
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
                        "AccountAPI.fullaccess",//clients should match these scopes
                        "AccountAPI.read",//clients should match these scopes
                        "AccountAPI.write",//clients should match these scopes
                        "offline_access"
                    },
                    ClientSecrets =
                    {
                        new Secret("mysecret".Sha256())
                    },
            }
        };
}
