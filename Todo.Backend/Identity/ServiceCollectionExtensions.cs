using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;

namespace Todo.Backend.Identity
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAndConfigureIdentityServer(this IServiceCollection services)
        {
            var scopes = new List<ApiScope>()
            {
                new ApiScope("api/v1", "Todo API")
            };

            var clients = new List<Client>
            {
                new Client
                {
                    ClientId = "todo_client",
                    ClientSecrets = { new Secret(Environment.GetEnvironmentVariable("CLIENT_SECRET") .Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    // scopes that client has access to
                    AllowedScopes = { "api/v1" }
                }
            };

            services.AddIdentityServer()
                // .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(scopes)
                .AddInMemoryClients(clients)
                .AddDeveloperSigningCredential();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Authority = Environment.GetEnvironmentVariable("JWT_BEARER_AUTHORITY");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });
        }
    }
}
