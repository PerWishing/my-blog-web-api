using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MyBlog.Persistance.Services;
using Microsoft.Extensions.DependencyInjection;
using MyBlog.Persistance.Database;
using MyBlog.Persistance.Identity;

namespace MyBlog.Web.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DbInitializer(IServiceScopeFactory scopeFactory)
        {
            this._scopeFactory = scopeFactory;
        }

        public void Initialize()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    if (context != null)
                    {
                        context.Database.Migrate();
                    }
                }
            }
        }

        public void SeedData()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>())
                {
                    if (userManager != null)
                    {
                        if (userManager != null && !userManager.Users.Any())
                        {
                            var adminUser = new ApplicationUser
                            {
                                UserName = "admin",
                            };

                            adminUser.UserProfile = new Domain.Entities.UserProfile {
                                Id = adminUser.Id,
                                UserName = "admin" 
                            };

                            var result = userManager.CreateAsync(adminUser, "123qwe").GetAwaiter().GetResult();

                            if(result.Succeeded)
                            {
                                userManager.AddClaimAsync(adminUser, new Claim(ClaimTypes.Role, "Admin"))
                                    .GetAwaiter().GetResult();
                            }
                        }

                        //@No Identity adding admin user 
                        //if (context != null && !context.Users.Any())
                        //{
                        //    var passHasher = new PasswordHasher<ApplicationUser>();
                        //    var adminUser = new ApplicationUser
                        //    {
                        //        UserName = "admin",
                        //    };
                        //    adminUser.PasswordHash = passHasher.HashPassword(adminUser, "admin");
                        //    context.Users.Add(adminUser);
                        //}

                        //context.SaveChanges();
                    }
                }
            }
        }
    }
}
