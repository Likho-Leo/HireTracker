using HireTracker.Controllers;
using Microsoft.AspNetCore.Identity;

namespace HireTracker.Services
{
    public class SeedServices
    {
        public static async Task SeedData (IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedServices>>();
            var config  = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            try
            {

                //Add user
                logger.LogInformation("seeding data.");

                var email = config["SeedSetting:Email"];
                var password = config["SeedSettings:Password"];

                var users = new[]
                {
                    new { Email = config["SeedSettings:User1Email"], Password = config["SeedSettings:User1Password"]},
                    new { Email = config["SeedSettings:User2Email"], Password = config["SeedSettings:User2Password"]},
                };

                foreach(var u in users)
                {
                    if (await userManager.FindByEmailAsync(u.Email) == null)
                    {
                        var user = new IdentityUser
                        {
                            UserName = u.Email,
                            NormalizedEmail = u.Email.ToUpper(),
                            Email = u.Email,
                            NormalizedUserName = u.Email.ToUpper(),
                            EmailConfirmed = true,
                            SecurityStamp = Guid.NewGuid().ToString(),
                        };

                        await userManager.CreateAsync(user, u.Password);

                    }
                }
               
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "An error occured while seeding to the database");
            }
        }
    }
}
