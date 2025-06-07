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

        [Parameter] public int PostId { get; set; }

        public bool IsFileSum { get; set; }

        IBrowserFile file;
        byte[]? blob = null;
        Dictionary<string, FileVm> images = new();

        List<int> ProgressBars = new() {0,0,0,0,0,0,0,0,0};
        
        public CreateSumVm _createSum = new();

        protected override async Task OnParametersSetAsync()
        {
            images = new Dictionary<string, FileVm>();
            file = null;
        }

        async Task IncreasePercent(
            int ticks,
            int progBarIndex = 0)
        {
            var random = new Random();
            var percent = 100 / ticks;
            await Task.Delay(100);
            ProgressBars[progBarIndex] += random.Next(7, 12);

            StateHasChanged();

            for (int i = 0; i < ticks; i++)
            {
                var sec = random.Next(3, 6);
                await Task.Delay(sec * 1000);

                if (ProgressBars[progBarIndex] + percent > 100)
                {
                    ProgressBars[progBarIndex] = 100;
                }
                else
                {
                    ProgressBars[progBarIndex] += percent;
                }

                StateHasChanged();
                await Task.Delay(sec * 200);
            }
        }

        async Task HandleCreatePost()
        {
            if (!IsFileSum)
            {
                await DoSummarization();
            }
            else
            {
                var index = 0;
                foreach (var f in images)
                {
                    await DoSummarization(index, f.Value);
                    index++;
                }
            }
            
            NavigationManager.NavigateTo($"/post/{PostId}");
        }

        async Task DoSummarization(
            int progBarIndex = 0,
            FileVm? file = null)
        {
            var progressTask = IncreasePercent(IsFileSum ? 3 : 1, progBarIndex);
            _createSum.PostId = PostId;
            await postService.CreateSummarizationAsync(_createSum, file);
            await progressTask;
        }

        async Task OnInputFileChange(InputFileChangeEventArgs args)
        {
            file = args.File;
            var buffers = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffers);
            var fileName = file.Name;
            images.Add(fileName, new FileVm
            {
                File = buffers,
                FileProgressBar = 0,
                FileName = fileName
            });
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