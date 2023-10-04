using Microsoft.AspNetCore.Components;
using MyBlog.BlazorApp.Models.Post;
using MyBlog.BlazorApp.Services.Post;

namespace MyBlog.BlazorApp.Pages.Posts
{
    public partial class PostCard
    {
        [Inject]
        private IPostService postService { get; set; } = null!;
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;
        [Parameter]
        public int PostId { get; set; }

        public PostDto _post = new PostDto();

        protected override async Task OnParametersSetAsync()
        {
            var apiPosts = await postService.GetPostAsync(PostId);

            if (apiPosts != null)
            {
                _post = apiPosts;
            }
        }
    }
}
