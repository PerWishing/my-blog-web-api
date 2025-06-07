using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MyBlog.BlazorApp.Models.Post;
using MyBlog.BlazorApp.Models.Summarizations;
using MyBlog.BlazorApp.Services.Post;

namespace MyBlog.BlazorApp.Pages.Summarizations
{
    public partial class CreateSummarization
    {
        [Inject] private IPostService postService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        [Parameter]
        public int PostId { get; set; }

        public bool IsFileSum { get; set; }
        
        IBrowserFile file;
        byte[]? blob = null;
        Dictionary<string, byte[]> images = new();
        public bool IsProgressBarDisabled = true;
        public int ProgressBarPercent = 0;

        public CreateSumVm _createSum = new();

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
            _createSum.PostId = PostId;
            var id = await postService.CreateSummarizationAsync(_createSum, images);
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
        }

        void HandleDeleteImg(string imgKey)
        {
            images.Remove(imgKey);
        }

        void ToggleSumType()
        {
            IsFileSum = !IsFileSum;
            StateHasChanged();
        }
    }
}