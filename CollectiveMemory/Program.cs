using CollectiveMemory.Core.Data;
using CollectiveMemory.Core.Entities;
using CollectiveMemory.Core.Services;
using CollectiveMemory.Core.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CollectiveMemory
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // --- Database (NeonDB via Npgsql) ---
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    npgsqlOptions =>
                    {
                        npgsqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(5),
                            errorCodesToAdd: null
                        );
                    }
                )
            );

            // --- Identity ---
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 10;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/Login";
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;
                options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
                    ? CookieSecurePolicy.SameAsRequest
                    : CookieSecurePolicy.Always;
            });

            // Keys live in the database: container disks are ephemeral, and losing them would
            // sign everyone out and invalidate open forms on every restart / free-plan spin-down.
            builder.Services.AddDataProtection()
                .SetApplicationName("CollectiveMemory")
                .PersistKeysToDbContext<ApplicationDbContext>();

            //Register Services
            builder.Services.AddScoped<IShowService, ShowService>();
            builder.Services.AddScoped<IMemberService, MemberService>();

            // --- MVC ---
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            // Liveness only (no DB check) so a sleeping Neon compute doesn't get the service restarted.
            builder.Services.AddHealthChecks();

            var app = builder.Build();

            // Apply pending EF migrations so a fresh database is usable on first deploy.
            using (var scope = app.Services.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();

                var adminUsername = app.Configuration["Admin:Username"];
                var adminPassword = app.Configuration["Admin:Password"];
                if (string.IsNullOrEmpty(adminUsername) || string.IsNullOrEmpty(adminPassword))
                {
                    app.Logger.LogWarning("Admin:Username / Admin:Password not set - no admin account was seeded.");
                }
                else
                {
                    IdentitySeeder.SeedAdminAsync(
                        scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>(),
                        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>(),
                        adminUsername, adminPassword).GetAwaiter().GetResult();
                }
            }

            // --- Middleware pipeline ---
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Home/HttpStatus/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
            app.MapHealthChecks("/healthz");

            app.Run();
        }
    }
}
