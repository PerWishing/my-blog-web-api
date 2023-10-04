using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyBlog.BlazorApp.Models.Post;
using MyBlog.BlazorApp.Services.Post;

namespace MyBlog.BlazorApp.Pages.Posts
{
    public partial class AllPosts
    {
        [Inject]
        private IPostService postService { get; set; } = null!;
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        [Parameter]
        public int Page { get; set; }
        [Parameter]
        public string Search { get; set; }

        public PostsPageDto _posts = new PostsPageDto();
        
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
            var apiPosts = new PostsPageDto();
            apiPosts = await postService.GetAllPostsAsync(Search, Page);

            if (apiPosts != null)
            {
                _posts = apiPosts;
            }
        }

        async Task OnNextPage()
        {
            Page += 1;
            if (string.IsNullOrEmpty(Search))
                NavigationManager.NavigateTo("all-posts/" + Page);
            else
                NavigationManager.NavigateTo("all-posts/" + Search + "/" + Page);
        }

        async Task OnPrevPage()
        {
            Page -= 1;
            if (string.IsNullOrEmpty(Search))
                NavigationManager.NavigateTo("all-posts/" + Page);
            else
                NavigationManager.NavigateTo("all-posts/" + Search + "/" + Page);
        }

        async Task PerformSearch()
        {
            NavigationManager.NavigateTo("all-posts/" + Search + "/1");
        }

        async Task ResetParametersAsync()
        {
            _posts = new PostsPageDto();
        }

        public async Task RefreshComponentAsync()
        {
            await OnParametersSetAsync();
        }
    }
}
