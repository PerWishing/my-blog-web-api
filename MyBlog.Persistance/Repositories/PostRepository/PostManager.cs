using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.Persistance.Database;
using MyBlog.Persistance.Identity;
using MyBlog.Domain.Entities;

namespace MyBlog.Persistance.Repositories.PostRepository
{
    public partial class PostManager : IPostManager
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly float pageResults = 10f;
        private readonly float pageResultsForUserPosts = 4f;

        public PostManager(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<int> CreateAsync(CreatePostRequest request)
        {
            var author = await context.UserProfiles.FirstOrDefaultAsync(u => u.UserName == request.AuthorsName!);
            if (author == null)
            {
                return 0;
            }

            var post = new Post
            {
                Title = request.Title,
                Text = request.Text,
                PublishDate = DateTime.Now,
                Author = author
            };

            context.Add(post);
            await context.SaveChangesAsync();

            int id = post.Id;
            return id;
        }

        public async Task<bool> UpdateAsync(UpdatePostRequest request)
        {
            var updatedPost = await context.Posts.FirstOrDefaultAsync(x => x.Id == request.Id);

            updatedPost!.Title = request.Title;
            updatedPost!.Text = request.Text;
            updatedPost!.PublishDate = DateTime.Now;

            context.Update(updatedPost);
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

        public async Task<bool> DeleteAsync(int id)
        {
            var postForDelete = await context.Posts.FirstOrDefaultAsync(x => x.Id == id);

            if (postForDelete == null)
            {
                return false;
            }

            context.Remove(postForDelete);
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

        public async Task<GetPostsPageResponse> GetAllAsync(int page)
        {
            var queryCount = await context.Posts
                .Include(x => x.Author)
                .Where(x => x.Author.IsBlocked == false)
                .CountAsync();

            var pageCount = Math.Ceiling(queryCount / pageResults);

            var queryResult = await context.Posts.OrderByDescending(x => x.PublishDate)
                .Include(x => x.Author)
                .Where(x => x.Author.IsBlocked == false)
                .Include(x => x.Images)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var posts = new List<GetPostResponse>();

            foreach (var p in queryResult)
            {
                var post = new GetPostResponse
                {
                    Id = p.Id,
                    Title = p.Title,
                    Text = p.Text,
                    PublishDate = p.PublishDate,
                    AuthorsName = p.Author.UserName!
                };

                posts.Add(post);
            }

            var GetPostsPageResponse = new GetPostsPageResponse
            {
                PageCount = (int)pageCount,
                Posts = posts
            };
            return GetPostsPageResponse;
        }

        public async Task<GetPostsPageResponse> GetAllByAuthorAsync(string author, int page)
        {
            var queryCount = await context.Posts.Include(x => x.Author)
                .Where(x => x.Author.UserName == author)
                .Where(x => x.Author.IsBlocked == false)
                .CountAsync();

            var pageCount = Math.Ceiling(queryCount / pageResultsForUserPosts);

            var queryResult = await context.Posts
                .Where(x => x.Author.UserName == author)
                .Where(x => x.Author.IsBlocked == false)
                .OrderByDescending(x => x.PublishDate)
                .Include(x => x.Author)
                .Skip((page - 1) * (int)pageResultsForUserPosts)
                .Take((int)pageResultsForUserPosts)
                .ToListAsync();

            var posts = new List<GetPostResponse>();

            foreach (var p in queryResult)
            {
                var post = new GetPostResponse
                {
                    Id = p.Id,
                    Title = p.Title,
                    Text = p.Text,
                    PublishDate = p.PublishDate,
                    AuthorsName = p.Author.UserName!
                };

                posts.Add(post);
            }

            var GetPostsPageResponse = new GetPostsPageResponse
            {
                PageCount = (int)pageCount,
                Posts = posts
            };
            return GetPostsPageResponse;
        }

        public async Task<GetPostsPageResponse> GetAllBySearchAsync(int page, string searchString)
        {
            searchString = searchString.ToUpper();

            var queryCount = await context.Posts
                .Include(x => x.Author)
                .Where(x => x.Author.IsBlocked == false)
                .Where(p => EF.Functions.Like(p.Author.UserName!.ToUpper(), $"%{searchString}%")
                || EF.Functions.Like(p.Title!.ToUpper(), $"%{searchString}%")
                || EF.Functions.Like(p.Text!.ToUpper(), $"%{searchString}%")
                )
                .CountAsync();

            var pageCount = Math.Ceiling(queryCount / pageResults);

            var postsQuery = await context.Posts
            .Include(x => x.Author)
            .Where(x => x.Author.IsBlocked == false)
                .Where(p => EF.Functions.Like(p.Author.UserName!.ToUpper(), $"%{searchString}%")
                || EF.Functions.Like(p.Title!.ToUpper(), $"%{searchString}%")
                || EF.Functions.Like(p.Text!.ToUpper(), $"%{searchString}%")
                )
                .OrderByDescending(x => x.PublishDate)
                .Include(x => x.Images)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var posts = new List<GetPostResponse>();

            foreach (var p in postsQuery)
            {
                var post = new GetPostResponse
                {
                    Id = p.Id,
                    Title = p.Title,
                    Text = p.Text,
                    PublishDate = p.PublishDate,
                    AuthorsName = p.Author.UserName!
                };

                posts.Add(post);
            }

            var GetPostsPageResponse = new GetPostsPageResponse
            {
                PageCount = (int)pageCount,
                Posts = posts
            };

            return GetPostsPageResponse;
        }

        public async Task<GetPostResponse?> GetByIdAsync(int id)
        {
            var queryResult = await context.Posts.Include(x => x.Author)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (queryResult == null)
            {
                return null;
            }

            var post = new GetPostResponse
            {
                Id = id,
                Title = queryResult!.Title,
                Text = queryResult.Text,
                PublishDate = queryResult.PublishDate,
                AuthorsName = queryResult.Author.UserName!
            };

            return post;
        }

        public async Task<int> SavesCountAsync(int id)
        {
            return await context.SavedPosts.Where(x => x.PostsId == id).CountAsync();
        }

        public async Task<GetPostsPageResponse> GetAllSavedAsync(string username, int page)
        {
            var queryCount = await context.SavedPosts
                .Include(x => x.UserProfile)
                .Where(x => x.UserProfile.UserName == username)
                .Include(x => x.Post)
                .ThenInclude(x => x.Author)
                .Where(x => x.Post.Author.IsBlocked == false)
                .CountAsync();

            var pageCount = Math.Ceiling(queryCount / pageResultsForUserPosts);

            var queryResult = await context.SavedPosts
                .Include(x => x.UserProfile)
                .Where(x => x.UserProfile.UserName == username)
                .Include(x => x.Post)
                .ThenInclude(x => x.Author)
                .Where(x => x.Post.Author.IsBlocked == false)
                .OrderByDescending(x => x.Post.PublishDate)
                .Skip((page - 1) * (int)pageResultsForUserPosts)
                .Take((int)pageResultsForUserPosts)
                .ToListAsync();

            var posts = new List<GetPostResponse>();

            foreach (var p in queryResult)
            {
                var post = new GetPostResponse
                {
                    Id = p.Post.Id,
                    Title = p.Post.Title,
                    Text = p.Post.Text,
                    PublishDate = p.Post.PublishDate,
                    AuthorsName = p.Post.Author.UserName!
                };

                posts.Add(post);
            }

            var GetPostsPageResponse = new GetPostsPageResponse
            {
                PageCount = (int)pageCount,
                Posts = posts
            };

            return GetPostsPageResponse;
        }

        public async Task<bool> IsSavedAsync(int id, string username)
        {
            var isSaved = await context.SavedPosts.Where(x =>
            x.UserProfile.UserName == username &&
            x.Post.Id == id).AnyAsync();

            if (isSaved)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> AddToSavedAsync(int id, string username)
        {
            var post = await context.Posts.FirstOrDefaultAsync(x => x.Id == id);

            var user = await context.UserProfiles.FirstOrDefaultAsync(u => u.UserName == username);

            context.SavedPosts.Add(new SavedPosts
            {
                PostsId = id,
                Post = post!,
                UserProfileId = user!.Id,
                UserProfile = user!
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

        public async Task<bool> DeleteFromSavedAsync(int id, string username)
        {
            var post = await context.Posts.FirstOrDefaultAsync(x => x.Id == id);

            var user = await context.UserProfiles.FirstOrDefaultAsync(u => u.UserName == username);

            context.SavedPosts.Remove(new SavedPosts
            {
                PostsId = id,
                Post = post!,
                UserProfileId = user!.Id,
                UserProfile = user!
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
