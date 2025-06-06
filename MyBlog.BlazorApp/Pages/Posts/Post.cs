using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyBlog.BlazorApp.Models.Post;
using MyBlog.BlazorApp.Services.Post;

namespace MyBlog.BlazorApp.Pages.Posts
{
    public partial class Post
    {
        [Inject]
        private IJSRuntime Js { get; set; } = null!;
        [Inject]
        private IPostService postService { get; set; } = null!;
        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; } = null!;
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;
        [Parameter]
        public int Id { get; set; }
        public bool IsAuth { get; set; }
        public bool IsAuthor { get; set; }
        public bool IsSaved { get; set; }
        public bool IsRendered { get; set; }
        public bool ShowDelPop { get; set; }
        public string Username { get; set; } = "";

        public PostVm _post = new PostVm();

        protected override async Task OnParametersSetAsync()
        {
            await ResetParametersAsync();

            var apiPost = await postService.GetPostAsync(Id);

            var authResult = await AuthStateProvider.GetAuthenticationStateAsync();
            IsAuth = authResult.User.Identity!.IsAuthenticated;

            if (IsAuth)
            {
                IsSaved = await postService.IsSavedPostAsync(Id);
                IsAuthor = authResult.User.Identity!.Name == apiPost!.AuthorsName;
                Username = authResult.User.Identity!.Name!;
            }

            if (apiPost != null)
            {
                _post = apiPost;
            }
            IsRendered = true;
        }

        async Task SavePostAsync()
        {
            await postService.SavePostAsync(Id);
            IsSaved = true;
        }

        async Task UnsavePostAsync()
        {
            await postService.DeleteSavedPostAsync(Id);
            IsSaved = false;
        }

        async Task DeletePostAsync()
        {
            await postService.DeletePostAsync(Id);
            NavigationManager.NavigateTo("/user");
        }

        async Task DownloadInputSummarization()
        {
            var fileStream = await postService.DownloadInputSummarizationAsync(Id);

            if (fileStream != null)
            {
                using var streamRef = new DotNetStreamReference(stream: fileStream);

                await Js.InvokeVoidAsync("downloadFileFromStream", _post.InputFileName, streamRef);
            }
        }
        
        async Task EditPostAsync()
        {
            NavigationManager.NavigateTo($"/edit-post/{Id}");
        }

        async Task ResetParametersAsync()
        {
            IsAuth = false;
            IsAuthor = false;
            IsSaved = false;
            ShowDelPop = false;
            IsRendered = false;
            Username = "";
        }
    }
}
