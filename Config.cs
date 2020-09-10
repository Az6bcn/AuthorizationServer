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
        // Identity Resource that I want to protect
        // when these identity scopes are requested : they map to claims in the id_token
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        // API Resources that I want to protect and the scopes needed (that can be requested) to access them.
        /* when these apiResources are requested : they map to array of scope claims in the access_token,
         * when the scopes that belongs to any of these apiResources  are requested : they map to claims in the access_token.
         */
        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource> {
                new ApiResource{
                    Name = "restaurantAPI",
                    DisplayName = "restaurant API",
                    Description = "restaurant API resource",
                    // Api Resource scope: defines the type of access you can request from the API Resource. i.e you will need to have the scope(s) to be able to access the API and we could limit what a user can do based on the type of access they have (scope),the API itsself will challenge and check.
                    Scopes = new List<Scope>
                    {
                        // scope: represents what you are allowed to do.
                        new Scope
                        {
                            Name = "restaurantAPI.fullaccess",
                            DisplayName = "Rea, Write and Delete your data",
                            Description = "Read, Write and Deelete access to restaurantAPI",
                            /* List of associated user claim types that should be included in the access token.
                             *  return these claims in the daccess_token , in addition to the sub
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

        // Register the clients that can access the API resources I am protecting, i.e software that can request tokens(id_token: for authenticating user or/and token: access token for accessing a resources (API)) from IdentityServer.
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "restaurantReactClient",
                    ClientName = "Restaurantes Carta Online",
                    ClientUri = GetClientConfig["ClientUri"],
                    Description = "Restaurantes Carta Online",
                    AllowedGrantTypes = GrantTypes.Code,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowAccessTokensViaBrowser = false,
                    RequirePkce = true,
                    AccessTokenLifetime = TimeSpan.FromMinutes(15).Seconds,
                    AllowedCorsOrigins = new List<string>{GetClientConfig["AllowedCorsOrigins"] },
                    RedirectUris = new List<string> {GetClientConfig["RedirectUri"] },
                    PostLogoutRedirectUris = new List<string> {GetClientConfig["PostLogOutUri"] },
                    // my resources scopes that can be requested by this client (Identity and API scopes), can request scopes for both if the flow permits it.
                    AllowedScopes = new List<string>
                    {
                        // Identity Resources scopes
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        // APIs Resources scopes that I want them to request and when requested this will automatically add the resource they belong to to the audience property of the token.
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