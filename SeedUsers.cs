using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityServer
{
    public class SeedUsers
    {
        private ApplicationDbContext _context;
        private ILogger<SeedUsers> _logger;
        private UserManager<ApplicationUser> _userManager;

        public SeedUsers(ApplicationDbContext context, ILogger<SeedUsers> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task Seed()
        {
            _logger.LogInformation("Seeding Class......");

            // check if users exists
            var usersExists = _context.Users.Any();

            if (usersExists)
            {
                _logger.LogInformation("Users already exist. Not need to seed users.");
                return;
            }

            if (!usersExists)
            {
                _logger.LogInformation("Checking if to seed users.......");

                // seed users
                var alice = new ApplicationUser { UserName = "alice", Email = "alice@test.com" };

                var bob = new ApplicationUser { UserName = "bob", Email = "bob@test.com" };

                var aliceResult = new IdentityResult();
                try
                {
                    aliceResult = await _userManager.CreateAsync(alice, "Pass123$");

                }
                catch (System.Exception ex)
                {

                }


                // https://stackoverflow.com/questions/32459670/resolving-instances-with-asp-net-core-di

                if (aliceResult.Succeeded)
                {
                    _logger.LogInformation($"Alice created successfully");
                    // add alice claims
                    await AddUserClaims(alice, "Alice", GetAliceClaims());
                }
                else
                {
                    var errors = GetErrors(aliceResult.Errors);
                    _logger.LogInformation($"Alice could not be added to database, Errors: {errors}");
                }



                var bobResult = await _userManager.CreateAsync(bob, "Pass123$");

                if (bobResult.Succeeded)
                {
                    _logger.LogInformation($"Bob created successfully");
                    // add alice claims
                    await AddUserClaims(alice, "Bob", GetBobClaims());
                }
                else
                {
                    var errors = GetErrors(bobResult.Errors);
                    _logger.LogInformation($"Bob could not be added to database, Errors: {errors}");
                }
            }

        }


        private async Task AddUserClaims(ApplicationUser applicationUser, string user, IEnumerable<Claim> claims)
        {
            var response = await _userManager.AddClaimsAsync(applicationUser, claims);
            if (response.Succeeded)
            {
                _logger.LogInformation($"{user} claims added to database");
            }
            else
            {
                var errors = GetErrors(response.Errors);
                _logger.LogInformation($"Could not add claims for {user} to database, Errors: {errors}");
            }
        }

        private string GetErrors(IEnumerable<IdentityError> identityErrors)
        {
            var response = identityErrors.Select(x => x.Description)
                .ToList();

            var joinedErrors = string.Join(',', response);

            return joinedErrors;
        }

        private static IEnumerable<Claim> GetAliceClaims()
        {
            return new List<Claim>
            {
                        new Claim(JwtClaimTypes.Name, "Alice Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Alice"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
            };
        }

        private static IEnumerable<Claim> GetBobClaims()
        {
            return new List<Claim>
            {
                        new Claim(JwtClaimTypes.Name, "Alice Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Alice"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
            };
        }
    }
}
