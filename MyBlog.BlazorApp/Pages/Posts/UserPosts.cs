using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyBlog.BlazorApp.Models.Post;
using MyBlog.BlazorApp.Services.Post;

namespace MyBlog.BlazorApp.Pages.Posts
{
    public partial class UserPosts
    {
        [Inject]
        private IPostService postService { get; set; } = null!;
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;
        [CascadingParameter(Name = "Username")]
        public string? Username { get; set; }
        [CascadingParameter(Name = "IsSaved")]
        public bool IsSaved { get; set; }
        [Parameter]
        public int Page { get; set; }

        public PostsPageVm _posts = new PostsPageVm();

        [Inject]
        private IJSRuntime JS { get; set; } = null!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await DoMasonryJavaScript();
        }

        private async Task DoMasonryJavaScript()
        {
            await Task.Delay(1000);
            await JS.InvokeVoidAsync("DoMasonry");
        }

        protected override async Task OnParametersSetAsync()
        {
            await ResetParametersAsync();

            if (Page == 0) { Page = 1; }
            var apiPosts = new PostsPageVm();
            if (!IsSaved)
            {
                apiPosts = await postService.GetUserPostsAsync(Username!, Page);
            }
            else
            {
                apiPosts = await postService.GetUserSavedPostsAsync(Username!, Page);
            }

            if (apiPosts != null)
            {
                _posts = apiPosts;
            }
        }

        async Task OnNextPage()
        {
            Page += 1;
            await RefreshComponentAsync();
        }
        async Task OnPrevPage()
        {
            Page -= 1;
            await RefreshComponentAsync();
        }

        async Task ResetParametersAsync()
        {
            _posts = new PostsPageVm();
        }

        public async Task RefreshComponentAsync()
        {
            await OnParametersSetAsync();
        }
    }
}
