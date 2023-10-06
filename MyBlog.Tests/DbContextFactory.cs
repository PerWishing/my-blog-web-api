using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using MyBlog.Domain.Entities;
using MyBlog.Persistance.Database;
using MyBlog.Persistance.Identity;
using System.Security.Claims;

namespace MyBlog.Tests
{
    public class DbContextFactory
    {

        public static async ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();


            var user = new ApplicationUser
            {
                UserName = "User1",
                Email = "a@a.com",
            };

            user.UserProfile = new UserProfile
            {
                Id = user.Id,
                UserName = user.UserName
            };

            var result = await userManager.CreateAsync(user, "");
            if (result.Succeeded)
            {
                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "User"));

                return true;
            }

            context.Users.Add(
                new Persistance.Identity.ApplicationUser
                {
                    
                }
                );

            context.Posts.AddRange(
                
                );

            return context;
        }

        public Mock<UserManager<ApplicationUser>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
        }
    }
}
