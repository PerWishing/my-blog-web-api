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

        IBrowserFile file;
        byte[]? blob = null;
        Dictionary<string, byte[]> images = new Dictionary<string, byte[]>();
        public bool IsDisabled = false;
        public bool IsProgressBarDisabled = true;
        public int ProgressBarPercent = 0;

        public CreatePostVm _createPost = new CreatePostVm();

        protected override async Task OnParametersSetAsync()
        {
            images = new Dictionary<string, byte[]>();
            file = null;
        }

        async void IncreasePercent(int ticks)
        {
            var random = new Random();
            var percent = 100 / ticks;
            await Task.Delay(100);
            ProgressBarPercent += random.Next(7, 12);
            
            StateHasChanged();
            
            for (int i = 0; i < ticks; i++)
            {
                var sec = random.Next(3, 6);
                await Task.Delay(sec * 1000);

                if (ProgressBarPercent + percent > 100)
                {
                    ProgressBarPercent = 100;
                }
                else
                {
                    ProgressBarPercent += percent;
                }

                StateHasChanged();
            }
        }

        async Task HandleCreatePost()
        {
            IsProgressBarDisabled = false;
            IncreasePercent(3);
            var id = await postService.CreateSummarizationPostAsync(_createPost, images.Values);
            if (id != null)
            {
                NavigationManager.NavigateTo($"/post/{id}");
            }
        }

        async Task OnInputFileChange(InputFileChangeEventArgs args)
        {
            file = args.File;
            var buffers = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffers);
            var fileName = file.Name;
            images.Add(fileName, buffers);
            IsDisabled = true;
        }

        void HandleDeleteImg(string imgKey)
        {
            images.Remove(imgKey);
            IsDisabled = false;
        }
    }
}