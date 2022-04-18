using System.Security.Claims;
using BenchmarkDotNet.Attributes;
using IdentityModel;
using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityServer;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var demoUser = userMgr.FindByNameAsync("demo").Result;
            if (demoUser == null)
            {
                demoUser = new ApplicationUser
                {
                    UserName = "demo",
                    Email = "demo@email.com",
                    EmailConfirmed = true,
                };

                var result = userMgr.CreateAsync(demoUser, "Demo123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(
                    demoUser,
                    new Claim[]{
                        new Claim(JwtClaimTypes.Name, "Demo"),
                        new Claim(JwtClaimTypes.GivenName, "Demo"),
                        new Claim(JwtClaimTypes.FamilyName, "Demo"),
                        new Claim(JwtClaimTypes.WebSite, "https://Demo.com"),
                    }).Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Log.Debug("Demo created");
            }
            else
            {
                Log.Debug("Demo already exists");
            }
        }
    }
}
