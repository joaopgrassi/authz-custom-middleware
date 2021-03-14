// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[] {new IdentityResources.OpenId(), new IdentityResources.Profile(), new IdentityResources.Email()};

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api", "API", new [] {"profile", "name", "email", "role", "DateOfBirth"}),
            };

        public static IEnumerable<ApiResource> GetApiResource()
        {
            return new List<ApiResource>
            {
                new ApiResource("api", "API")
                {
                    Scopes = { "api"}
                },
            };
        }

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // SwaggerUI client
                new Client
                {
                    ClientId = "swagger-ui",
                    ClientName = "Swagger UI",
                    ClientSecrets = {new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RedirectUris = {"https://localhost:5001/swagger/oauth2-redirect.html"},
                    AllowedCorsOrigins = {"https://localhost:5001"},
                    AllowedScopes = {"openid", "profile", "api", "email"},
                },
            };
    }
}
