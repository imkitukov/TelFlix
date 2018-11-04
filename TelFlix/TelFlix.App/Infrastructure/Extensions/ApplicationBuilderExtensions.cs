using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TelFlix.Data.Context;
using TelFlix.Data.Models;

namespace TelFlix.App.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<TFContext>().Database.Migrate();

                // Seeding the default administrator
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                var adminName = "Administrator";
                var regularName = "RegularUser";

                // This allows us to call asynchronous code in a synchronous context and await it
                Task.Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(regularName) == false)
                    {
                        await roleManager.CreateAsync(new IdentityRole
                        {
                            Name = regularName
                        });
                    }

                    var roleExists = await roleManager.RoleExistsAsync(adminName);

                    if (!roleExists)
                    {
                        await roleManager.CreateAsync(new IdentityRole
                        {
                            Name = adminName
                        });
                    }

                    var adminEmail = "admin@admin.admin";
                    var adminUser = await userManager.FindByNameAsync(adminEmail);

                    if (adminUser == null)
                    {
                        adminUser = new User
                        {
                            Email = "admin@admin.admin",
                            UserName = "admin@admin.admin",
                        };

                        await userManager.CreateAsync(adminUser, "admin");

                        await userManager.AddToRoleAsync(adminUser, adminName);
                    }
                })
                .Wait();
            }

            return app;
        }
    }
}
