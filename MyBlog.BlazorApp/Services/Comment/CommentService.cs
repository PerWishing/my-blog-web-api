using MyBlog.BlazorApp.Models.Comments;
using MyBlog.BlazorApp.Models.Post;
using MyBlog.BlazorApp.Models.User;
using System.Text;
using System.Text.Json;

namespace MyBlog.BlazorApp.Services.Comment
{
    public class CommentService : ICommentService
    {
        private readonly HttpClient httpClient;

        public CommentService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IList<CommentDto>?> GetCommentsAsync(int postId)
        {
            try
            {
                var apiResponse = await httpClient.GetStreamAsync($"api/comments/{postId}");

                var comments = await JsonSerializer.DeserializeAsync<IList<CommentDto>>(apiResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                return comments;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> CreateCommentAsync(CreateCommentDto post)
        {
            try
            {
                post.AuthorsName = "";
                var itemJson = new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, "application/json");

                var apiResponse = await httpClient.PostAsync("api/comment/create", itemJson);

                if (apiResponse.IsSuccessStatusCode)
                {
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> DeleteAsync(int id)
        {
            try
            {
                var apiResponse = await httpClient.DeleteAsync($"api/comments/delete/{id}");

                if (apiResponse.IsSuccessStatusCode)
                {
                    return null;
                }
                return "Can't delete comment.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> IsLikedAsync(int id)
        {
            try
            {
                var apiResponse = await httpClient.GetStreamAsync($"api/comments/is-liked/{id}");
                var isLiked = await JsonSerializer.DeserializeAsync<bool>(apiResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });
                if (isLiked)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task<string?> LikeAsync(int id)
        {
            try
            {
                var itemJson = new StringContent(JsonSerializer.Serialize(id), Encoding.UTF8, "application/json");
                var apiResponse = await httpClient.PostAsync($"api/comments/like/{id}", itemJson);

                if (apiResponse.IsSuccessStatusCode)
                {
                    return null;
                }
                return "Can't like comment.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> UnlikeAsync(int id)
        {
            try
            {
                var apiResponse = await httpClient.DeleteAsync($"api/comments/unlike/{id}");

                if (apiResponse.IsSuccessStatusCode)
                {
                    return null;
                }
                return "Can't like comment.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}
