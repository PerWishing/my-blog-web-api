using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MyBlog.BlazorApp.Models.Post;
using MyBlog.BlazorApp.Services.Post;

namespace MyBlog.BlazorApp.Pages.Posts
{
    public partial class CreatePost
    {
        [Inject] private IPostService postService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        

        public CreatePostVm _createPost = new();

        protected override async Task OnParametersSetAsync()
        {
        }

        async Task HandleCreatePost()
        {
            var id = await postService.CreateProjectAsync(_createPost);
            if (id != null)
            {
                NavigationManager.NavigateTo($"/post/{id}");
            }
        }
    }
}