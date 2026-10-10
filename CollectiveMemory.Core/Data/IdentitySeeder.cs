using CollectiveMemory.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace CollectiveMemory.Core.Data
{
    /// <summary>
    /// Creates the band's admin account from the credentials passed in (Admin:Username /
    /// Admin:Password: user-secrets locally, environment variables in production).
    /// There is deliberately no default login: the database is remote, so a well-known
    /// password would be public. An existing account is left alone, so changing the configured
    /// password later has no effect, unless <c>resetPassword</c> is set (Admin:ResetPassword=true):
    /// then the account's password is set to the configured one and any lockout is cleared.
    /// </summary>
    public static class IdentitySeeder
    {
        public const string AdminRole = "Admin";

        public static async Task SeedAdminAsync(
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            string username, string password, bool resetPassword = false)
        {
            if (!await roleManager.RoleExistsAsync(AdminRole))
                await roleManager.CreateAsync(new IdentityRole(AdminRole));

            var admin = await userManager.FindByNameAsync(username);
            if (admin is null)
            {
                admin = new ApplicationUser { UserName = username, DisplayName = "Band admin" };

                var result = await userManager.CreateAsync(admin, password);
                if (!result.Succeeded)
                    throw new InvalidOperationException(
                        "Failed to seed admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            else if (resetPassword)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(admin);
                var result = await userManager.ResetPasswordAsync(admin, token, password);
                if (!result.Succeeded)
                    throw new InvalidOperationException(
                        "Failed to reset the admin password: " + string.Join(", ", result.Errors.Select(e => e.Description)));

                // a reset is also the way out of a lockout after too many wrong attempts
                await userManager.SetLockoutEndDateAsync(admin, null);
                await userManager.ResetAccessFailedCountAsync(admin);
            }

            if (!await userManager.IsInRoleAsync(admin, AdminRole))
                await userManager.AddToRoleAsync(admin, AdminRole);
        }
    }
}
