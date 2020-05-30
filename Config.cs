// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        // when these identity scopes are requested : they map to claims in the id_token
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        /* when these apiResources are requested : they map to array of scope claims in the access_token,
         * when the scopes that belongs to any of these apiResources  are requested : they map to array claims in the access_token.
         */
        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource> {
                new ApiResource{
                    Name = "restaurantAPI",
                    DisplayName = "restaurant API",
                    Description = "restaurant API resource",
                    Scopes = new List<Scope>
                    {
                        new Scope
                        {
                            Name = "restaurantAPI.fullaccess",
                            DisplayName = "Rea, Write and Delete your data",
                            Description = "Read, Write and Deelete access to restaurantAPI",
                            /* List of associated user claim types that should be included in the access token.
                             *  return these claims in the access_token , in addition to the sub
                             */
                            UserClaims = new List<string>{
                                JwtClaimTypes.Profile,
                                JwtClaimTypes.Name,
                                JwtClaimTypes.GivenName,
                                "canAdd",
                                "canEdit",
                                "canRead",
                                "canDelete",
                                "role"
                            }
                        },
                        new Scope
                        {
                            Name = "restaurantAPI.readonly",
                            DisplayName = "Read your data",
                            Description = "Read only access to restaurantAPI",
                            /* List of associated user claim types that should be included in the access token.
                             *  return these claims in the access_token , in addition to the sub
                             */
                            UserClaims = new List<string>{
                                JwtClaimTypes.Profile,
                                JwtClaimTypes.Name,
                                JwtClaimTypes.GivenName,
                                "canRead",
                                "role"
                            }
                        },
                        new Scope
                        {
                            Name = "restaurantAPI.writeonly",
                            DisplayName = "Write your data",
                            Description = "Write only access to restaurantAPI",
                            /* List of associated user claim types that should be included in the access token.
                             *  return these claims in the access_token , in addition to the sub
                             */
                            UserClaims = new List<string>{
                                JwtClaimTypes.Profile,
                                JwtClaimTypes.Name,
                                JwtClaimTypes.GivenName,
                                "canAdd",
                                "canEdit",
                                "canRead",
                                "canDelete",
                                "role"
                            }
                        },
                        new Scope
                        {
                            Name = "restaurantAPI.deleteonly",
                            DisplayName = "Delete your data",
                            Description = "Delete only access to restaurantAPI",
                            /* List of associated user claim types that should be included in the access token.
                             *  return these claims in the access_token , in addition to the sub
                             */
                            UserClaims = new List<string>{
                                JwtClaimTypes.Profile,
                                JwtClaimTypes.Name,
                                JwtClaimTypes.GivenName,
                                "canDelete",
                                "role"
                            }
                        }
                    }
                }
            };


        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "restaurantReactClient",
                    ClientName = "Restaurantes Carta Online",
                    ClientUri = GetClientConfig["ClientUri"],
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    AccessTokenLifetime = TimeSpan.FromMinutes(15).Seconds,
                    RedirectUris = new List<string> {GetClientConfig["RedirectUri"] },
                    PostLogoutRedirectUris = new List<string> {GetClientConfig["PostLogOutUri"] },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "restaurantAPI.fullaccess",
                        "restaurantAPI.readonly",
                        "restaurantAPI.writeonly",
                        "restaurantAPI.deleteonly"
                    }
                },
                new Client
                {
                    ClientId = "postmanClient",
                    ClientName = "postman",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret> {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "restaurantAPI.fullaccess",
                        "restaurantAPI.readonly",
                        "restaurantAPI.writeonly",
                        "restaurantAPI.deleteonly"
                    }
                },
                new Client
                {
                    ClientId = "postmanCodeClient",
                    ClientName = "postmancodeclient",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = new List<Secret> {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "restaurantAPI.fullaccess",
                        "restaurantAPI.readonly",
                        "restaurantAPI.writeonly",
                        "restaurantAPI.deleteonly"
                    }
                }

            };

        public static IConfiguration GetClientConfig => Startup.StaticIConfiguration.GetSection("RestaurantClient");
    }
}