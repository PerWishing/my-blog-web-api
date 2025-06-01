using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBlog.Domain.Entities;
using MyBlog.Domain.Entities.Summarizations;
using MyBlog.Persistance.Identity;

namespace MyBlog.Persistance.Database
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {

        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<LikedComments> LikedComments { get; set; }

        public DbSet<SavedPosts> SavedPosts { get; set; }

        public DbSet<Readers> Readers { get; set; }

        public DbSet<UserAvatar> Avatars { get; set; }
        
        
        public DbSet<Summarization> Summarizations { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserProfile>()
            .HasOne(a => a.Avatar).WithOne(up => up.UserProfile)
            .HasForeignKey<UserAvatar>(up => up.UserProfileId);

            builder.Entity<SavedPosts>()
                .HasKey(x => new { x.UserProfileId, x.PostsId});
            builder.Entity<Readers>()
                .HasKey(x => new { x.ReaderId, x.UserId });
            builder.Entity<LikedComments>()
                .HasKey(x => new { x.UserProfileId, x.CommentId });
        }

    }
}
