using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.Persistance.Database;
using MyBlog.Persistance.Identity;
using MyBlog.Domain.Entities;

namespace MyBlog.Persistance.Repositories.CommentRepository
{
    public class CommentManager : ICommentManager
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentManager(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<bool> CreateAsync(CreateCommentRequest request)
        {
            var author = await context.UserProfiles.FirstOrDefaultAsync(u => u.UserName == request.AuthorsName);
            if (author == null)
            {
                return false;
            }

            var post = await context.Posts.FirstOrDefaultAsync(x => x.Id == request.PostId);

            var comment = new Comment
            {
                Text = request.Text,
                Date = DateTime.Now,
                Author = author,
                Post = post!
            };

            context.Add(comment);

            var result = await context.SaveChangesAsync();
            if (result > 0)
                return true;
            else
                return false;
        }

        public async Task<bool> DeleteAsync(int id, string username)
        {
            var commentForDelete = await context.Comments
                .Include(x=> x.Author)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (commentForDelete == null)
            {
                return false;
            }

            if(commentForDelete.Author.UserName != username)
            {
                return false;
            }

            context.Remove(commentForDelete);

            var result = await context.SaveChangesAsync();
            if (result > 0)
                return true;
            else
                return false;
        }

        public async Task<IList<GetCommentResponse>> GetAllByPostAsync(int postId)
        {
            var queryResult = await context.Comments
                .Include(x => x.Author)
                .Where(x => x.Author.IsBlocked == false)
                .Include(x => x.Post)
                .Where(x => x.Post.Id == postId)
                .ToListAsync();

            var comments = new List<GetCommentResponse>();

            foreach (var p in queryResult)
            {
                comments.Add(new GetCommentResponse
                {
                    Id = p.Id,
                    Text = p.Text,
                    Date = p.Date.ToShortDateString(),
                    AuthorsName = p.Author.UserName!,
                    PostId = p.Post.Id,
                    LikesCount = await context.LikedComments.Where(x => x.CommentId == p.Id).CountAsync()
                });
            }

            return comments;
        }

        public async Task<bool> IsLikedAsync(int id, string username)
        {
            var IsLiked = await context.LikedComments.Where(x =>
            x.UserProfile.UserName == username &&
            x.Comment.Id == id).AnyAsync();

            if (IsLiked)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> AddLikeAsync(int id, string username)
        {
            var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id == id);

            var user = await context.UserProfiles.FirstOrDefaultAsync(u => u.UserName == username);

            context.LikedComments.Add(new LikedComments
            {
                CommentId = id,
                Comment = comment!,
                UserProfileId = user!.Id,
                UserProfile = user
            });

            var result = await context.SaveChangesAsync();

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteLikeAsync(int id, string username)
        {
            var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id == id);

            var user = await context.UserProfiles.FirstOrDefaultAsync(u => u.UserName == username);

            context.LikedComments.Remove(new LikedComments
            {
                CommentId = id,
                Comment = comment!,
                UserProfileId = user!.Id,
                UserProfile = user
            });

            var result = await context.SaveChangesAsync();

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
