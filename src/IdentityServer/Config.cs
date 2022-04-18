using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IdentityServer;

public static class Config
{
    private const string WebClientId = "Web";
    private const string ApiClientId = "Client";
    private const string ApiScopeDemo = "ApiDemo";
    private const string WebAppClientId = "WebApp";
    private const string ClientSecret = "Secrect";

    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope(ApiScopeDemo, "Demo API")
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            // machine to machine client
            new Client
            {
                ClientId = ApiClientId,
                ClientSecrets = { new Secret(ClientSecret.Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                // scopes that client has access to
                AllowedScopes = { ApiScopeDemo }
            },
                
            // interactive ASP.NET Core Web App
            new Client
            {
                ClientId = WebClientId,
                ClientSecrets = { new Secret(ClientSecret.Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,

                // where to redirect to after login
                RedirectUris =
                {
                    "https://localhost:5002/signin-oidc"
                },

                // where to redirect to after logout
                PostLogoutRedirectUris =
                {
                    "https://localhost:5002/signout-callback-oidc"
                },
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    ApiScopeDemo
                }
            },
             // interactive ASP.NET Core Web App
            new Client
            {
                ClientId = WebAppClientId,
                ClientSecrets = { new Secret(ClientSecret.Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,

                // where to redirect to after login
                RedirectUris =
                {
                    "http://localhost:4200/index.html"
                },

                // where to redirect to after logout
                PostLogoutRedirectUris =
                {
                    "http://localhost:4200"
                },
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    ApiScopeDemo
                },
                RequireClientSecret = false
            }
        };
}

