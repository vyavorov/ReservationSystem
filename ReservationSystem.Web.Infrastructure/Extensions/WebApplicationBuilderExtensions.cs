using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ReservationSystem.Data.Models;
using static ReservationSystem.Common.GeneralApplicationConstants;

namespace ReservationSystem.Web.Infrastructure.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static IApplicationBuilder SeedAdministrator(this IApplicationBuilder app, string email)
    {
        using IServiceScope scopedServices = app.ApplicationServices.CreateScope();

        IServiceProvider serviceProvider = scopedServices.ServiceProvider;

        UserManager<ApplicationUser> userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        RoleManager<IdentityRole<Guid>> roleManager =
            serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        Task.Run(async () =>
        {
            // Ensure role exists
            if (!await roleManager.RoleExistsAsync(AdminRoleName))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(AdminRoleName));
            }

            // Ensure user exists
            ApplicationUser adminUser = await userManager.FindByEmailAsync(email);
            if (adminUser == null)
            {
                // Create the user
                adminUser = new ApplicationUser { UserName = email, Email = email };
                await userManager.CreateAsync(adminUser, "123456"); // Consider a stronger default password logic
            }

            // Assign role to user
            if (!await userManager.IsInRoleAsync(adminUser, AdminRoleName))
            {
                await userManager.AddToRoleAsync(adminUser, AdminRoleName);
            }

        })
        .GetAwaiter()
        .GetResult();

        return app;
    }

}
