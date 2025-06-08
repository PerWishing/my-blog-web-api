using Microsoft.AspNetCore.Components;
using MyBlog.BlazorApp.Models.User;
using MyBlog.BlazorApp.Services.Post;
using MyBlog.BlazorApp.Services.User;

namespace MyBlog.BlazorApp.Pages.Summarizations
{
    public partial class ProjectSubs
    {
        [Inject] private IPostService PostService { get; set; } = null!;
        

        public List<string> Usernames = new List<string>();
        
        [Parameter]
        public int PostId { get; set; }

        [Parameter] public int Page { get; set; }
        [Parameter] public string Search { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await ResetParametersAsync();
            if (Page == 0)
            {
                Page = 1;
            }
            
            Usernames = (await PostService.GetProjectSubsAsync(PostId))!.Usernames;

            StateHasChanged();
            await base.OnParametersSetAsync();
        }

        async Task ResetParametersAsync()
        {
            Usernames = new List<string>();
        }

        public async Task RefreshComponentAsync()
        {
            await OnParametersSetAsync();
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

        async Task PerformSearch()
        {
            Page = 1;
            await RefreshComponentAsync();
        }
    }
}