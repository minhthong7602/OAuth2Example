using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityConfiguration
{
    public class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "weatherApi1",
                    ClientName = "ASP.NET Core Weather Api 1",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret> {new Secret("ProCodeGuide".Sha256())},
                    AllowedScopes = new List<string> {"weatherApi.read"},
                    AllowedCorsOrigins = new List<string>
                    {
                        "https://localhost:5194"
                    }
                },
                 new Client
                {
                    ClientId = "weatherApi2",
                    ClientName = "ASP.NET Core Weather Api 2",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret> {new Secret("ProCodeGuide".Sha256())},
                    AllowedScopes = new List<string> {"weatherApi.write"},
                    AllowedCorsOrigins = new List<string>
                    {
                        "https://localhost:5194"
                    }
                },
                new Client
                {
                    ClientId = "oidcMVCApp",
                    ClientName = "Sample ASP.NET Core MVC Web App",
                    ClientSecrets = new List<Secret> {new Secret("ProCodeGuide".Sha256())},
    
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = new List<string> {"https://localhost:44346/signin-oidc"},
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role",
                        "weatherApi.read"
                    },
                     AllowedCorsOrigins = new List<string>
                    {
                        "https://localhost:5194"
                    },
                    RequirePkce = true,
                    AllowPlainTextPkce = false
                }
            };
        }
    }
}
