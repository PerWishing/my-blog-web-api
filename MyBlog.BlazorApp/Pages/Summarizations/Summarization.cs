using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyBlog.BlazorApp.Models.Post;
using MyBlog.BlazorApp.Models.Summarizations;
using MyBlog.BlazorApp.Services.Post;

namespace MyBlog.BlazorApp.Pages.Summarizations
{
    public partial class Summarization
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
        public bool IsRendered { get; set; }
        public bool ShowDelPop { get; set; }
        public string Username { get; set; } = "";

        public SumVm _sum = new SumVm();

        protected override async Task OnParametersSetAsync()
        {
            await ResetParametersAsync();

            var sum = await postService.GetSumAsync(Id);

            var authResult = await AuthStateProvider.GetAuthenticationStateAsync();
            IsAuth = authResult.User.Identity!.IsAuthenticated;

            if (IsAuth)
            {
                Username = authResult.User.Identity!.Name!;
            }

            if (sum != null)
            {
                _sum = sum;
            }
            
            IsRendered = true;
        }

        async Task DownloadInputSummarization()
        {
            var fileStream = await postService.DownloadInputSummarizationAsync(Id);

            if (fileStream != null)
            {
                using var streamRef = new DotNetStreamReference(stream: fileStream);

                await Js.InvokeVoidAsync("downloadFileFromStream", _sum.InputFileName, streamRef);
            }
        }
        
        async Task DownloadOutputSummarization()
        {
            var fileStream = await postService.DownloadOutputSummarizationAsync(Id);

            if (fileStream != null)
            {
                using var streamRef = new DotNetStreamReference(stream: fileStream);

                await Js.InvokeVoidAsync("downloadFileFromStream", $"Суммаризация {_sum.InputFileName}", streamRef);
            }
        }
        
        async Task EditPostAsync()
        {
            NavigationManager.NavigateTo($"/edit-post/{Id}");
        }

        async Task ResetParametersAsync()
        {
            IsAuth = false;
            ShowDelPop = false;
            IsRendered = false;
            Username = "";
        }
    }
}
